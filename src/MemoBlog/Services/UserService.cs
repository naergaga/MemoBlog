using MemoBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoBlog.Data;

namespace MemoBlog.Services
{
    public class UserService : BaseService<ApplicationUser>
    {
        public UserService(ApplicationDbContext db) : base(db)
        {
        }

        public override ApplicationUser Get(object pkey)
        {
            return _dbSet.SingleOrDefault(t => t.Id == (string)pkey);
        }

        public override List<ApplicationUser> GetList()
        {
            return _dbSet.ToList();
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return _dbSet.SingleOrDefault(t => t.UserName == userName);
        }

        public string GetIdByUserName(string name)
        {
            var item=_dbSet.SingleOrDefault(t => t.UserName == name);
            if (item!=null)
            {
                return item.Id;
            }
            return null;
        }

    }
}
