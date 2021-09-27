using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Repositories;


namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPostRepository _postRepository;

        public CategoryController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Details(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);


        }
          

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.Add(category);

                return RedirectToAction("Index");   
            }

            catch
            {
                return View(category);
            }
        }

        public ActionResult Edit(int id)
        {

            Category category = _categoryRepository.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }

        [HttpPost]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.Update(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }

        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);

            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }
    }   
    
}