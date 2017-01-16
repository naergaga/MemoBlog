using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MemoBlog.Models;
using MemoBlog.Models.Memo;
using MemoBlog.Models.emoji;

namespace MemoBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,AppRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTags>().HasKey(t => new { t.TagId, t.PostId });
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Emoticon> Emoticons { get; set; }
    }
}
