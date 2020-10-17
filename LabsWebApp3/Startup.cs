using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using LabsWebApp3.Models.Domain;
using LabsWebApp3.Models.Domain.Repositories.Abstract;
using LabsWebApp3.Models.Domain.Repositories.EntityFramework;
using LabsWebApp3.Helpers;

namespace LabsWebApp3
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //подключаем конфиг из appsetting.json
            Configuration.Bind("Project", new Config());
            Config.WebRootPath = Path
                .Combine(Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "wwwroot");

            //подключаем нужный функционал приложения в качестве сервисов
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IEventItemsRepository, EFEventItemsRepository>();
            services.AddTransient<DataManager>();

            //подключаем контекст БД
            services.AddDbContext<EFAppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            //настраиваем identity систему и сложность пароля
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<EFAppDbContext>().AddDefaultTokenProviders();

            //настраиваем authentication cookie
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "LabsWebApp3Auth";
            //    options.Cookie.HttpOnly = true;
            //    options.LoginPath = "/account/login";
            //    options.AccessDeniedPath = "/account/accessdenied";
            //    options.SlidingExpiration = true;
            //});

            //настраиваем политику авторизации для Admin area
            //позже настроим для чата
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });

            //добавляем сервисы для контроллеров и представлений (MVC)
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            })
                //выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //!!! порядок регистрации middleware очень важен

            //в процессе разработки нам важно видеть какие именно ошибки
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //подключаем поддержку статичных файлов в приложении (css, js и т.д.)
            app.UseStaticFiles();

            //подключаем систему маршрутизации
            app.UseRouting();

            //подключаем аутентификацию и авторизацию
           // app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //регистрируем нужные нам маршруты (ендпоинты)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", 
                  "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
