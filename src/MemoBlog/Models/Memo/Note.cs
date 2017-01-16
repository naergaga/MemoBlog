using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.Memo
{
    public class Note
    {
        public int Id { get; set; }
        [Display(Name ="创建日期")]
        public DateTime CreateTime { get; set; }
        [Display(Name ="内容")]
        public string Content { get; set; }
        [Display(Name ="状态")]
        public NoteState State { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }

}
