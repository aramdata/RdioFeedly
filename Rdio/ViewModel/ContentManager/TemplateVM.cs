﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rdio.ViewModel.ContentManager
{
    public class TemplateVM
    {
        public string _id { get; set; }
        public string rssid { get; set; }
        public string sampleurl { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public List<Tuple<string,string>> structures { get; set; }
    }
}