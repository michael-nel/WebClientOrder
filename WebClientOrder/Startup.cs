using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebClientOrder.Domain.Handler;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Services;
using WebClientOrder.Infra.Context;
using WebClientOrder.Infra.Map;
using WebClientOrder.Infra.Repositories;
using WebClientOrder.Infra.Services;

namespace WebClientOrder
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
            services.AddControllers()
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
              .AddJsonOptions(opt =>
              {
                  var serializerOptions = opt.JsonSerializerOptions;
                  serializerOptions.IgnoreNullValues = true;
                  serializerOptions.IgnoreReadOnlyProperties = false;
                  serializerOptions.WriteIndented = true;
              });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebClientOrder", Version = "v1" });
            });

            MongoConfigure.Configure();

            #region Context
            services.AddScoped<IMongoContext, MongoContext>();
            #endregion

            #region Repository
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            #endregion

            #region Handler
            services.AddTransient<ProductHandler, ProductHandler>();
            services.AddTransient<ClientHandler, ClientHandler>();
            services.AddTransient<OrderHandler, OrderHandler>();
            #endregion Handler

            #region Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICepService, CepService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebClientOrder v1"));
            }

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
