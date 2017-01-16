using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.Util
{
    public class UploadInfo
    {
        public string State { get; set; }
        public string Url { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
    }
}
