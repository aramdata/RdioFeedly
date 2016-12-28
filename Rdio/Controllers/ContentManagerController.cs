using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Rdio.Controllers
{
    public class ContentManagerController : Controller
    {
        Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();

        [Authorize]
        public ActionResult EditSite(string id)
        {
            var model = new ViewModel.ContentManager.SiteVM();
            if (!string.IsNullOrWhiteSpace(id))
                model = new ViewModel.ContentManager.SiteVM();
            return View(model);
        }
        [Authorize]
        public ActionResult SiteManage()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> RssManage(string id,string siteid)
        {
            var siteModel = await ContentManagerRepository.SiteInfo(siteid);
            var rssModel = await ContentManagerRepository.RssInfo(id);

            var model = new ViewModel.ContentManager.RssVM() {
                sitetitle= siteModel.title,
                siteid= siteModel._id,
                categories= rssModel.categories,
                tags = rssModel.tags,
                lang= rssModel.lang,
                title= rssModel.title,
                url= rssModel.url,
                userid= rssModel.userid,
                _id= rssModel._id
            };
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> EditTemplate(string rssid,string templateid)
        {
            var rssModel = await ContentManagerRepository.RssInfo(rssid);

            var model = new ViewModel.ContentManager.TemplateVM()
            {
                rssid = rssModel._id,
                name = "",
                sampleurl = "",
                type = "",
                structures = new List<Tuple<string, string>>()
            };
            return View(model);
        }
        public async Task<ActionResult> mohsen() {
            var rs = new Service.RssService();
            var res=await rs.RssFetcherManager();
            return Content("Salam - "+ res);
        }
    }
}