using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Web.Core.Common;
using Web.Core.Common.Helper;
using static Web.Core.SwaggerHelper.CustomApiVersion;

namespace Web.Core.Extensions
{
    /// <summary>
    /// 启动swagger 
    /// </summary>
    public static class SwaggerSetup
    {

        public static void AddSwaggerSetup(this IServiceCollection service) {

            if (service == null) throw new ArgumentNullException(nameof(service));

            var basePath = AppContext.BaseDirectory;
            var apiName = Appsettings.app(new string[] { "Startup","ApiName"});

            
            service.AddSwaggerGen(c => {

                typeof(ApiVirsionScope).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{apiName} 接口文档——Netcore 3.1",
                        Description = $"{apiName} HTTP API " + version,
                        Contact = new OpenApiContact { Name = apiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                        License = new OpenApiLicense { Name = apiName + " 官方文档", Url = new Uri("http://apk.neters.club/.doc/") }
                    });
                    c.OrderActionsBy(o => o.RelativePath);
                });
                //开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header 中添加token 传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(basePath, xmlFile);
                c.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）
                var xmlModelPath = Path.Combine(basePath, "Web.Core.Model.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey

                });

            });



        }

    }
}
