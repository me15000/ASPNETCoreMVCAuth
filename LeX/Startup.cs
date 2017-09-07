using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using LeX.PermissionMiddleware;
using LeX.Models.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;

namespace LeX
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string connecion = Configuration.GetConnectionString("DefaultConnection");

            services.Add(new ServiceDescriptor(typeof(Data.Common.DB.IDBHelper), p =>
            {
                return new Data.Common.DB.SQLServer.DBHelper(connecion);

            }, ServiceLifetime.Transient));



            services.AddAuthorization(options =>
            {
                //基于角色的策略
                // options.AddPolicy("RequireClaim", policy => policy.RequireRole("admin", "system"));
                //基于用户名
                //options.AddPolicy("RequireClaim", policy => policy.RequireUserName("桂素伟"));
                //基于Claim
                //options.AddPolicy("RequireClaim", policy => policy.RequireClaim(ClaimTypes.Country,"中国"));
                //自定义值
                // options.AddPolicy("RequireClaim", policy => policy.RequireClaim("date","2017-09-02"));
                //自定义Requirement,userPermission可从数据库中获得

                options.AddPolicy("Permission",
                          policy => policy.Requirements.Add(new PermissionRequirement()));

            })
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/login");
                options.AccessDeniedPath = new PathString("/denied");
            });

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //使用中间件
            //app.UsePermission(new PermissionMiddlewareOption());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
