using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provide;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provide)
        {
            _provide = provide;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var v in _provide.ApiVersionDescriptions)
            {
                options.SwaggerDoc
                (
                    v.GroupName,
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Parky API {v.ApiVersion}",
                        Version = v.ApiVersion.ToString()
                    }

                );
            }

            //Add bearer to swagger UI

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                             "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                             "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                             "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
             });

            //Include ///XML comments in swagger UI
            var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";       //"Assembly.GetExecutingAssembly().GetName().Name" will retrive Project name from assembly
             var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
             options.IncludeXmlComments(cmlCommentsFullPath);
            
        }
    }   
}
