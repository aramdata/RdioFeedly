using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Rdio.Domain;
using Rdio.Models.Content;
using Rdio.Models.ContentManager;
using Rdio.Repository;

namespace Rdio.Api
{
    public class UserBlogController : ApiController
    {
        Repository.ContentManagerRepository ContentManagerRepository = new Repository.ContentManagerRepository();
        Repository.ContentRepository ContentRepository = new Repository.ContentRepository();
        Service.LequeService LequeService = new Service.LequeService();

        [Route("api/UserBlog/GetCategories")]
        public async Task<ServiceResult<Models.ContentManager.Category>> GetCategories([FromUri]ViewModel.UserBlog.GetCategoriesVM model)
        {
            try
            {
                var res = await ContentManagerRepository.GetUserCategories(model.userid);
                return new ServiceResult<Category>
                {
                    Data = res,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                    ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Category>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetBlockNews")]
        public async Task<ServiceResult<Models.Content.NewsContent>> GetBlockNews([FromUri]ViewModel.UserBlog.GetBlockNewsVM model)
        {
            try
            {
                IEnumerable<Models.Content.NewsContent> res = new List<Models.Content.NewsContent>();

                //Block Of Category News
                if (!string.IsNullOrWhiteSpace(model.BlockCode))
                    res = await ContentRepository.GetBlockContent(await (ContentManagerRepository.GetBlockRssIds(model.CategoryId, model.BlockCode)), model.Count);

                //Categories News
                if (string.IsNullOrWhiteSpace(model.BlockCode))
                    res = await ContentRepository.GetBlockContent(await (ContentManagerRepository.GetCategoryRssIds(model.CategoryId)), model.Count);

                return new ServiceResult<Models.Content.NewsContent>
                {
                    Data = res,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                    ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Content.NewsContent>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetNewsInfo")]
        public async Task<ServiceResult<Models.Content.NewsContent>> GetNewsInfo([FromUri]ViewModel.UserBlog.GetNewsInfo model)
        {
            try
            {
                var res = await ContentRepository.ContentInfo(model.ContentId);
                return new ServiceResult<Models.Content.NewsContent>
                {
                    Data = new List<NewsContent> { res },
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                    ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Content.NewsContent>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetFootbalLegue")]
        public async Task<ServiceResult<Models.Legue.Varzesh3Legue>> GetFootbalLegue([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetFootbalLegue(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = new List<Models.Legue.Varzesh3Legue> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3Legue>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetFootbalLegueFixture")]
        public async Task<ServiceResult<Models.Legue.Varzesh3LegueFixture>> GetFootbalLegueFixture([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetFootbalLegueFixture(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = new List<Models.Legue.Varzesh3LegueFixture> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetVollybalLegue")]
        public async Task<ServiceResult<Models.Legue.Varzesh3Legue>> GetVollybalLegue([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetVollybalLegue(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = new List<Models.Legue.Varzesh3Legue> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3Legue>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }
        [Route("api/UserBlog/GetVollybalLegueFixture")]
        public async Task<ServiceResult<Models.Legue.Varzesh3LegueFixture>> GetVollybalLegueFixture([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetVollybalLegueFixture(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = new List<Models.Legue.Varzesh3LegueFixture> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }


        [Route("api/UserBlog/GetBasketbalLegue")]
        public async Task<ServiceResult<Models.Legue.Varzesh3Legue>> GetBasketbalLegue([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetBasketbalLegue(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = new List<Models.Legue.Varzesh3Legue> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3Legue>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3Legue>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }

        [Route("api/UserBlog/GetBasketbalLegueFixture")]
        public async Task<ServiceResult<Models.Legue.Varzesh3LegueFixture>> GetBasketbalLegueFixture([FromUri]ViewModel.UserBlog.GetFootbalLegueVM model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.FootbalLegueId))
                {
                    var res = await LequeService.GetBasketbalLegueFixture(int.Parse(model.FootbalLegueId));
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = new List<Models.Legue.Varzesh3LegueFixture> { res },
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.OK,
                        ServiceResultMassage = Util.Common.ServiceResultMessage.OKMessage.ToString()
                    };
                }
                else
                    return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                    {
                        Data = null,
                        ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                        ServiceResultMassage = ""
                    };

            }
            catch (Exception ex)
            {
                return new ServiceResult<Models.Legue.Varzesh3LegueFixture>
                {
                    Data = null,
                    ServiceResultStatus = (int)Rdio.Util.Common.ServiceResultStatus.Error,
                    ServiceResultMassage = ex.GetBaseException().Message
                };
            }
        }
    }
}
