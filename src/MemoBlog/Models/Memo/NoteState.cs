using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.Memo
{
    public enum NoteState
    {
        uncompleted = 0,
        Processing = 1,
        completed = 2,
        abandoned = 3
    }
}
