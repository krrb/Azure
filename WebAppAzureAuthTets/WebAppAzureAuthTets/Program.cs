using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppAzureAuthTets.Data;

namespace WebAppAzureAuthTets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("WebAppAzureAuthTetsContextConnection") ?? throw new InvalidOperationException("Connection string 'WebAppAzureAuthTetsContextConnection' not found.");

            builder.Services.AddDbContext<WebAppAzureAuthTetsContext>(options => options.UseSqlite(connectionString)); ;

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<WebAppAzureAuthTetsContext>(); ;

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string[] scopes = new string[] { "https://storage.azure.com/user_impersonation" };

            builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
                            .EnableTokenAcquisitionToCallDownstreamApi(scopes)
                            .AddInMemoryTokenCaches();
            builder.Services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}