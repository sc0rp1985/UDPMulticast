using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
	[XmlRoot("config")]
	public class GlobalConfig
	{
		public static GlobalConfig Instance { get; }

		static GlobalConfig()
		{
			var serializer = new XmlSerializer(typeof(GlobalConfig));

			using (var reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory+"config.xml"))
			{
				using (var strReader = new StringReader(reader.ReadToEnd()))
				{
					Instance = (GlobalConfig)serializer.Deserialize(strReader);
				}
			}
		}

		[XmlElement("multicastGroupAddress")]
		public MulticastAddress Multicast { get; set; }
		[XmlElement("valueRange")]
		public ValueRange Range { get; set; }
		[XmlElement("sendDelay")]
		public SendDelay SenderDelay { get; set; }
		[XmlElement("receiveDelay")]
		public ReceiveDelay ReceiverDelay { get; set; }

		public class MulticastAddress 
		{ 
			[XmlAttribute("address")]
			public string Address { get; set; }

		}

		public class ValueRange 
		{
			[XmlAttribute("from")]
			public int StartVal { get; set; }
			[XmlAttribute("to")]
			public int EndVal { get; set; }
		}

		public class SendDelay 
		{
			[XmlAttribute("delayMs")]
			public int Delay { get; set; }
		}
		public class ReceiveDelay 
		{
			[XmlAttribute("tickMs")]
			public int TickMs { get; set; }
			[XmlAttribute("sleepMs")]
			public int SleepMs { get; set; }
		}
	}
}
