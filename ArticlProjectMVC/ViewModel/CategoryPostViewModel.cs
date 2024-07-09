using ArticlProjectMVC.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticlProjectMVC.ViewModel
{
    [NotMapped]
    public class CategoryPostViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<AuthorPost> Posts { get; set; }
    }
}
