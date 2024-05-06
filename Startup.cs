using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.ui.Identity;

namespace shopapp.ui
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlite("Data Source=shopDb"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller = "Shop", action = "List" }
                );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                );
                               
                endpoints.MapControllerRoute(
                   name: "adminproductcreate",
                   pattern: "admin/products/create",
                   defaults: new { controller = "Admin", action = "ProductCreate" }
               );

                endpoints.MapControllerRoute(
                   name: "admincategories",
                   pattern: "admin/categories",
                   defaults: new { controller = "Admin", action = "CategoryList" }
               );

                endpoints.MapControllerRoute(
                   name: "admincategorycreate",
                   pattern: "admin/categories/create",
                   defaults: new { controller = "Admin", action = "CategoryCreate" }
               );

                endpoints.MapControllerRoute(
                   name: "adminproductedit",
                   pattern: "admin/products/{id?}",
                   defaults: new { controller = "Admin", action = "ProductEdit" }
               );

                endpoints.MapControllerRoute(
                  name: "admincategoryedit",
                  pattern: "admin/categories/{id?}",
                  defaults: new { controller = "Admin", action = "CategoryEdit" }
              );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Shop", action = "search" }
                );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "Shop", action = "Details" }
                );


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
