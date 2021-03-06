using System.IO;
using System.Reflection;
using DatabaseHelper;
using DataprocessingApi.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace DataprocessingApi
{
    /// <summary>
    /// ASP.Net Core Startup class.
    /// </summary>
    public class Startup
    {
        private readonly Database database;
        private readonly ConfigFile configFile;
        private readonly IsoCountries isoCountries;

        /// <summary>
        /// Constructs a new Startup object.
        /// </summary>
        /// <param name="configuration">Configuration to use.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // This generates a config file. Check config.json.
            configFile = ConfigFile.Load();

            // connecting to database.
            database = new Database(configFile.DbHost, configFile.DbName, configFile.DbUser, configFile.DbPass);

            // Loading iso countries json file
            isoCountries = IsoCountries.Load();
        }

        /// <summary>
        /// Current config.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures Services.
        /// </summary>
        /// <param name="services">Servicecollection to use.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Return XML or JSON depending on headers. 
            // ASP.Net Core doesn't ship with XML by default.
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
                    "\nLocal paths to the XML/JSON schema files will be provided in the \"link\" header." +
                    "\nThe schemas generated by Swagger are NOT the ones I use."
                });

                // Custom schema to work around some errors in the swagger lib
                c.SchemaFilter<CustomXmlSchemaFilter>();

                // I want to use my xmldocs for api info.
                var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DataprocessingApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// Configures the HTTP pipeline.
        /// </summary>
        /// <param name="app">appbuilder to use.</param>
        /// <param name="env">environment to use.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Default aspnet stuff
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

            // File server to serve schemas
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
