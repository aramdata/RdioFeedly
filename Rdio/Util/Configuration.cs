using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rdio.Util
{
    public class Configuration
    {
        public static string GetPhysicalPath(string path) {
            return System.Web.Hosting.HostingEnvironment.MapPath(path);
        }

        public enum ContentType
        {
            [Description("خبر")]
            News = 0,
            [Description("فروشگاه")]
            Shopping = 1
        }
    }
}