using ArticlProjectMVC.Code;
using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.ViewModel.AuthorViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ArticlProjectMVC.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IBaseReop<Author> _repo;
        private readonly IWebHostEnvironment _webHost;
        private readonly IAuthorizationService _authorizationService;
        private readonly FileHelper _fileHelper;
        

        public AuthorController(IBaseReop<Author> repo,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService)
        {
            _repo = repo;
            _webHost = webHost;
            _authorizationService = authorizationService;
            _fileHelper = new Code.FileHelper(_webHost);
        }
        // GET: AuthorController
        [Authorize("Admin")]
        public ActionResult Index()
        {
            return View(_repo.GetAllData());
        }

        // GET: AuthorController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var author = _repo.Find(id);
            AuthorView authorView = new AuthorView
            {
                Id = author.Id,
                Bio = author.Bio,
                Facbook = author.Facbook,
                FullName = author.FullName,
                Instagram = author.Instagram,
                Twitter = author.Twitter,
                UserId = author.UserId,
                UserEmail = author.UserEmail,
            };
            return View(authorView);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, AuthorView collection)
        {
            try
            {
                var author = new Author
                {
                    Id = collection.Id,
                    Bio = collection.Bio,
                    Facbook = collection.Facbook,
                    FullName = collection.FullName,
                    Instagram = collection.Instagram,
                    Twitter = collection.Twitter,
                    UserId = collection.UserId,
                    UserEmail = collection.UserEmail,
                    ProfileImageUrl = _fileHelper.UploadFile(collection.ProfileImageUrl, "Images")

                };
                _repo.Edit( author);
               var result = _authorizationService.AuthorizeAsync(User, "Admin");
                if(result.Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorController/Delete/5
        [Authorize("Admin")]
        public ActionResult Delete(int id)
        {
            var author = _repo.Find(id);
            //AuthorView authorView = new AuthorView
            //{
            //    Id = author.Id,
            //    Bio = author.Bio,
            //    Facbook = author.Facbook,
            //    FullName = author.FullName,
            //    Instagram = author.Instagram,
            //    Twitter = author.Twitter,
            //    UserId = author.UserId,
            //    UserEmail = author.UserEmail,
            //};
            return View(author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public ActionResult Delete(int id, Author collection)
        {
            try
            {
                _repo.Delete(id);
                string filePath = "~/Images/" + collection.ProfileImageUrl;
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
