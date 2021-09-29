using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController (ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);

                return RedirectToAction("Index");
            }

            catch
            {
                return View(tag);
            }
        }

        public ActionResult Edit(int id)
        {

            Tag tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);

        }

        [HttpPost]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.Update(tag);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }

        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        [HttpPost]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }
    }

}