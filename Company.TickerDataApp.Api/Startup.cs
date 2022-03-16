using Company.TickerDataApp.Application.Features.Tickers;
using Company.TickerDataApp.Application.Services;
using Company.TickerDataApp.Database;
using Company.TickerDataApp.Database.Repositories;
using Company.TickerDataApp.Domain.Repositories;
using Company.TickerDataApp.GoogleSheetsSource;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Company.TickerDataApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.Configure<StoreSettings>(Configuration.GetSection(nameof(StoreSettings)));
            services.Configure<GoogleApiAuthSettings>(Configuration.GetSection(nameof(GoogleApiAuthSettings)));
            services.AddTransient<ITickersRepository, TickersRepository>();
            services.AddTransient<ITickersSourceService, GoogleSheetsTickersSource>();
            services.AddValidatorsFromAssembly(typeof(GetAll.Request).Assembly);
            services.AddMediatR(typeof(GetAll.Request));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TickerDataApp", Version = "v1"});
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TickerDataApp v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();
            app.UseCors(options =>
            {
                options.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(Configuration.GetSection("AllowedOrigins").Value.Split(';'));
            });
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
