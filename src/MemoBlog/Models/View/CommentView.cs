using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoBlog.Models.View
{
    public class CommentView
    {

        public CommentView(Comment comment)
        {
            this.Id = comment.Id;
            this.Content = comment.Content;
            this.PostId = comment.PostId;
            this.UserId = comment.UserId;
            if (comment.User != null)
            {
                this.User = new UserView
                {
                    Id =comment.User.Id,
                    UserName = comment.User.UserName
            };
            }

            this.Pid = comment.Pid;
            this.CreateTime = comment.CreateTime;
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int Pid { get; set; }
        public DateTime CreateTime { get; set; }
        public UserView User { get; set; }
        public List<CommentView> SubList { get; set; }
    }

    public class UserView
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
