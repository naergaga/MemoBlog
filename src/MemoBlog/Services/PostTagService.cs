using MemoBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoBlog.Data;

namespace MemoBlog.Services
{
    public class PostTagService : BaseService<PostTags>
    {
        public PostTagService(ApplicationDbContext db) : base(db)
        {
        }

        public override PostTags Get(object pkey)
        {
            throw new NotImplementedException();
        }

        public override List<PostTags> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
