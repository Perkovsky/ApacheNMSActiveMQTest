using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MG.Common.Extensions
{
	public static partial class IConfigurationBuilderExtensions
	{
		public static IConfigurationBuilder SetBaseConfiguration(this IConfigurationBuilder builder)
		{
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
			var environmentFileName = $"appsettings.{environmentName}.json";
			var settingFileName = "appsettings.json";

			return builder.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(settingFileName, optional: true)
				.AddJsonFile(environmentFileName, optional: true);
		}
	}
}
