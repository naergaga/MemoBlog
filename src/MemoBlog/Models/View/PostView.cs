using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.View
{
    public class PostView:Post
    {

        public PostView(Post item)
        {
            this.Content = item.Content;
            this.CreateTime = item.CreateTime;
            this.Id = item.Id;
            this.IsPublic = item.IsPublic;
            this.Title = item.Title;
            this.UserId = item.UserId;
            this.User = item.User;
            this.CategoryId = item.CategoryId;
        }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
