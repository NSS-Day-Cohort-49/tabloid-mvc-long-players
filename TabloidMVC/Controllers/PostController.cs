using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            List<Tag> tags = _tagRepository.GetTagsbyPostId(post.Id);
            List<Tag> allTags = _tagRepository.GetAllTags();
            PostDetailViewModel vm = new PostDetailViewModel()
            {
                Post = post,
                Tags = tags,
                AllTags = allTags
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public IActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public IActionResult UserPosts()
        {
            int userProfileId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllCurrentUserPosts(userProfileId);

            return View(posts);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult Edit(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }
        public ActionResult RemoveTagFromPost(int post, int tag)
        {

            _postRepository.RemoveTagFromPost(tag, post);
            string postdirect = Convert.ToString(post);

            return RedirectToAction("Details", new { id = post });

        }
        public ActionResult AddTagToPost(int post, int tag)
        {

            _postRepository.AddTagToPost(tag, post);

            return RedirectToAction("Details", new { id = post });
        }
    }
}
