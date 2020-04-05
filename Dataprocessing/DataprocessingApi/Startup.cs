using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;
using DatabaseHelper;
using DatabaseHelper.Models;
using DataprocessingApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DataprocessingApi
{
    public class Startup
    {
        private Database database;
        private ConfigFile configFile;
        private IsoCountries isoCountries;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // This generates a config file that MAY be invalid for the current setup. Check config.json.
            configFile = ConfigFile.Load();

            // connecting to database.
            database = new Database(configFile.DbHost, configFile.DbName, configFile.DbUser, configFile.DbPass);

            isoCountries = IsoCountries.Load();
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

            // Adding database dependency
            services.AddSingleton(typeof(Database), database);
            services.AddSingleton(typeof(IsoCountries), isoCountries);

            // cross origin requests
            services.AddCors(o => o.AddPolicy("publicpolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            // Setting up swagger documentation generation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo(){ Title = "DataProcessing API",Version = "v1", 
                    Description = "Dataprocessing API by Ryan de Jonge @ NHL Stenden" +
                    "\nThe schema's listed here are not the ones I'm using for my validation, those can be found <a href=\"\">here.</a>" });

                // Custom schema to work around some errors in the swagger lib
                c.SchemaFilter<CustomXmlSchemaFilter>();

                // I want to use my xmldocs for api info.
                var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DataprocessingApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        public void Register(HttpConfiguration config)
        {
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Deafult aspnet stuff
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

            // Documentation UI at /docs
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dataprocessing API v1");
                c.RoutePrefix = "docs";
                c.ConfigObject.DefaultModelsExpandDepth = -1;
            });

            // Using static files and setting index.html as default file.
            DefaultFilesOptions dfo = new DefaultFilesOptions();
            dfo.DefaultFileNames.Clear();
            dfo.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(dfo);
            app.UseStaticFiles();

            // Voor serven van schemas
            app.UseFileServer(new FileServerOptions()
            {
                RequestPath = "/schemas",
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(
                        // Getting current directory the exe/dll is in (dont remember which I compiled lol)
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "schemas")
                    )
            });
        }
    }
}
