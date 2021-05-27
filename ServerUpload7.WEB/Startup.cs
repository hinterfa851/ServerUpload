using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServerUpload7.DAL.EF;
using Microsoft.EntityFrameworkCore;
using ServerUpload7.DAL.Repositories;
using ServerUpload7.DAL.Interfaces;
using ServerUpload7.BLL.Services;
using ServerUpload7.BLL.Interfaces;

namespace ServerUpload7.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("webconfig.json");
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); //, b => b.MigrationsAssambly("ServerUpload.WEB")
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IMaterialsService, MaterialsService>();
            services.AddTransient<IVersionsService, VersionsService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IConfiguration>(provider => Configuration);
            services.AddControllers().AddNewtonsoftJson(options =>
                 options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerUpload7.WEB", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerUpload7.WEB v1"));
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = c =>
                {
                    var exception = c.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exception.Error.GetType().Name switch
                    {
                        "CategoryException" => HttpStatusCode.BadRequest,
                        "MaterialExistException" => HttpStatusCode.BadRequest,
                        "MaterialNotExistException" => HttpStatusCode.NotFound,
                        "VersionExistException" => HttpStatusCode.BadRequest,
                        _ => HttpStatusCode.ServiceUnavailable
                    };
                    c.Response.StatusCode = (int)statusCode;

                    return Task.CompletedTask;
                }
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
