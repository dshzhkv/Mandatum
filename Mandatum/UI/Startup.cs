using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Application.Api;
using Application.ApiInterface;
using Application.Converters;
using Application.DbContext;
using Application.EFRepository;
using Application.Entities;
using Application.RepositoryInterface;
using Mandatum.Controllers;
using Mandatum.Convertors;
using Mandatum.infra;
using Mandatum.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mandatum
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
            // >> db connect 
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("UI")));
            // << db connect 
            // >> api
            services.AddScoped<IBoardApi, BoardApi>();
            services.AddScoped<ITaskApi, TaskApi>();
            services.AddScoped<IUserApi, UserApi>();
            // << api
            // >> repo
            services.AddScoped<ITaskRepo, TaskRepo>();
            services.AddScoped<IBoardRepo, BoardRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            // << repo
            // >> convertors
            services.AddScoped<TaskConverterUILayer>();
            services.AddScoped<BoardConverterUILayer>();
            services.AddScoped<BoardConverterApiLayer>();
            services.AddScoped<TaskConverterAppLayer>();
            // << convertors
            // >> board formats
            services.AddScoped<IBoardFormat, KanbanBoardFormat>();
            services.AddScoped<IBoardFormat, TableBoardFormat>();
            // >> board formats
            // Auth 
            services.AddSingleton<IOAuthType, GoogleAuthType>();
            services.AddSingleton<IOAuthType, GitAuthType>();
            // установка конфигурации подключения
            services.AddScoped<ResponseHandler>();
            services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.ClientId = "e7f46bd747f935b05dec";
                    options.ClientSecret = "670a614ae729b7012b021acb41bedeb0f1c206eb";
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");
                    options.ClientId = "137321901177-9lsr1i1no0cui21t65pbej3jldqcb2cs.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });
            services.AddIdentity<UserRecord, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddControllersWithViews();
            services.AddControllers();
        }
 
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
 
            app.UseStaticFiles();
 
            app.UseRouting();
 
            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}