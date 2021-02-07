using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AvgProcessor : IProcessor
    {
        public string Name { get => "Avg"; }

        public async Task<StatResult> Process(ulong[] data)        
        {
                return await Task.Run(() =>
                {
                    var res = new StatResult
                    {
                        StatName = Name,
                    };
                    try
                    {
                        res.Value =  Utils.Average(data);
                        return res;
                    }
                    catch (Exception e)
                    {
                        res.Value = double.NaN;
                        return res;                        
                    }
                });            
            
        }       
    }
    
    public class CountProcessor : IProcessor
    {
        public string Name { get => "Count"; }

        public async Task<StatResult> Process(ulong[] data)        
        {
            return await Task.Run(() =>
            {
                var res = new StatResult
                {
                    StatName = Name,
                };
                try
                {                    
                    ulong count = 0;
                    for (var i = 0; i < data.Length; i++)
                    {
                        var di = data[i];
                        count += di;
                    }
                    res.Value = count;
                    return res;
                }
                catch(Exception e)
                {
                    res.Value = double.NaN;
                    return res;
                }
            });
        }       
    }

    
}
