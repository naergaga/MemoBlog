using MemoBlog.Common;
using MemoBlog.Data;
using MemoBlog.Models;
using MemoBlog.Models.Util;
using MemoBlog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MemoBlog.Services
{
    public class PostService : BaseService<Post>
    {
        private TagService tagService;
        private PostTagService postTagService;

        public PostService(ApplicationDbContext db, TagService tagService, PostTagService postTagService) : base(db)
        {
            this.tagService = tagService;
            this.postTagService = postTagService;
        }

        public override Post Get(object pkey)
        {
            return _db.Posts.SingleOrDefault(t => t.Id == (int)pkey);
        }

        public override List<Post> GetList()
        {
            return _db.Posts.ToList();
        }

        public bool IsPostByUserId(int id, string userId)
        {
            return _db.Posts.Where(t => t.UserId == userId && t.Id == id).Count() == 1;
        }

        public void Add(Post post, string tags)
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                this.Add(post);
                return;
            }
            var tagArr = TagCommon.GetTags(tags);
            //写入Post
            if (this.Add(post))
            {
                foreach (var item in tagArr)
                {
                    //获取tag
                    Tag tag = tagService.GetByName(item);
                    if (tag == null)
                    {
                        tag = new Tag { Name = item, UserId = post.UserId };
                        tagService.Add(tag);
                    }
                    //添加PostTag
                    postTagService.Add(new PostTags
                    {
                        PostId = post.Id,
                        TagId = tag.Id
                    });
                }
            }
        }

        public List<PostView> GetPagePostViewListInPublic(PageOption po, string author)
        {
            int skipNum = (po.CurrentPage - 1) * po.PageSize;

            var postList = (from p in _db.Posts
                           join u in _db.Users on p.UserId equals u.Id
                           where u.UserName == author && p.IsPublic==true
                           select p).OrderBy(p => p.CreateTime).Skip(skipNum).Take(po.PageSize);
            List<PostView> list = new List<PostView>();
            foreach (var item in postList)
            {
                PostView pv = new PostView(item);
                pv.Tags = from t in _db.Tags
                          join pt in _db.PostTags on t.Id equals pt.TagId
                          join p in _db.Posts on pt.PostId equals p.Id
                          where p.Id == item.Id
                          select t;
                list.Add(pv);
            }
            return list;
        }

        public int CountByPublic(string author)
        {
            return (from p in _db.Posts
                    join u in _db.Users on p.UserId equals u.Id
                    where u.UserName==author && p.IsPublic == true
                    select p).Count();
        }

        public List<PostView> GetPagePostViewListInPublic(PageOption po)
        {
            int skipNum = (po.CurrentPage - 1) * po.PageSize;

            var postList = _db.Posts.Where(t=>t.IsPublic==true).Include(t=>t.User).OrderBy(p => p.CreateTime).Skip(skipNum).Take(po.PageSize);
            List<PostView> list = new List<PostView>();
            foreach (var item in postList)
            {
                PostView pv = new PostView(item);
                pv.Tags = from t in _db.Tags
                          join pt in _db.PostTags on t.Id equals pt.TagId
                          join p in _db.Posts on pt.PostId equals p.Id
                          where p.Id == item.Id
                          select t;
                list.Add(pv);
            }
            return list;
        }

        public int CountByPublic()
        {
            return _db.Posts.Where(t => t.IsPublic == true).Count();
        }

        public int CountByUser(string userName)
        {
            return (from p in _db.Posts
                    join u in _db.Users on p.UserId equals u.Id
                    where u.UserName == userName
                    select p).Count();
        }

        public int CountByTagAndUser(string userName, int tagId)
        {
            return (from p in _db.Posts
                    join u in _db.Users on p.UserId equals u.Id
                    join pt in _db.PostTags on p.Id equals pt.PostId
                    join t in _db.Tags on pt.TagId equals t.Id
                    where u.UserName == userName
                    where t.Id == tagId
                    select p).Count();
        }

        public List<PostView> GetPagePostViewListByTag(string userName, int tagId, PageOption po)
        {
            int skipNum = (po.CurrentPage - 1) * po.PageSize;

            var postList = (from p in _db.Posts
                           join u in _db.Users on p.UserId equals u.Id
                           join pt in _db.PostTags on p.Id equals pt.PostId
                           join t in _db.Tags on pt.TagId equals t.Id
                           where u.UserName == userName
                           where t.Id == tagId
                           select p).OrderBy(p => p.CreateTime).Skip(skipNum).Take(po.PageSize);
            List<PostView> list = new List<PostView>();
            foreach (var item in postList)
            {
                PostView pv = new PostView(item);
                pv.Tags = from t in _db.Tags
                          join pt in _db.PostTags on t.Id equals pt.TagId
                          join p in _db.Posts on pt.PostId equals p.Id
                          where p.Id == item.Id
                          select t;
                list.Add(pv);
            }
            return list;
        }

        public PostView GetPostView(int id)
        {
            var post = _db.Posts.SingleOrDefault(t => t.Id == id);
            if (post == null) { return null; }
            PostView pv = new PostView(post);
            pv.Tags = from t in _db.Tags
                      join pt in _db.PostTags on t.Id equals pt.TagId
                      join p in _db.Posts on pt.PostId equals p.Id
                      where p.Id == post.Id
                      select t;
            return pv;
        }

        public List<PostView> GetPagePostViewList(string userName,PageOption po)
        {
            int skipNum = (po.CurrentPage - 1) * po.PageSize;
            var postList = (from p in _db.Posts
                           join u in _db.Users on p.UserId equals u.Id
                           where u.UserName == userName
                           select p).OrderBy(p => p.CreateTime).Skip(skipNum).Take(po.PageSize);
            List<PostView> list = new List<PostView>();
            foreach (var item in postList)
            {
                PostView pv = new PostView(item);
                pv.Tags = from t in _db.Tags
                          join pt in _db.PostTags on t.Id equals pt.TagId
                          join p in _db.Posts on pt.PostId equals p.Id
                          where p.Id == item.Id
                          select t;
                list.Add(pv);
            }
            return list;
        }

        public void Edit(Post post, string tags)
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                this.Edit(post);
                return;
            }
            var tagArr = TagCommon.GetTags(tags);
            //写入Post
            if (this.Edit(post))
            {

                var taglist = (from t in _db.Tags
                               join pt in _db.PostTags on t.Id equals pt.TagId
                               join p in _db.Posts on pt.PostId equals p.Id
                               where p.Id == post.Id
                               select t).ToList();
                //这个tag不在了
                for (int i = taglist.Count() - 1; i >= 0; i--)
                {
                    var item = taglist.ElementAt(i);
                    if (!tagArr.Contains(item.Name))
                    {
                        tagService.Delete(item);
                    }
                }

                foreach (var item in tagArr)
                {
                    //这个tag没改变
                    if (TagCommon.Contains(taglist, item))
                    {
                        continue;
                    }
                    //获取tag
                    Tag tag = tagService.GetByName(item);
                    if (tag == null)
                    {
                        tag = new Tag { Name = item, UserId = post.UserId };
                        tagService.Add(tag);
                    }
                    //添加PostTag
                    postTagService.Add(new PostTags
                    {
                        PostId = post.Id,
                        TagId = tag.Id
                    });

                }
            }
        }
    }
}
