using MongoDB.Bson;
using Rdio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Rdio.Util;
using MongoDB.Driver;

namespace Rdio.Repository
{
    public class ContentManagerRepository
    {
        public async Task<bool> EditSite(Models.ContentManager.Site model)
        {
            try
            {
                var _id = ObjectId.GenerateNewId().ToString();
                if (!string.IsNullOrWhiteSpace(model._id))
                    _id = model._id;
                var res = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'sites',updates:[{q:{_id:ObjectId('" + _id + "')},u:{$set:{_id:ObjectId('" + _id + "'),userid:'"+Rdio.Util.Common.My.id+"',title:'" + model.title + "',createdateticks:" + DateTime.Now.Ticks+ ",url:'" + model.url + "'}},upsert:true}]}");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<Models.ContentManager.Site> SiteInfo(string id)
        {
            var model = new Models.ContentManager.Site() {
                createdateticks=0,
                title=string.Empty,
                url = string.Empty,
                userid = string.Empty,
                _id = string.Empty
            };

            try
            {
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'sites',pipeline:[{$match:{_id:ObjectId('" + id + "')}},{$limit:1}]}");
                if (_model.GetValue("result").AsBsonArray.Any())
                    model = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Site>(_model.GetValue("result")[0].AsBsonDocument);
            }
            catch (Exception ex)
            {
            }
            return model;

        }
        public async Task<Models.ContentManager.Rss> RssInfo(string id)
        {
            var model = new Models.ContentManager.Rss() {
                _id=string.Empty,
                categories = new List<string>(),
                createdateticks = 0,
                lang = string.Empty,
                siteid = string.Empty,
                tags = new List<string>(),
                title = string.Empty,
                url = string.Empty,
                userid = string.Empty
            };
            try
            {
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'rss',pipeline:[{$match:{_id:ObjectId('" + id + "')}},{$limit:1}]}");
                if (_model.GetValue("result").AsBsonArray.Any())
                    model = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Rss>(_model.GetValue("result")[0].AsBsonDocument);
            }
            catch (Exception ex)
            {
            }
            return model;

        }
        public async Task<bool> EditRss(Models.ContentManager.Rss model)
        {
            try
            {
                var _id = ObjectId.GenerateNewId().ToString();
                if (!string.IsNullOrWhiteSpace(model._id))
                    _id = model._id;
                var res = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'rss',updates:[{q:{_id:ObjectId('" + _id + "')},u:{$set:{_id:ObjectId('" + _id + "'),siteid:'"+model.siteid+"',userid:'" + Rdio.Util.Common.My.id + "',title:'" + model.title + "',createdateticks:" + DateTime.Now.Ticks + ",url:'" + model.url + "',lang:'"+model.lang+ "',tags:"+model.tags.toJSON()+ ",categories:" + model.categories.toJSON() + ",lastcrawldate:"+0+"}},upsert:true}]}");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Models.ContentManager.Rss>> DequeRss(int count) {
            var model = new List<Models.ContentManager.Rss>();
            try
            {
                //TODO : Change lastcrawldate $lt DateTime.Now.addMinuates(-5).Ticks OR (-any)
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'rss',pipeline:[{$match:{lastcrawldate:{$lt:" + DateTime.Now.Ticks + "}}},{$sort : {'_id' : -1 }},{$limit:" + count + "}]}");
                //var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'rss',pipeline:[{$match:{}},{$sort : {'_id' : -1 }},{$limit:" + count + "}]}");

                if (_model.GetValue("result").AsBsonArray.Any())
                    foreach (var item in _model.GetValue("result").AsBsonArray)
                        model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Rss>(item.AsBsonDocument));
            }
            catch (Exception ex)
            {
            }
            return model;

        }
        public async Task<bool> ChangeLastCarawlDateRss(List<Models.ContentManager.Rss> model)
        {
            try
            {
                ObjectId[] objarray = new ObjectId[model.Count];
                var objectidArrayFormated = "[";
                foreach (var item in model)
                    objectidArrayFormated += string.Format("ObjectId('{0}'),",item._id);
                objectidArrayFormated = objectidArrayFormated.Trim(',') + "]";
                var d = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'rss',updates:[{q:{_id:{$in:" + objectidArrayFormated + "}},u:{$set:{'lastcrawldate':NumberLong(" + DateTime.Now.Ticks + ")}},upsert:false,multi:true}]}");


                //var collection = NoSql.Instance.GetCollection<BsonDocument>("rss");
                //var update = Builders<BsonDocument>.Update.Set("lastcrawldate", DateTime.Now.Ticks);
                //await collection.UpdateManyAsync(;
                //var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'rss',pipeline:[{$match:{lastcrawldate:{$lt:" + DateTime.Now.Ticks + "}}},{$sort : {'_id' : -1 }},{$limit:" + count + "}]}");
                //if (_model.GetValue("result").AsBsonArray.Any())
                //    foreach (var item in _model.GetValue("result").AsBsonArray)
                //        model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Rss>(item.AsBsonDocument));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<Models.ContentManager.Rss>> GetUserAllRss(string id)
        {
            var model =new List<Models.ContentManager.Rss>();
            try
            {
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'rss',pipeline:[{$match:{userid:'" + id + "'}}]}");
                if (_model.GetValue("result").AsBsonArray.Any())
                    foreach (var item in _model.GetValue("result").AsBsonArray)
                        model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Rss>(item.AsBsonDocument));

            }
            catch (Exception ex)
            {
            }
            return model;

        }
        public async Task<List<Models.ContentManager.Site>> GetUserAllSite(string id)
        {
            var model = new List<Models.ContentManager.Site>();
            try
            {
                var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'sites',pipeline:[{$match:{userid:'" + id + "'}}]}");
                if (_model.GetValue("result").AsBsonArray.Any())
                    foreach (var item in _model.GetValue("result").AsBsonArray)
                        model.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.ContentManager.Site>(item.AsBsonDocument));

            }
            catch (Exception ex)
            {
            }
            return model;

        }
        public async Task<bool> EditTemplate(string siteid,Models.Crawl.CrawlTemplate model)
        {
            try
            {
                var _id = ObjectId.GenerateNewId().ToString();
                if (!string.IsNullOrWhiteSpace(model._id))
                {
                    _id = model._id;
                    var res = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'sites',updates:[{q:{_id:ObjectId('" + siteid + "')},u:{$set:{'template.0.name':'" + model.name + "','template.0.type':'" + model.type + "','template.0.sampleurl':'" + model.sampleurl + "','template.0.structure':" + model.structure.toJSON() + "}},upsert:false}]}");
                    //var res = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'sites',updates:[{q:{_id:ObjectId('" + siteid + "')},u:{$set:{template.$.name:'" + model.name + "'}},upsert:false}]}");

                }
                else
                {
                    var res = await NoSql.Instance.RunCommandAsync<BsonDocument>("{update:'sites',updates:[{q:{_id:ObjectId('" + siteid + "')},u:{$push:{template:{_id:ObjectId('" + _id + "'),name:'" + model.name + "',type:'" + model.type + "',sampleurl:'" + model.sampleurl + "',structure:" + model.structure.toJSON() + "}}},upsert:true}]}");
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //public async Task<Models.Crawl.CrawlTemplate> CrawlTemplate(string siteid)
        //{
        //    var model = new Models.Crawl.CrawlTemplate();

        //    try
        //    {
        //        var _model = await NoSql.Instance.RunCommandAsync<BsonDocument>("{aggregate:'sites',pipeline:[{$match:{_id:ObjectId('" + siteid + "')}},{$limit:1}]}");
        //        if (_model.GetValue("result").AsBsonArray.Any())
        //            model = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Models.Crawl.CrawlTemplate>(_model.GetValue("result")[0].AsBsonDocument);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return model;

        //}

    }
}