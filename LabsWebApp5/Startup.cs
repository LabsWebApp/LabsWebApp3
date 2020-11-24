using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LabsWebApp5.Models.Domain;
using LabsWebApp5.Models.Domain.Repositories.Abstract;
using LabsWebApp5.Models.Domain.Repositories.EntityFramework;
using LabsWebApp5.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(LabsWebApp5.Startup))]

namespace LabsWebApp5
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //���������� ������ �� appsetting.json
            Configuration.Bind("Project", new Config());
            Config.WebRootPath = Path
                .Combine(Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "wwwroot");

            //���������� ������ ���������� ���������� � �������� �������
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IEventItemsRepository, EFEventItemsRepository>();
            services.AddTransient<DataManager>();

            //���������� �������� ��
            services.AddDbContext<EFAppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            //����������� identity ������� � ��������� ������
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<EFAppDbContext>()
                .AddDefaultTokenProviders();

            //����������� authentication cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "LabsWebApp3Auth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            //����������� �������� ����������� ��� Admin area
            //����� �������� ��� ����
            services.AddAuthorization(x =>
            {
                x.AddPolicy("ChatArea", policy 
                    => { policy.RequireRole(Config.RoleReader);});
                x.AddPolicy("AdminArea", policy 
                    => { policy.RequireRole(Config.RoleAdmin); });
            });

            //��������� ������� ��� ������������ � ������������� (MVC)
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AreaAuthorization("Chat", "ChatArea"));
                x.Conventions.Add(new AreaAuthorization("Admin", "AdminArea"));
            })
                //���������� ������������� � asp.net core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //!!! ������� ����������� middleware ����� �����

            //� �������� ���������� ��� ����� ������ ����� ������ ������
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //���������� ��������� ��������� ������ � ���������� (css, js � �.�.)
            app.UseStaticFiles();

            //���������� ������� �������������
            app.UseRouting();

            //���������� �������������� � �����������
            // app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

           // app.MapSignalR();

            //������������ ������ ��� �������� (���������)
            app.UseEndpoints(endpoints =>
            {
               // endpoints.MapHub<ChatHub>("/Chat");
                endpoints.MapControllerRoute(Config.Admin,
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
