using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.Models;
using ArticlProjectMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace ArticlProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseReop<Category> _repoCategory;
        private readonly IBaseReop<AuthorPost> _repoPost;
        public readonly int NoOfItem;
        public IEnumerable<AuthorPost> posts;

        public HomeController(ILogger<HomeController> logger,
            IBaseReop<Category> repoCategory,
            IBaseReop<AuthorPost> repoPost)
        {
            _logger = logger;
            _repoCategory = repoCategory;
            _repoPost = repoPost;
            NoOfItem = 6;
        }

        public IActionResult Index(string LoadState,string CatName,string search,int id)
        {
            var categories = _repoCategory.GetAllData();
            if (LoadState == null || LoadState == "All")
            {
                posts = _repoPost.GetAllData().Take(NoOfItem);
            }
            else if (LoadState == "ByCategory")
            {
                posts = CategoryDataByCategoryName(CatName);
            }
            else if (LoadState=="Search")
            {
                posts = SearchData(search);
            }
            else if (LoadState == "Next")
            {
                posts = GetNextData(id);
            }
            else if (LoadState == "Prev")
            {
                posts = GetNextData(id-NoOfItem);
            }

            var model = new CategoryPostViewModel
            {
                Categories = categories,
                Posts = posts
            };

            return View(model);
        }

        public IActionResult Articl(int ArtId)
        {
            var articl = _repoPost.GetAllData().Where(p => p.Id == ArtId).First();
            return View(articl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private IEnumerable<AuthorPost> CategoryDataByCategoryName(string categoryName)
        {
          var posts = _repoPost.GetAllData().Where(c => c.PostCategory == categoryName).Take(NoOfItem);
            return posts;
        }
        public IEnumerable<AuthorPost> SearchData(string SearchItem)
        {
            var posts = _repoPost.Search(p=>p.PostTitle.Contains(SearchItem)).Take(NoOfItem);
            return posts;
        }
        private IEnumerable<AuthorPost> GetNextData(int id)
        {
            var data= _repoPost.GetAllData().Where(x => x.Id > id).Take(NoOfItem).ToList();
            return data;
        }
    }
   
}