using ArticlProjectMVC.Code;
using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.ViewModel.PostViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Claims;

namespace ArticlProjectMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IBaseReop<AuthorPost> _repo;
        private readonly IWebHostEnvironment _webHost;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBaseReop<Author> _repoAuthor;
        private readonly IBaseReop<Category> _repoCategory;
        private readonly FileHelper _fileHelper;
        private int pageItem;
        private Task<AuthorizationResult> result;
        private string UserId;

        public PostController(
            IBaseReop<AuthorPost> repo,
            IWebHostEnvironment webHost,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            IBaseReop<Author> repoAuthor,
            IBaseReop<Category> repoCategory)
        {
            _repo = repo;
            _webHost = webHost;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _repoAuthor = repoAuthor;
            _repoCategory = repoCategory;
            _fileHelper = new Code.FileHelper(_webHost);
            pageItem = 5;
           
        }
        // GET: AuthorPostController
        public ActionResult Index(int? id)
        {
            SetUser();
            // user case "Admin" => All data
            if (result.Result.Succeeded)
            {
                if (id == 0 || id == null)
                {
                    return View(_repo.GetAllData().Take(pageItem));
                }
                else
                {
                    var data = _repo.GetAllData().Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            }
            else

            {
                if (id == 0 || id == null)
                {
                    return View(_repo.GetDataByUser(UserId).Take(pageItem));
                }
                else
                {
                    var data = _repo.GetDataByUser(UserId).Where(x => x.Id > id).Take(pageItem);
                    return View(data);
                }
            }
            
            // user case "User" => User id
        }
        public ActionResult Search(string SearchItem)
        {
            SetUser();
            if (result.Result.Succeeded)
            {
                if (SearchItem == null)
                {
                    return View("Index", _repo.GetAllData());
                }
                else
                {
                    return View("Index", _repo.Search(c =>
                c.PostCategory.Contains(SearchItem) ||
                c.PostDescription.Contains(SearchItem) ||
               
                c.PostTitle.Contains(SearchItem)).ToList());
                }
            }
            else
            {
                if (SearchItem == null)
                {
                    return View("Index", _repo.GetDataByUser(UserId));
                }
                else
                {
                    return View("Index", _repo.Search(
                   c=>c.PostTitle.Contains(SearchItem)).Where(x => x.UserId == UserId).ToList());
                }
            }
        }

        // GET: AuthorPostController/Details/5
        public ActionResult Details(int id)
        {
            SetUser();
            return View(_repo.Find(id));
        }

        // GET: AuthorPostController/Create
        public ActionResult Create()
        {
            SetUser();
            return View();
        }

        // POST: AuthorPostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostView collection)
        {
            SetUser();
            try
            {
                var post = new AuthorPost
                {
                    AddedDate = DateTime.Now,
                    Author = collection.Author,
                    AuthorId = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    Category = collection.Category,
                    CategoryId = _repoCategory.GetAllData().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    FullName = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.UserEmail).First(),
                    PostImageUrl = _fileHelper.UploadFile(collection.PostImageUrl, "Images")
                };
                _repo.Add(post);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorPostController/Edit/5
        public ActionResult Edit(int id)
        {

          
            var dataAuthor = _repo.Find(id);
            //var _imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            //string imagePath = Path.Combine(_imagesFolderPath, dataAuthor.PostImageUrl);
            //var stream = new FileStream(imagePath, FileMode.Open);
            ViewBag.PostTitle = dataAuthor.PostImageUrl;
            var post = new PostView
            {
                AddedDate = dataAuthor.AddedDate,
                Author = dataAuthor.Author,
                AuthorId = dataAuthor.AuthorId,
                Category = dataAuthor.Category,
                CategoryId = dataAuthor.CategoryId,
                FullName = dataAuthor.FullName,
                PostCategory = dataAuthor.PostCategory,
                PostDescription = dataAuthor.PostDescription,
                PostTitle = dataAuthor.PostTitle,
                UserId = dataAuthor.UserId,
                UserName = dataAuthor.UserName,
                //Id = dataAuthor.Id,
                //PostImageUrl= new FormFile(stream, 0, stream.Length, null, Path.GetFileName(dataAuthor.PostImageUrl))
        };
                return View(post);
    }

    // POST: AuthorPostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostView collection)
        {
            try
            {
                SetUser();

                var Post = new AuthorPost
                {
                    AddedDate = DateTime.Now,
                    Author = collection.Author,
                    AuthorId = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.Id).First(),
                    Category = collection.Category,
                    CategoryId = _repoCategory.GetAllData().Where(x => x.Name == collection.PostCategory).Select(x => x.Id).First(),
                    FullName = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.FullName).First(),
                    PostCategory = collection.PostCategory,
                    PostDescription = collection.PostDescription,
                    PostTitle = collection.PostTitle,
                    UserId = UserId,
                    UserName = _repoAuthor.GetAllData().Where(x => x.UserId == UserId).Select(x => x.UserEmail).First(),
                    PostImageUrl = _fileHelper.UploadFile(collection.PostImageUrl, "Images"),
                    Id = id
                };
                _repo.Edit(Post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthorPostController/Delete/5
        public ActionResult Delete(int id)
        {
          
            return View(_repo.Find(id));
        }

        // POST: AuthorPostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AuthorPost collection)
        {
            try
            {
                _repo.Delete(id);
                string filePath = "~/Images/" + collection.PostImageUrl;
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
        private void SetUser()
        {
            result = _authorizationService.AuthorizeAsync(User, "Admin");
            UserId = _userManager.GetUserId(User);
        }
    }
}
