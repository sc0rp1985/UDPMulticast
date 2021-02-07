using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using Common;
using System.Threading;

namespace Server
{
    class Program
    {
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
                var sleepTime = 0;
                var sendError = false;
                while (true)
                {
                    var val = rnd.Next(config.Range.StartVal,config.Range.EndVal);
                    number++;
                    Console.WriteLine($"send value {val} nummber {number}");
                    try
                    {
                        proxy.SendPackage(new PackageDto
                        {
                            PackageDate = DateTime.Now,
                            PackageNumber = number,
                            Value = val,
                        });
                        sendError = false;
                        sleepTime = 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ошибка отправки сообщения: " + e.Message);
                        Console.WriteLine($"Повтор через {sleepTime} c.");
                        Thread.Sleep(sleepTime * 1000);
                        sendError = true;
                    }
                    if (sendError) 
                    {
                        if (sleepTime == 0)
                            sleepTime = 10;
                        else if (sleepTime < 30)
                            sleepTime = 30;
                        else if (sleepTime < 60)
                            sleepTime = 60;
                        else if (sleepTime < 300)
                            sleepTime = 300;
                        else if (sleepTime < 600)
                            sleepTime = 600;
                    }
                    
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
