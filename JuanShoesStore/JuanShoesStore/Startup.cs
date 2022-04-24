using JuanShoesStore.Models;
using JuanShoesStore.Services;
using JuanShoesStore.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanShoesStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _enviroment = environment; 
        }


        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _enviroment;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(opt =>
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<JuanContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<AppUser, IdentityRole>(c =>
            {
                c.Password.RequireNonAlphanumeric = false;
                c.User.RequireUniqueEmail = true;
                c.Password.RequiredLength = 8;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<JuanContext>();

            services.AddScoped<LayoutService>();

            services.AddHttpContextAccessor();

            Constants.UploadImagesFolderPath = _enviroment.WebRootPath + "/uploads";
            Constants.TemplatesFolderPath = _enviroment.WebRootPath + "/templates";
            Constants.AccountRegisterLink = "https://localhost:44352/account/register  ";
            Constants.EmailAddress = Configuration["Gmail:MailAddress"];
            Constants.Password = Configuration["Gmail:Password"];
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
