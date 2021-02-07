using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Median : IProcessor
    {
        public string Name => "Median";

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

                    ulong sum = 0;
                    Dictionary<int, ulong> d = new Dictionary<int, ulong>();
                    for (var i = 0; i < data.Length; i++)
                    {
                        var di = data[i];
                        if (di > 0)
                        {
                            sum += di;
                            d.Add(i, sum);
                        }
                    }
                    var n = (sum + 1) / 2;
                    var me = d.First(x => x.Value >= n).Key;

                    res.Value = me;
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
}
