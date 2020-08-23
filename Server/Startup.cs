using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CheckNote.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

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
            services.AddHttpContextAccessor();

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

            services.AddAuthentication(config =>
            {
                config.RequireAuthenticatedSignIn = false;
            })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false; // development
                    config.SaveToken = true;

                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        RequireAudience = false, ValidateAudience = false, ValidateIssuer = false
                    };
                });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/");

            services.AddControllersWithViews(options =>
            {
                var schemes = new[] { IdentityConstants.ApplicationScheme, JwtBearerDefaults.AuthenticationScheme };

                var policy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(schemes)
                    .RequireAuthenticatedUser();

                options.Filters.Add(new AuthorizeFilter(policy.Build()));
            });
            
            services.AddRazorPages();

            services.AddScoped(services => 
                new HttpClient
                {
                    BaseAddress = new Uri(services.GetRequiredService<NavigationManager>().BaseUri)
                }
            );

            services.AddScoped<JwtService>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<NoteService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<Role> roleManager, UserManager<User> userManager)
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

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/Index");
            });

            // do this when adding user to a role !
            if (!roleManager.RoleExistsAsync(Role.Admin).Result)
            {
                roleManager.CreateAsync(new Role { Name = Role.Admin }).Wait();
            }

            var admin = userManager.FindByNameAsync("admin").Result;

            if (admin == null)
            {
                var newAdmin = new User { UserName = "admin", Email = "admin@admin.com" };

                userManager.CreateAsync(newAdmin, "admin").Wait();
                userManager.AddToRoleAsync(newAdmin, Role.Admin).Wait();
            }
        }
    }
}
