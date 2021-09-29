using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController :Controller
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
        public IActionResult Edit(int id)
        {

            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);

        }

        [HttpPost]
        public ActionResult Edit(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.Update(userProfile);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }
    }
}
