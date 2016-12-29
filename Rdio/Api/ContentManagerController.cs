﻿using MongoDB.Bson;
using Rdio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rdio.Api
{
    public class ContentManagerController : ApiController
    {
        Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();

        [Authorize]
        [Route("api/ContentManager/EditSite")]
        public async Task<ViewModel.ServiceResult> PostEditSite([FromBody]ViewModel.ContentManager.SiteVM model)
        {
            try
            {
                var res = await ContentManagerRepository.EditSite(new Models.ContentManager.Site()
                {
                    _id = model._id,
                    title = model.title,
                    url = model.url
                });

                if (res)
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                else
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.FaildMessage.ToString()
                    };
            }
            catch (Exception ex)
            {
                return new ViewModel.ServiceResult()
                {
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Authorize]
        [Route("api/ContentManager/SiteManage")]
        public async Task<ViewModel.ContentManager.SimpleSiteVM> GetSiteManage(int page)
        {
            try
            {
                page = page < 1 ? 1 : page;
                var limit = 20;
                var skip = limit * (page - 1);

                string q = "{aggregate:'sites',pipeline:[{$match:{'userid':'"+Rdio.Util.Common.My.id+"'}},{$sort : { 'createdateticks' : -1  }},{$skip:" + skip.ToString() + "},{$limit:" + limit.ToString() + "}]}";
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>(q);
                var model = new List<Models.ContentManager.Site>();
                foreach (var item in _model.GetValue("result").AsBsonArray)
                    model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Site>(item.AsBsonDocument));
                var result = new ViewModel.ContentManager.SimpleSiteVM();
                result.Data = model;
                result.CurrentPage = page;
                result.PrevPage = (page == 1 ? 2 : page) - 1;
                result.NextPage = page + 1;
                result.ServiceResultStatus = (int)Util.Common.ServiceResultStatus.OK;
                return result;
            }
            catch (Exception ex)
            {
                return new ViewModel.ContentManager.SimpleSiteVM()
                {
                    ServiceResultStatus = (int)Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Authorize]
        [Route("api/ContentManager/RssManage")]
        public async Task<ViewModel.ContentManager.SimpleRssVM> GetRssManage(int page,string siteid)
        {
            try
            {
                page = page < 1 ? 1 : page;
                var limit = 20;
                var skip = limit * (page - 1);

                string q = "{aggregate:'rss',pipeline:[{$match:{'userid':'" + Rdio.Util.Common.My.id + "',siteid:'"+ siteid + "'}},{$sort : { '_id' : -1  }},{$skip:" + skip.ToString() + "},{$limit:" + limit.ToString() + "}]}";
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>(q);
                var model = new List<Models.ContentManager.Rss>();
                foreach (var item in _model.GetValue("result").AsBsonArray)
                    model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Rss>(item.AsBsonDocument));
                var result = new ViewModel.ContentManager.SimpleRssVM();
                result.Data = model;
                result.CurrentPage = page;
                result.PrevPage = (page == 1 ? 2 : page) - 1;
                result.NextPage = page + 1;
                result.ServiceResultStatus = (int)Util.Common.ServiceResultStatus.OK;
                return result;
            }
            catch (Exception ex)
            {
                return new ViewModel.ContentManager.SimpleRssVM()
                {
                    ServiceResultStatus = (int)Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Authorize]
        [Route("api/ContentManager/EditRss")]
        public async Task<ViewModel.ServiceResult> PostEditRss([FromBody]ViewModel.ContentManager.RssVM model)
        {
            try
            {
                var res = await ContentManagerRepository.EditRss(new Models.ContentManager.Rss()
                {
                    _id = model._id,
                    title = model.title,
                    url = model.url,
                    siteid=model.siteid,
                    lang=model.lang,
                    tags=model.tags,
                    categories=model.categories
                });

                if (res)
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                else
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.FaildMessage.ToString()
                    };
            }
            catch (Exception ex)
            {
                return new ViewModel.ServiceResult()
                {
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Authorize]
        [Route("api/ContentManager/EditTemplate")]
        public async Task<ViewModel.ServiceResult> PostEditTemplate([FromBody]ViewModel.ContentManager.EditTemplateVM model)
        {
            try
            {
                var res = await ContentManagerRepository.EditTemplate(model.siteid,new Models.Crawl.CrawlTemplate()
                {
                    _id = model._id,
                    name = model.name,
                    type = model.type,
                    sampleurl = model.sampleurl,
                    structure = model.structure
                });

                if (res)
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                else
                    return new ViewModel.ServiceResult()
                    {
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.FaildMessage.ToString()
                    };
            }
            catch (Exception ex)
            {
                return new ViewModel.ServiceResult()
                {
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }
    }
}
