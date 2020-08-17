using System;

namespace ApacheNMSActiveMQTest.Models
{
	public class MessageType
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public string Message { get; set; }
	}

	public class SendMailMessage
	{
		public MessageType Message { get; set; }
	}
}
