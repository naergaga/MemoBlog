using MemoBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoBlog.Data;
using Microsoft.EntityFrameworkCore;
using MemoBlog.Common;
using MemoBlog.Models.View;

namespace MemoBlog.Services
{
    public class CommentService : BaseService<Comment>
    {
        public CommentService(ApplicationDbContext db) : base(db)
        {
        }

        public override Comment Get(object pkey)
        {
            return _db.Comments.SingleOrDefault(c => c.Id == (int)pkey);
        }

        public override List<Comment> GetList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回递归的评论
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public List<CommentView> GetListByPost(int postId)
        {
            return CommentCommon.ParseList(
                _db.Comments.OrderBy(c=>c.CreateTime).Where(c => c.PostId == postId).Include(c => c.User).ToList());
        }

        public bool Delete(int id, string name)
        {
            var item = _db.Comments.Include(t => t.User).SingleOrDefault(c => c.Id == id);
            if (item.User.UserName != name) return false;
            Delete(item);
            return true;
        }

        private void DeleteWithChildren(Comment cItem)
        {
            var queryList = _db.Comments.Where(c => c.Pid == cItem.Id);
            foreach (var item in queryList)
            {
                DeleteWithChildren(item);
            }
            this.Delete(cItem);
        }

        public List<Comment> GetListByUserName(string name)
        {
            var list = (from c in _db.Comments
                       join u in _db.Users on c.UserId equals u.Id
                       where u.UserName == name
                       select c).ToList();
            return list;
        }
    }
}
