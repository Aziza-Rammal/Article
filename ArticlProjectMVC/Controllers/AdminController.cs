using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Linq;
using System.Security.Claims;

namespace ArticlProjectMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IBaseReop<AuthorPost> _repo;

        public AdminController(IBaseReop<AuthorPost> repo)
        {
            _repo = repo;
        }

        public int AllPost { get; set; }
        public int PostLastMouth { get; set; }
        public int PostThisYear { get; set; }
        public IActionResult Index()
        {
            var datem = DateTime.Now.AddMonths(-1);
            var datey = DateTime.Now.AddYears(-1);
            //var dateD = DateTime.Now.AddDays(-1);
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.AllPost = _repo.GetDataByUser(userid).Count;
            ViewBag.PostLastMouth = _repo.GetDataByUser(userid).Where(x => x.AddedDate >= datem).Count();
            ViewBag.PostThisYear = _repo.GetDataByUser(userid).Where(x => x.AddedDate >= datey).Count();
            ViewBag.FullName = "aziza";
            return View();
        }
    }
}
