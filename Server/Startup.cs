using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Components;

namespace CheckNote.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddIdentity<User, Role>(config =>
            {
                // development
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped(s => 
                new HttpClient
                {
                    BaseAddress = new Uri(s.GetRequiredService<NavigationManager>().BaseUri)
                }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToPage("/Index");
            });

            // do this when adding user to a role
            if (!roleManager.RoleExistsAsync(Role.Admin).Result)
            {
                roleManager.CreateAsync(new Role { Name = Role.Admin });
            }
        }
    }
}
