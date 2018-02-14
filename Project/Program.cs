using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Project
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((builderContext, config) =>
				{
					IHostingEnvironment env = builderContext.HostingEnvironment;
					builderContext.HostingEnvironment.ConfigureNLog("nlog.config");
					config
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile($"appsettings.json", true, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, reloadOnChange: true)
					.Build();
				})
				.UseStartup<Startup>()
				.UseNLog()
				.Build();
	}
}
