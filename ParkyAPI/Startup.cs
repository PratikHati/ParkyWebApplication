using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkyAPI.Date;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ParkyAPI.ParkyMapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.(Dependency Injection done here)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<INationalParkRepository, NationalParkRepository>();      //Scoping of interface
             
            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkyMappings));          //automapper to map DTO to original models

            services.AddApiVersioning(options =>                    //api versionaing
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options=>options.GroupNameFormat = "'v'VVV");  //adding version names

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen();

            /*services.AddSwaggerGen(options=> {                      //API documentation
                options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                { 
                    Title = "Parky API",
                    Version = "1",
                    Description="Parky API NP",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "pratikbablu267@gmail.com",
                        Name = "Pratik Kumar Hati"
                    }
                });
                *//*options.SwaggerDoc("ParkyOpenAPISpecTrails", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Parky API Trails",
                    Version = "1",
                    Description = "Parky API Trails",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "pratikbablu267@gmail.com",
                        Name = "Pratik Kumar Hati"
                    }
                });*//*
                //Include ///XML comments in swagger UI
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";       //"Assembly.GetExecutingAssembly().GetName().Name" will retrive Project name from assembly
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });*/

            services.AddControllers();  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.(middleware)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options => {
                foreach(var v in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{v.GroupName}/swagger.json", v.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });

            /*app.UseSwaggerUI(options=> {
                options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");

                //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "Parky API(National Park)");
                //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");
                
                options.RoutePrefix = "";           //we want to use SwaggerUI page as default page, so change "launchUrl" in launchappsetting.json
            });*/

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}   
