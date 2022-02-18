using Cognizant.TickerDataApp.Application.Features.Tickers;
using Cognizant.TickerDataApp.Application.Services;
using Cognizant.TickerDataApp.Database;
using Cognizant.TickerDataApp.Database.Repositories;
using Cognizant.TickerDataApp.Domain.Repositories;
using Cognizant.TickerDataApp.GoogleSheetsSource;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Cognizant.TickerDataApp.Api
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
            services.AddTransient<ITickersRepository, TickersRepository>();
            services.AddTransient<ITickersSourceService, GoogleSheetsTickersSource>();
            services.AddMediatR(typeof(GetAll.Request));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "TickerDataApp", Version = "v1"});
            });
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
