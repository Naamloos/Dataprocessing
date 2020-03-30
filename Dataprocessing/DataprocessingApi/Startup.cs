using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DatabaseHelper;
using DataprocessingApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataprocessingApi
{
    public class Startup
    {
        private Database database;
        private ConfigFile configFile;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // This generates a config file that MAY be invalid for the current setup. Check config.json.
            configFile = ConfigFile.Load();
            database = new Database(configFile.DbHost, configFile.DbName, configFile.DbUser, configFile.DbPass);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Return XML or JSON depending on headers
            services.AddMvc()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            services.AddSingleton(typeof(Database), database);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo(){ Title = "DataProcessing API",Version = "v1", 
                    Description = "Dataprocessing API by Ryan de Jonge @ NHL Stenden" +
                    "\nThe schema's listed here are not the ones I'm using for my validation, those can be found <a href=\"\">here.</a>" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "DataprocessingApi.xml");
                c.IncludeXmlComments(filePath);
                c.SchemaFilter<CustomXmlSchemaFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dataprocessing API v1");
                c.RoutePrefix = "docs";
                c.ConfigObject.DefaultModelsExpandDepth = -1;
            });

            app.UseStaticFiles();
        }
    }
}
