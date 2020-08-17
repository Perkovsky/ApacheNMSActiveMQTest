using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using ApacheNMSActiveMQTest.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApacheNMSActiveMQTest
{
	public class Service
	{
        public async Task StartAsync()
		{
			try
			{
				Log.Information("Service has been started.");
                Uri connecturi = new Uri("activemq:tcp://localhost:61616");
                Log.Information($"About to connect to {connecturi}");
                // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
                IConnectionFactory factory = new NMSConnectionFactory(connecturi);

                using (IConnection connection = factory.CreateConnection())
                using (ISession session = connection.CreateSession())
                {
                    IDestination destination = SessionUtil.GetDestination(session, "queue://send-mail");
                    Log.Information($"Using destination: {destination}");

                    using (IMessageConsumer consumer = session.CreateConsumer(destination))
                    {
                        connection.Start();
                        consumer.Listener += msg =>
                        {
                            var byteMsg = msg as ActiveMQBytesMessage;
                            var strData = Encoding.UTF8.GetString(byteMsg.Content);
                            var m = JsonConvert.DeserializeObject<SendMailMessage>(strData);
                            Log.Information($"Received message with ID: {m.Message.Id}, Text: '{m.Message.Message}', Date: {m.Message.CreatedDate}");
                        };

						await Task.Delay(Timeout.Infinite);
						//Thread.Sleep(Timeout.Infinite);
                    }
                }
            }
			catch (Exception ex)
			{
				Log.Error(ex, "Start service error");
			}
		}

		public async Task StopAsync()
		{
			try
			{
				Log.Information("Service has been stopped.");
				await Task.CompletedTask;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Stop service error");
			}
		}
	}
}
