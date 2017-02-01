using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Rdio.Service
{
    public class CrawlerService
    {
        Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();
        Repository.ContentRepository ContentRepository = new Repository.ContentRepository();
        Repository.BaseContentRepository baseContentRepository = new Repository.BaseContentRepository();
        Service.CacheService cacheService = new CacheService();

        public async Task<bool> CrawlLinkManager()
        {
            if (!IsInProccess(CrawlLinkIsInProccessCacheKey))
            {
                try
                {
                    SetScheduleInProccess(SchedulerStat.inProccess, CrawlLinkIsInProccessCacheKey);
                    Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();
                    Repository.BaseContentRepository BaseContentRepository = new Repository.BaseContentRepository();
                    var SuccessList = new List<Models.ContentManager.Site>();
                    var BaseContentList = new List<Models.BaseContent.BaseContent>();
                    var deque = await ContentManagerRepository.DequeSite(10);
                    foreach (var item in deque)
                    {
                        var rssModel = (await ContentManagerRepository.GetSiteAllRss(item._id)).FirstOrDefault(q=>string.IsNullOrEmpty(q.url));
                        var res = await CrawlerLink(item);
                        if (res != null)
                        {
                            SuccessList.Add(item);
                            foreach (var rss in res)
                                BaseContentList.Add(new Models.BaseContent.BaseContent()
                                {
                                    dateticks = rss.dateticks,
                                    description = rss.description,
                                    insertdateticks = DateTime.Now.Ticks,
                                    rssid = rssModel!=null ? rssModel._id: "",
                                    title = rss.title,
                                    url = rss.url,
                                    userid = item.userid,
                                    bycrawled=true
                                });
                        }
                    }
                    
                    //TODO: Resolve concarrency problem insert repeated if waite for preve task 
                    var addRes = await BaseContentRepository.Add(BaseContentList);
                    var changeRes = await ContentManagerRepository.ChangeLastCarawlDateSite(SuccessList);
                    var AddToRedisRes = await BaseContentRepository.AddRssURlInRedis(BaseContentList);

                    SetScheduleInProccess(SchedulerStat.idle, CrawlLinkIsInProccessCacheKey);
                    return true;
                }
                catch (Exception ex)
                {
                    SetScheduleInProccess(SchedulerStat.idle, CrawlLinkIsInProccessCacheKey);
                    return false;
                }
            }

            return false;
        }
        public async Task<List<Models.BaseRssItem>> CrawlerLink(Models.ContentManager.Site model)
        {
            try
            {
                string htmlContent = "";
                var uri = new Uri(model.url);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0");
                    client.DefaultRequestHeaders.Add("Host", uri.Authority);
                    using (var r = await client.GetAsync(uri))
                    {
                        htmlContent = await r.Content.ReadAsStringAsync();
                    }
                }
                var parser = new HtmlParser();
                var document = parser.Parse(htmlContent);

                var allLink = document.QuerySelectorAll("a").Where(q=>q.Attributes.Where(x=>x.Name=="href").Any(x=>x.Value.Contains(model.url)) || q.Attributes.Where(x => x.Name == "href").Any(x=>x.Value.StartsWith("/")));
                var res = new List<Models.BaseRssItem>();

                foreach (var item in allLink)
                {
                    string title = item.TextContent;
                    string link =new Uri(uri, item.GetAttribute("href")).ToString();
                    string description = "";
                    var datetime = DateTime.Now;
                    if(!string.IsNullOrEmpty(title))
                        res.Add(new Models.BaseRssItem() { title = title, url = link, description = description, dateticks = datetime.Ticks });
                }
                return res;
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public async Task<bool> CrawlManager()
        {
            if (!IsInProccess(CrawlInProccessIsInProccessCacheKey))
            {
                try
                {
                    SetScheduleInProccess(SchedulerStat.inProccess, CrawlInProccessIsInProccessCacheKey);
                    Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();
                    Repository.BaseContentRepository BaseContentRepository = new Repository.BaseContentRepository();
                    var SuccessList = new List<Models.ContentManager.Rss>();
                    var BaseContentList = new List<Models.BaseContent.BaseContent>();
                    var deque = await BaseContentRepository.Deque(10);
                    //TODO : loop in basecontetn and then just pass them to crawl function and those change status and add to content repository possible not true !!!
                    foreach (var item in deque)
                    {
                        var res = await Crawler(item);
                    }
                    
                    SetScheduleInProccess(SchedulerStat.idle, CrawlInProccessIsInProccessCacheKey);
                    return true;
                }
                catch (Exception ex)
                {
                    SetScheduleInProccess(SchedulerStat.idle, CrawlInProccessIsInProccessCacheKey);
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> Crawler(Models.BaseContent.BaseContent model)
        {
            try
            {
                string htmlContent = "";
                var uri = new Uri(model.url);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:50.0) Gecko/20100101 Firefox/50.0");
                    client.DefaultRequestHeaders.Add("Host", uri.Authority);
                    using (var r = await client.GetAsync(uri))
                    {
                        htmlContent = await r.Content.ReadAsStringAsync();
                    }
                }
                var parser = new HtmlParser();
                var document = parser.Parse(htmlContent);

                var rssModel = await ContentManagerRepository.RssInfo(model.rssid);
                var SiteModel = await ContentManagerRepository.SiteInfo(rssModel.siteid);

                //var template = Util.Common.fromJSON<Models.Crawl.CrawlTemplate>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Models/simplecrawltemplate.json")));
                //var template = Util.Common.fromJSON<Models.Crawl.CrawlTemplate>(System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Models/simplecrawltemplate.json")));
                var template = SiteModel.template.FirstOrDefault();

                if(template!=null)
                {
                    var content = new Models.Content.NewsContent();
                    content.rssid = model.rssid;
                    content.userid = model.userid;
                    content.contenttype = (int)Util.Configuration.ContentType.News;
                    content.basecontentid = model._id;
                    content.url = model.url;

                    foreach (var item in template.structure)
                    {
                        var element = document.QuerySelector(item.query);
                        var elementcontent = "";
                        if (element != null && !string.IsNullOrWhiteSpace(item.query))
                        {
                            switch (item.type)
                            {
                                case "innerhtml":
                                    elementcontent = Util.Common.CleanHtmlContent(element.InnerHtml);
                                    break;
                                case "src":
                                    elementcontent = new Uri(uri, element.GetAttribute(item.type)).ToString();
                                    break;
                                default:
                                    break;
                            }
                            try
                            {
                                content.GetType().GetProperty(item.field).SetValue(content, elementcontent, null);
                            }
                            catch
                            {
                            }
                        }
                    }

                    var result = await ContentRepository.AddContent(content);

                    if (result)
                        await baseContentRepository.ChangeIsCrawled(model);
                    return result;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        string CrawlInProccessIsInProccessCacheKey = "CrawlInProccessIsInProccessCacheKey";
        string CrawlLinkIsInProccessCacheKey = "CrawlLinkIsInProccessCacheKey";

        public enum SchedulerStat
        {
            inProccess = 0,
            idle = 1
        }
        public bool IsInProccess(string cachKey)
        {
            try
            {
                var model = cacheService.GetCache(cachKey);
                if (model == null)
                    return false;
                else
                    return int.Parse(model.ToString()) == (int)SchedulerStat.inProccess ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SetScheduleInProccess(SchedulerStat stat, string cachKey)
        {
            cacheService.AddToCache(cachKey, ((int)stat).ToString());
        }
    }
}