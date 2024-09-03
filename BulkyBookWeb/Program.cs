using Bulky.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BulkyBook.Utilities;
using Stripe;
using BulkyBook.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
// to populate the secretkey and publishablekey in stripesettings without Iconfiguration

builder.Services
    .AddIdentity<IdentityUser,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// its work is to add identity user and role to dbcontext and also add token provider for password reset and email confirmation etc 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});// this is used to configure the url of application

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "816452900481629";
    options.AppSecret = "secret key";
});// this is used to configure the facebook login

builder.Services.AddDistributedMemoryCache();// this is used to store the session in memory cache 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});// this is used to configure the session
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// in transient service, jevdhya ves service call kelya jail tevdha ves new object create karto
// in scoped service, pratek service la ekdach implemantation delya jail karan he request var depend karto
// in singleton service, jevdhya ves service call kelya jail tevdha ves same object create karto

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey= builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();
// the first approach (ToString()) retrieves the entire section and its sub-sections as a string,
// while the second approach (Get<string>())
// specifically retrieves the value associated with the "Stripe:SecretKey" key as a string,
// and it's a more appropriate way to retrieve the value itself if that's what you need
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();
SeedDatabase();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        /*var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();*/
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<IDbInitializer>();

        

        context.Initialize();
    }
}
