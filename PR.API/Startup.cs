using HRM.Domain.Constants;
using HRM.Domain.Entities;
using HRM.Service.HR.Interfaces;
using HRM.Service.HR.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PR.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(p => p.AddDefaultPolicy(build =>
            {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            services.AddDbContext<HrmContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(ConfigurationKey.HRMConnectionString));
            });
            services.AddTransient<INotificationService, NotificationService>();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIOpenSource");
                });
            }
            app.UseCors(build =>
            {
                build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.MapControllers();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

        }
    }
}
