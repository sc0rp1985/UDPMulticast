using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using Common;

namespace Server
{
    class Program
    {
        

        public class ValueGenerator 
         { 

         }


         static void Main(string[] args)
         {
            var config = GlobalConfig.Instance;            
            ulong number = 0;
            var rnd = new Random();
            Uri baseAddress = new Uri(config.Multicast.Address);
            UdpBinding myBinding = new UdpBinding();
            ChannelFactory<IReceiveService> factory = new ChannelFactory<IReceiveService>(myBinding, new EndpointAddress(baseAddress));
            IReceiveService proxy = factory.CreateChannel();
            try
            {
                while (true)
                {
                    var val = rnd.Next(config.Range.StartVal,config.Range.EndVal);
                    number++;
                    Console.WriteLine($"send value {val} nummber {number}");
                    proxy.SendPackage(new PackageDto
                    {
                        PackageDate = DateTime.Now,
                        PackageNumber = number,
                        Value = val,
                    });
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0,0,config.SenderDelay.Delay));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }      
       
    }
}
