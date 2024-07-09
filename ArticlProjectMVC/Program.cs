using ArticlProjectMVC.Code;
using ArticlProjectMVC.Core.IReposetory;
using ArticlProjectMVC.Data;
using ArticlProjectMVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ArticlProjectMVC.EF.Reposetory;
using ArticlProjectMVC.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<AppDbContext>(option =>
//            option.UseSqlServer("Server=Aziza\\SQL5075.site4now.net;Database=db_aa415a_articlprojectmvc;User Id=db_aa415a_articlprojectmvc_admin;Password=Zz1234~~"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<AppDbContext>(option =>
            option.UseSqlServer("Server=Aziza\\SQLEXPRESS;Database=articlprojectmvc;Trusted_Connection=true"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("User", p => p.RequireClaim("User", "User"));
    op.AddPolicy("Admin", p => p.RequireClaim("Admin", "Admin"));
}
);
builder.Services.AddScoped(typeof(IBaseReop<>), typeof(BaseRepo<>));
    
//builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
