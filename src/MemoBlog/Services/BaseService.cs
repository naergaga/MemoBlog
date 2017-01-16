using MemoBlog.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Services
{
    public abstract class BaseService<T> where T:class
    {
        protected ApplicationDbContext _db;
        protected DbSet<T> _dbSet;

        public BaseService(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public bool Add(T t)
        {
            _dbSet.Add(t);
            return _db.SaveChanges()==1;
        }

        public bool Edit(T t)
        {
            _dbSet.Update(t);
            return _db.SaveChanges() == 1;
        }

        public bool Delete(T t)
        {
            _dbSet.Remove(t);
            return _db.SaveChanges() == 1;
        }

        public abstract T Get(object pkey);
        public abstract List<T> GetList();

    }
}
