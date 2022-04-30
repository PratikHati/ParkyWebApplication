using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            foreach(var v in _provide.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    v.GroupName,
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Parky API {v.ApiVersion}",
                        Version = v.ApiVersion.ToString()
                    }

                );

                //Include ///XML comments in swagger UI
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";       //"Assembly.GetExecutingAssembly().GetName().Name" will retrive Project name from assembly
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            }
        }
    }   
}
