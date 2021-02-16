using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public interface IStorage 
    {
        StorageInfo GetStorageInfo();
        void AddPackage(PackageDto package);
    }


    public class Storage : IStorage
    {       
        ulong[] data;        
        object _lockData = new object();
        ulong _lostCount;
        ulong _lastNum;
        ConcurrentQueue<PackageDto> cq; 
        bool _needSleep;
        Timer sleepTimer;



        public StorageInfo GetStorageInfo ()
        {            
            var copy = new ulong[data.Length];
            ulong lost = 0;
            int qc = cq.Count();
            lock (_lockData)
            {
                data.CopyTo(copy, 0);
                lost =_lostCount;

            }
            var info = new StorageInfo
            {
                Data = copy,
                Lost = lost,
                QueueCount = qc,
            };
            return info;            
        }

        void FillArray()
        {
            try
            {
                while (!cq.IsEmpty)
                {
                    PackageDto package;
                    if (cq.TryDequeue(out package))
                    {
                        var x = package.PackageNumber - _lastNum;
                        if (x > 1)
                        {
                            _lostCount += x;
                            Console.WriteLine("потеря");
                        }
                        _lastNum = package.PackageNumber;

                        lock (_lockData)
                        {
                            data[package.Value] += 1;                            
                        }
                    }

                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        public Storage()
        {
            data= new ulong[GlobalConfig.Instance.Range.EndVal];
            cq = new ConcurrentQueue<PackageDto>();
            var fillThread = new Thread(()=> {
                while (true) {
                    FillArray();
                    Thread.Sleep(100);
                }
            });
            fillThread.Start();
            sleepTimer = new Timer(
                (obj) =>
                {
                   _needSleep = true;
                }, null, 0, GlobalConfig.Instance.ReceiverDelay.TickMs);
                
        }          
        
        public void AddPackage(PackageDto package) 
        {
            if (_needSleep) 
            {
                _needSleep = false;
                Thread.Sleep(GlobalConfig.Instance.ReceiverDelay.SleepMs);
                return;
            }
            cq.Enqueue(package);
        }
    }

    public class StorageInfo 
    { 
        public ulong[] Data { get; set; }
        public ulong Lost { get; set; }        
        public int QueueCount { get; set; }
    }
}
