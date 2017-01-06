using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rdio.ViewModel.UserBlog
{
    public class GetBlockNewsVM : BaseApiVM
    {
        public string CategoryId { get; set; }
        public string BlockCode { get; set; }
        public int Count { get; set; }

    }
}