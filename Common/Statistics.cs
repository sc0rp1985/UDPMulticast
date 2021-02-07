using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class Statistics
    {
        static List<StatResult> _statCache = new List<StatResult>();
        static readonly List<IProcessor> statList = new List<IProcessor> { new AvgProcessor(), new CountProcessor(), new StandartDeviationProcessor(), new ModeProcessor(), new Median() };
        static object _lockObj = new object();
        static Statistics()
        {
            var statThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        CalcStat();
                        Thread.Sleep(2000);
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            });
            statThread.Start();
        }

        static void CalcStat()
        {
            var si = Storage.StorageInfo;
            var copy = si.Data;

            var ta = statList.Select(x => x.Process(copy)).ToArray();
            Task.WaitAll(ta);            
            var tmp = ta.Select(x => x.Result).ToList();
            tmp.Add(new StatResult
            {
                StatName = "потеряно",
                Value = si.Lost,
            });
            tmp.Add(new StatResult
            {
                StatName = "буфер",
                Value = si.QueueCount,
            });
            lock (_lockObj)
            {
                _statCache = tmp;
            }
        }

        public static List<StatResult> GetStatistic()
        {
            lock(_lockObj)
                return _statCache.ToList();
        }
    }


}
