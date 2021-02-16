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
            var storage = new Storage();
            var stat = new Statistics(storage);            

            UdpBinding myBinding = new UdpBinding();
            var srv = new ReceiveService(storage);
            ServiceHost host = new ServiceHost(srv, new Uri(config.Multicast.Address));            
            host.AddServiceEndpoint(typeof(IReceiveService), myBinding, string.Empty);
            host.Open();            
            Console.WriteLine("Start receiving stock information");
            
            while (true) 
            {
                Console.ReadLine();
                try
                {
                    var statList = stat.GetStatistic();
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
