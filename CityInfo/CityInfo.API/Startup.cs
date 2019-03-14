using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            }).AddMvcOptions(o => o.OutputFormatters.Add(
                new XmlDataContractSerializerOutputFormatter()));
            services.AddTransient<IMailingService,MailingService>();
          //  string connectionString = @"Server=(LocalDb)\MSSQLLocalDB;DataBase = CityInfoDB;Trusted_Connection =True;"; 
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(Startup.Configuration["connectionStrings:CityInfoDBConnectionString"]));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory,CityInfoContext cityInfoContext)
        {
            //loggerFactory.AddDebug();   //In >net Core 2.0 it is by default so no need
            //add NLog to ASP.NET Core
            //https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2 followed this
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //cityInfoContext.EnsureSeedDateForContext();
            app.UseMvc();
            app.UseStatusCodePages();
            cityInfoContext.EnsureSeedDateForContext();
            //app.Run(async (context) =>
            //{
            //    throw new Exception("EXAMPLE EXCEPTION");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        } 
    }
}
