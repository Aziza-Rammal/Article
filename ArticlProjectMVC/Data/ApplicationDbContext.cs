using ArticlProjectMVC.Core.Models;
using ArticlProjectMVC.Data.Config;
using ArticlProjectMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ArticlProjectMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            new CategoryConfigration().Configure(builder.Entity<Category>());
            base.OnModelCreating(builder);
        }
    }
}