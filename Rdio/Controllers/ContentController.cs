using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Rdio.Controllers
{
    public class ContentController : Controller
    {
        Repository.ContentRepository contentRepository = new Repository.ContentRepository();
        public async Task<ActionResult> item(string id)
        {
            var model = await contentRepository.ContentInfo(id);
            return View(model);
        }
    }
}