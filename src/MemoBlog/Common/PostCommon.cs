using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemoBlog.Common
{
    public class PostCommon
    {

        public static string ParseImageSrc(string host, string content)
        {
            return content.Replace("<img src=\"../upload",string.Format("<img src=\"{0}/upload",host));
        }
    }
}
