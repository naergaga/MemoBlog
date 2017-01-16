using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models
{
    public class PostTags
    {
        //[Key,Column(Order =0)]
        [ForeignKey("Post")]
        public int PostId { get; set; }
        //[Key,Column(Order =1)]
        [ForeignKey("Tag")]
        public int TagId { get; set; }

        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}
