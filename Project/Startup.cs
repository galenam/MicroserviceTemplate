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
using Project.Auth;
using Common.Const;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;

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
			services.AddMvc(options =>
			{
				options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "Template", Version = "v1", });
			});

			services.Configure<AppSettings>(options => Configuration.GetSection(nameof(AppSettings)).Bind(options));
			var section = Configuration.GetSection(nameof(AppSettings));
			var appSettingsModel = new AppSettings();
			section.Bind(appSettingsModel);

			services.AddAuthentication(o =>
			{
				o.DefaultScheme = PolicyName.PhoneNumber;
			}).AddScheme<PhoneNumberAuthenticationOptions, PhoneNumberAuthenticationHandler>(PolicyName.PhoneNumber, o =>
			{
				o.PhoneMask = new System.Text.RegularExpressions.Regex(appSettingsModel.PhoneRegex);
			});

			services.AddDataProtection();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
		{
			factory.AddDebug();
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
			app.UseAuthentication();
			app.UseStaticFiles();

			app.UseMvc();
		}
	}
}
