using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using NLog.Web;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Project
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
			services.AddMvc();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Template", Version = "v1", });
			});

			var section = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(section);
			services.AddOptions();

			var appSettingsModel = new AppSettings();
			section.Bind(appSettingsModel);

			services.AddAuthorization(options =>
			{
				options.AddPolicy("NeedPhoneNumber", policy => policy.Requirements.Add(new PhoneNumberRequirement(appSettingsModel.PhoneRegex)));
			});

			services.AddSingleton<IAuthorizationHandler, pho>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			env.ConfigureNLog("NLog.config");
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "TemplateUi");
			});

			app.UseMvc();
		}
	}
}
