using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;

namespace WebApiTZ
{
	public class Program
	{
		public static void Main(string[] args)
		{
			EnsureDB();

			var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

			try
			{
				logger.Debug("init main");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception exception)
			{
				//NLog: catch setup errors
				logger.Error(exception, "Stopped program because of exception");
				throw;
			}
			finally
			{
				// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
				NLog.LogManager.Shutdown();
			}
		}

		static void EnsureDB()
		{
			if (System.IO.File.Exists(@"C:\temp\logs.db"))
				return;

			using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=C:\temp\logs.db"))
			using (SQLiteCommand command = new SQLiteCommand(
				"CREATE TABLE logging (longdate TEXT, level TEXT, logger TEXT, message TEXT, stacktrace TEXT)",
				connection))
			{
				connection.Open();
				command.ExecuteNonQuery();
			}

		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
				})
				.UseNLog();  
	}
}
