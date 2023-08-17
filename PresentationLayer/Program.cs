using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules.FluentValidation;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Models;
using PresentationLayer.Services;
using PresentationLayer.Utils.ConfigOptions;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


builder.Services.AddIdentity<AppUser, AppRole>()
    .AddRoleManager<RoleManager<AppRole>>() 
    .AddErrorDescriber<CustomIdentityErrorDescriber>() 
    .AddEntityFrameworkStores<Context>() 
    .AddDefaultTokenProviders(); ;

builder.Services.Configure<GCSConfigOptions>(builder.Configuration);
builder.Services.AddSingleton<ICloudStorageService, CloudStorageService>();

builder.Services.AddDbContext<Context>();
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<INewsDal, EfNewsDal>();
builder.Services.AddScoped<INewsService, NewsManager>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<ISocialMediaDal, EfSocialMediaDal>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();

builder.Services.AddScoped<IImageDal, EfImageDal>();
builder.Services.AddScoped<IImageService, ImageManager>();

builder.Services.AddScoped<ITestDal, EfTestDal>();
builder.Services.AddScoped<ITestService, TestManager>();

builder.Services.AddScoped<ITestAnswerDal, EfTestAnswerDal>();
builder.Services.AddScoped<ITestAnswerService, TestAnswerManager>();

builder.Services.AddScoped<IQuestionDal, EfQuestionDal>();
builder.Services.AddScoped<IQuestionService, QuestionManager>();

builder.Services.AddScoped<ITestCategoryDal, EfTestCategoryDal>();
builder.Services.AddScoped<ITestCategoryService, TestCategoryManager>();

builder.Services.AddScoped<IEatDal, EfEatDal>();
builder.Services.AddScoped<IEatService, EatManager>();

builder.Services.AddScoped<IContactDal, EfContactDal>();
builder.Services.AddScoped<IContactService, ContactManager>();

builder.Services.AddScoped<ICommentDal, EfCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddSession();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login"); // kullanýcý giriþ yapmadan eriþmeye çalýþýyorsa buraya yönlendirilecek
    config.LogoutPath = new PathString("/Admin/Auth/Logout"); // çýkýþ yaptýðýnda buraya.
    config.Cookie = new CookieBuilder
    {
        Name = "OnedioCookie", // cookie'ye isim verdik.
        HttpOnly = true, // sadece http protokolü üzerinden istek alýr.
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest // hem http hem de https üzerinden yapmak için
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(7); // cookie'nin tutulma süresi
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied"); // yetkisiz bir istek olduðunda bu sayfaya yönlendirilecek
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.UseSession();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Default}/{action=Index}/{id?}"
        );
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
        );
    endpoints.MapDefaultControllerRoute();
});

app.UseStatusCodePagesWithReExecute("/ErrorPage/Page404");

app.Run();
