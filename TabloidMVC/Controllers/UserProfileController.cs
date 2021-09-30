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
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Index()
        {
            var userProfiles = _userProfileRepository.GetAll();
            return View(userProfiles);  
        }

        public IActionResult Details(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        public IActionResult UpdateStatus(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.UpdateStatus(userProfile);
                return RedirectToAction("Index");
            }
            catch 
            {
                return View(userProfile);
            }
        }

        public IActionResult ActivateStatus(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        [HttpPost]
        public IActionResult ActivateStatus(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.ActivateStatus(userProfile);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

        public ActionResult Delete(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            return View(userProfile);
        }

        [HttpPost]
        public ActionResult Delete(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

          [HttpPost]
        public IActionResult UpdateUserType(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.UpdateUserType(userProfile);


                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
