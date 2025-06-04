using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OrderTask.Models;


namespace OrderTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
            // Register DbContext
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //// Add session services
            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromDays(1);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

            //builder.Services.AddIdentity<User, IdentityRole>()
            //                .AddEntityFrameworkStores<Context>();

            builder.Services.AddIdentity<User, IdentityRole>()
                              .AddEntityFrameworkStores<Context>()
                              .AddDefaultTokenProviders();
           
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1); 
                options.SlidingExpiration = true; // Optional: refresh expiration on each request
            });

            // Configure Identity options
            builder.Services.Configure<IdentityOptions>(optiion =>
            optiion.User.RequireUniqueEmail = true);

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

            app.UseRouting();
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}