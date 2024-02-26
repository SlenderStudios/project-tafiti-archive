using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;

namespace WLQuickApps.ContosoBicycleClub.Business.MetaWeblogApi
{
    public class MsnSpacesMetaWeblogService
    {
        MsnSpacesMetaWeblog blogApi;
        string username;
        string password;

        public MsnSpacesMetaWeblogService()
        {
            this.blogApi = new MsnSpacesMetaWeblog();
            this.username = ConfigurationManager.AppSettings["MetaWeblogUser"];
            this.password = ConfigurationManager.AppSettings["MetaWeblogPwd"];
            this.blogApi.Credentials = new NetworkCredential(username, password);
        }

        public string CreatePost(Post post)
        {
            return blogApi.newPost("MyBlog", this.username, this.password, post, true);
        }

        public void UpdatePost(string postId, Post post)
        {
            blogApi.editPost(postId, username, password, post, true);
        }

        public Post GetPost(string postId)
        {
            return blogApi.getPost(postId, username, password);
        }

        public void DeletePost(string postId)
        {
            blogApi.deletePost(String.Empty, postId, username, password, true);
        }
    }
}
