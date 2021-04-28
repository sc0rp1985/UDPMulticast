using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public interface IStaticstic
    {
        IList<StatResult> GetStatistic();

    }

    public class Statistics : IStaticstic
    {
         List<StatResult> _statCache = new List<StatResult>();
        readonly List<IProcessor> statList = new List<IProcessor> { new AvgProcessor(), new CountProcessor(), new StandartDeviationProcessor(), new ModeProcessor(), new Median() };
        object _lockObj = new object();
        IStorage _storage;
        public Statistics(IStorage storage)
        {
            _storage = storage;
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

        public async void CalcStat()
        {
            var si = _storage.GetStorageInfo();
            var copy = si.Data;

            var ta = statList.Select(x => x.Process(copy)).ToArray();
            await Task.WhenAll(ta);            
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

        public IList<StatResult> GetStatistic()
        {
            lock(_lockObj)
                return _statCache.ToList();
        }
    }


}
