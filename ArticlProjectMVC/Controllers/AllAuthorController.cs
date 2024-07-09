using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.Models;
using ArticlProjectMVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArticlProjectMVC.Controllers
{
    public class AllAuthorController : Controller
    {
        private readonly IBaseReop<Author> _repoAuthor;
        public readonly int NoOfItem;
        public IEnumerable<Author> ListOfAuthor;

        public AllAuthorController(IBaseReop<Author> repoAuthor)
        {
            
            _repoAuthor = repoAuthor;
            NoOfItem = 6;
        }

        public IActionResult Index(string LoadState, string search, int id)
        {

            if (LoadState == null || LoadState == "All")
            {
                ListOfAuthor = _repoAuthor.GetAllData().Take(NoOfItem);
            }
            else if (LoadState == "Search")
            {
                ListOfAuthor = SearchData(search);
            }
            else if (LoadState == "Next")
            {
                ListOfAuthor = GetNextData(id);
            }
            else if (LoadState == "Prev")
            {
                ListOfAuthor = GetNextData(id - NoOfItem);
            }

            //var model = new CategoryPostViewModel
            //{
            //    Posts = posts
            //};

            return View(ListOfAuthor);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        private IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IEnumerable<Author> SearchData(string SearchItem)
        {
            var posts = _repoAuthor.Search(p => p.FullName.Contains(SearchItem)).Take(NoOfItem);
            return posts;
        }

        private IEnumerable<Author> GetNextData(int id)
        {
            var data = _repoAuthor.GetAllData().Where(x => x.Id > id).Take(NoOfItem).ToList();
            return data;
        }
    }
}

