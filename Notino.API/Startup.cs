using Alphacloud.MessagePack.AspNetCore.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notino.API.Middleware;
using Notino.Application.DI;
using Notino.Persistence.MSSQL.DI;
using Notino.Persistence.HDD.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notino.Application.Settings;

namespace Notino.API
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            AddSwaggerDoc(services);
            services.Configure<PersistenceSettings>(Configuration.GetSection(nameof(PersistenceSettings)));
            services.AddOptions();

            services.ConfigureApplicationServices();
            services.ConfigurePersistenceServices(Configuration);
            services.ConfigureFilePersistenceServices(Configuration);

            //tototo konstanta
            services.AddCors(o =>
                o.AddPolicy(CorsPolicyName,
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            //services.AddControllers()
            //    .AddNewtonsoftJson()
            //    .AddXmlSerializerFormatters();

            services.AddControllers(options => { 
               
            
            });
           
            services.AddMessagePack(options => { 
                options.MediaTypes.Add("application/x-msgpack");
               
            });

            services.AddMemoryCache();
        }

        private void AddSwaggerDoc(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "HR.LeaveManagement.Api",
                    Version = "v1"
                });
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR.LeaveManagement.Api v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(CorsPolicyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
