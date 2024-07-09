using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ArticlProjectMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IBaseReop<Category> _repo;
        private int pageItem;

        public CategoryController(IBaseReop<Category> repo)
        {
            _repo = repo;
            pageItem = 5;
        }
        // GET: CategoryController
        public ActionResult Index(int? id)
        {
            if(id == null|| id==0)
            {
                return View(_repo.GetAllData().Take(pageItem));
            }
            else
            {
                var data = _repo.GetAllData().Where(x => x.Id > id).Take(pageItem);
                return View(data);
            }
            
        }

        public ActionResult Search(string SearchItem)
        {
            if (SearchItem == null)
            {
                return View("Index", _repo.GetAllData());
            }
            else
            {
                return View("Index", _repo.Search(c=>
                c.Name.Contains(SearchItem)||
                c.Id.ToString().Contains(SearchItem))
                .ToList());
            }
        }
        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            
                _repo.Add(category);
                return RedirectToAction("Index");
            
            //else
            //{
            //    return View(category);
            //}
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category =_repo.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Category category)
        {
            if (ModelState.IsValid)
            {
                _repo.Edit(category);
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _repo.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
           
                _repo.Delete(id);
                return RedirectToAction("Index");
            
            
        }
    }
}
