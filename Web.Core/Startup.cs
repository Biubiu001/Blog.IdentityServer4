using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Web.Core.AOP;
using Web.Core.AuthHelper.OverWrite;
using Web.Core.Common.MemoryCache;
using Web.Core.Extensions;
using Web.Core.IServices;

namespace Web.Core
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
            services.AddControllers();
            services.AddSwaggerSetup();
           // services.AddAuthorizationSetup();

            services.Configure<JwtDemo>(Configuration.GetSection("tokenConfig"));

            services.AddScoped<ICaching, MemoryCaching>();
            var token = Configuration.GetSection("tokenConfig").Get<JwtDemo>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuerSigningKey = true,
                        //获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey用于签名验证。
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                    GetBytes(token.Secret)),
                        //获取或设置一个System.String，它表示将使用的有效发行者检查代币的发行者。
                        ValidIssuer = token.Issuer,
                        //获取或设置一个字符串，该字符串表示将用于检查的有效受众反对令牌的观众。
                        ValidAudience = token.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew=TimeSpan.Zero,

                    };
                });

        }

        public void ConfigureContainer(ContainerBuilder builder) {

            var basePath = AppContext.BaseDirectory;
            //  builder.RegisterType<AdvertisementServices>().As<IAdvertisementService>();

            builder.RegisterType<BlogLopAOP>();
            builder.RegisterType<BlogCacheAOP>();
              var serviceDllFile = Path.Combine(basePath,"Web.Core.Service.dll");

            var repositoryDllFile = Path.Combine(basePath, "Web.Core.Repostitory.dll");

            var assemblysServices = Assembly.LoadFrom(serviceDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
               .InterceptedBy(typeof(BlogCacheAOP));

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "My API V1");
                c.RoutePrefix = "";
            });


            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseStatusCodePages();
            app.UseRouting();


           

            //先开启认证
            app.UseAuthentication();
            // 开启异常中间件，要放到最后
            app.UseAuthorization();

            app.UseMiddleware<JwtTokenAuth>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
