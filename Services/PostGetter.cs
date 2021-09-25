﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrueStoryMVC.Models;
using TrueStoryMVC.Models.ViewModels;

namespace TrueStoryMVC.Services
{
    public interface IPostGetter
    {
        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock);
    }

    public class HotPostGetter : IPostGetter
    {
        int N = 20;
        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock)
        {
            return db.Posts.Where(t => t.PostTime.Day + 1 >= DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year).OrderByDescending(p => p.comments.Count).Skip(postBlock.Number * N).Take(N);
        }
    }

    public class BestPostGetter : IPostGetter
    {
        int N = 20;
        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock)
        {
            return db.Posts.Where(t => t.PostTime.Day + 1 > DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year).OrderByDescending(p => p.Rating).Skip(postBlock.Number * N).Take(N);
        }
    }

    public class NewPostGetter : IPostGetter
    {
        int N = 20;
        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock)
        {
            return db.Posts.Where(t => t.PostTime.Day + 1 > DateTime.UtcNow.Day && t.PostTime.Month == DateTime.UtcNow.Month && t.PostTime.Year == DateTime.UtcNow.Year).OrderByDescending(p => p.PostTime).Skip(postBlock.Number * N).Take(N);
        }
    }

    public class TagsPostGetter : IPostGetter
    {
        int N = 20;
        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock)
        {
            IQueryable<Post> posts = null;   

            if (!string.IsNullOrEmpty(postBlock.Argument))
            {
                string[] tagArray = postBlock.Argument.Split(new char[] { ' ', ',', ';' });
                foreach (string s in tagArray)
                    posts = db.Posts.Where(p => p.Tags.Contains(s)).Skip(postBlock.Number * N).Take(N); 
            }
            return posts;
        }
    }

    public class UserPostGetter : IPostGetter
    {
        int N = 20;

        public IQueryable<Post> GetPosts(ApplicationContext db, PostBlockInfo postBlock)
        {
            return db.Posts.Where(p=>p.Author == postBlock.Argument).OrderByDescending(p => p.Rating).Skip(postBlock.Number * N).Take(N);
        }
    }
}
