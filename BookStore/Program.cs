using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookStore.Data;
using Microsoft.Extensions.Hosting;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
//using BookStore.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("BookStoreContext")
                    ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.")
                ));


            builder.Services.AddDefaultIdentity<DefaultUser>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<BookStoreContext>();

            //builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //builder.Services.AddScoped<Cart>(sp => Cart.GetCart(sp));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    UserRoleInitializer.InitializeAync(services).Wait();
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while attempting to seed the database");
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Store}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.Run();
        }
    }
}
