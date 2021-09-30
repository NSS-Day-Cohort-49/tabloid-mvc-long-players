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
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public IActionResult Index(int id)
        {
            CommentViewModel vm = new CommentViewModel();
            vm.CommentList = _commentRepository.GetCommentByPostId(id);
            vm.PostId = id;
            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var comment = _commentRepository.GetCommentByPostId(id);
            if (comment == null)
            {
                int userId = GetCurrentUserProfileId();
                comment = _commentRepository.GetUserCommentById(id, userId);
                if (comment == null)
                {
                    return NotFound();
                }
            }
            return View(comment);
        }

        public IActionResult Create(int id)
        {
            var vm = new CommentCreateViewModel
            {
                Comment = new Comment()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, CommentCreateViewModel commentCreate)
        {
            {
                try
                {
                    commentCreate.Comment.UserProfileId = GetCurrentUserProfileId();
                    commentCreate.Comment.PostId = id;
                    _commentRepository.Add(commentCreate.Comment);
                    return RedirectToAction("Index", new { id });
                }
                catch (Exception ex)
                {
                    var post = _postRepository.GetPublishedPostById(id);
                    commentCreate.Post = post;
                    return View(commentCreate);
                }
            }
        }


        public IActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Comment comment)
        {


            Comment commente = _commentRepository.GetCommentById(id);

            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction("Index", new { id = commente.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        public IActionResult UserComments()
        {
            int userProfileId = GetCurrentUserProfileId();
            var comments = _commentRepository.GetAllCurrentUserComments(userProfileId);

            return View(comments);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult Edit(int id)
        {
            int userProfileId = GetCurrentUserProfileId();
            List<Comment> comment = _commentRepository.GetUserCommentById(id, userProfileId);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }
    }
}
