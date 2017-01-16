using MemoBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoBlog.Data;

namespace MemoBlog.Services
{
    public class TagService : BaseService<Tag>
    {
        public TagService(ApplicationDbContext db) : base(db)
        {
        }

        public override Tag Get(object pkey)
        {
            return _db.Tags.SingleOrDefault(t => t.Id == (int)pkey);
        }

        public override List<Tag> GetList()
        {
            return _db.Tags.ToList();
        }

        public Tag GetByName(string name)
        {
            return _db.Tags.SingleOrDefault(t => t.Name == name);
        }

        public IEnumerable<Tag> getListByUser(string userName)
        {
            return from t in _db.Tags
                   join u in _db.Users on t.UserId equals u.Id
                   where u.UserName==userName
                   select t;
        }
    }
}
