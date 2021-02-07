using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = GlobalConfig.Instance;
            //прогрев 
            var t = Storage.Count;
            Statistics.GetStatistic();

            UdpBinding myBinding = new UdpBinding();            
            ServiceHost host = new ServiceHost(typeof(ReceiveService), new Uri(config.Multicast.Address));            
            host.AddServiceEndpoint(typeof(IReceiveService), myBinding, string.Empty);
            host.Open();            
            Console.WriteLine("Start receiving stock information");
            
            while (true) 
            {
                Console.ReadLine();
                try
                {
                    var statList = Statistics.GetStatistic();
                    Console.WriteLine(string.Join(", ", statList.Select(x => $"{x.StatName} = {x.Value}")));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
            }            
        }
    }
}
