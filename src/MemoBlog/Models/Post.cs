using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool IsPublic { get; set; }

        public ApplicationUser User { get; set; }
    }
}
