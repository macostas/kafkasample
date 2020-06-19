using Confluent.Kafka;
using kAfkawebsample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace kAfkawebsample
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
            services.AddHostedService<ConsumerServices>();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Development_Cors_Policy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithExposedHeaders("content-disposition")
                        .AllowAnyMethod());
            });

            services.AddOptions<ProducerConfig>()
                .Configure<IConfiguration>((producer, configuration) =>
                {
                    configuration.GetSection("KafkaProducer").Bind(producer);
                });

            services.AddOptions<ConsumerConfig>()
                .Configure<IConfiguration>((consumer, configuration) =>
                {
                    configuration.GetSection("KafkaConsumer").Bind(consumer);
                });

            // Add framework services.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Kafka sample API",
                        Version = "1",
                        Description =
                            "sample",
                        TermsOfService = new Uri("https://example.com/terms")
                    });

                options.CustomSchemaIds(id => id.FullName);


                // Set default values to the API version parameter
                // options.OperationFilter<SwaggerDefaultValuesFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

            app.UseSwagger();
        }
	}
}
