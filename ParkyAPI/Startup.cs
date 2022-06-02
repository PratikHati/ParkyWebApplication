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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddScoped<IUserRepository, UserRepository>();


            services.AddAutoMapper(typeof(ParkyMappings));          //automapper to map DTO to original models

            services.AddApiVersioning(options =>                    //api versionaing
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);  //default api version in "1"
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options=>options.GroupNameFormat = "'v'VVV");  //adding version names

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen();

            var appsettingSection = Configuration.GetSection("AppSettings");    //access section of appsetting.json

            services.Configure<AppSettings>(appsettingSection);   //Secret key hidden from public repos and encryption added

            var appSetting = appsettingSection.Get<AppSettings>();  

            var key = Encoding.ASCII.GetBytes(appSetting.Secret);   //access actual key from the key section

            services.AddAuthentication(x =>
            {                //bearertoken support
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x=> {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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
