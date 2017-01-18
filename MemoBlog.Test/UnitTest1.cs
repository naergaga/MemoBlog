using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using MemoBlog.Models;
using MemoBlog.Data;
using Microsoft.EntityFrameworkCore;
using MemoBlog.Models.Util;
using System.Linq;

namespace MemoBlog.Test
{
    [TestClass]
    public class UnitTest1
    {

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    string path = @"F:\work\code\csharp\2016\12\MemoBlog\src\MemoBlog\wwwroot\data\txt\ci.txt";
        //    string content = System.IO.File.ReadAllText(path);
        //    Regex reg = new Regex(@"\s+全宋词\s+");
        //    var collection = reg.Matches(content);
        //    Match item1 = null;
        //    StringBuilder sb = new StringBuilder();
        //    List<Song> list = new List<Song>();

        //    //每个作者
        //    foreach (Match item in collection)
        //    {
        //        if (item1 == null)
        //        {
        //            item1 = item;
        //            continue;
        //        }

        //        var start = item1.Index + item1.Length;
        //        var length = item.Index - start;

        //        item1 = item;
        //        string str2 = content.Substring(start, length);
        //        var arr = str2.Split("\r\n".ToCharArray());
        //        Song song = null;
        //        string author = arr[0].Trim();

        //        //每一行
        //        for (int i = 1; i < arr.Length; i++)
        //        {
        //            if (i==arr.Length-1)
        //            {
        //                ;
        //            }

        //            var line = arr[i].Trim();
        //            if (line.EndsWith("。"))
        //            {
        //                sb.Append(line);
        //            }
        //            else
        //            {
        //                if (string.IsNullOrWhiteSpace(line))
        //                {
        //                    continue;
        //                }
        //                if (song != null)
        //                {
        //                    song.Content = sb.ToString();
        //                    sb.Clear();
        //                    list.Add(song);
        //                }
        //                song = new Song
        //                {
        //                    Title = line,
        //                    Author = author
        //                };
        //            }
        //        }

        //        if (song!=null)
        //        {
        //            song.Content = sb.ToString();
        //            sb.Clear();
        //            list.Add(song);
        //        }
        //    }

        //    var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        //    builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-MemoBlog-9aaa3c57-ecb9-48e4-b171-09a9dba05dc6;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    ApplicationDbContext context = new ApplicationDbContext(builder.Options);

        //    context.Songs.AddRange(list);
        //    context.SaveChanges();

        //}

        [TestMethod]
        public void Test2()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-MemoBlog-9aaa3c57-ecb9-48e4-b171-09a9dba05dc6;Trusted_Connection=True;MultipleActiveResultSets=true");
            ApplicationDbContext _context = new ApplicationDbContext(builder.Options);

            PageOption po = new PageOption
            {
                ActionName = "CategoryIndex",
                ControllerName = "Blog",
                CurrentPage = 1
            };
            var cateItem = _context.Category.FirstOrDefault(t => t.Name == "编程");
            //分页Count
            po.AddPageCount(_context.Posts.Count(t => t.CategoryId == cateItem.Id && t.IsPublic == true));

        }
    }
}
