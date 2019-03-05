﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace CityInfo.API
{
    public class Startup
    {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
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
          
            app.UseMvc();
            app.UseStatusCodePages();

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
