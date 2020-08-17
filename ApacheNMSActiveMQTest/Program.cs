using MG.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Topshelf;

namespace ApacheNMSActiveMQTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBaseConfiguration()
				.Build();
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();

			HostFactory.Run(x =>
			{
				x.Service<Service>(s =>
				{
					s.ConstructUsing(name => new Service());
					s.WhenStarted(async c => await c.StartAsync());
					s.WhenStopped(async c => await c.StopAsync());
				});
				x.RunAsLocalSystem();

				x.SetDescription("Apache NMS ActiveMQ Test");
				x.SetDisplayName("Apache NMS ActiveMQ Test");
				x.SetServiceName("Apache NMS ActiveMQ Test");
			});
		}
	}
}
