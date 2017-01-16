using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string Name { get; set; }

        public ApplicationUser User { get; set; }
    }
}
