using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public int Pid { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }

        public Post Post { get; set; }
        public ApplicationUser User { get; set; }

    }

}
