using System.Collections.Generic;
using TabloidMVC.Controllers;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void UpdatePost(Post post);
        void DeletePost(int postId);

        List<Post> GetAllCurrentUserPosts(int userProfileId);

        void AddTagToPost(int tag, int post);
        void RemoveTagFromPost(int tag, int post);
    }
}