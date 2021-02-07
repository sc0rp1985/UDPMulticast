using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Стандартное отклонение
    /// </summary>
    public class StandartDeviationProcessor : IProcessor
    {
        public string Name => "StandDev";

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
                    var avg = Utils.Average(data);
                    double sum = 0;
                    ulong n = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        var di = data[i];
                        if (di > 0)
                        {
                            var a = Utils.mul64hi(di, (ulong)i);
                            var b = (avg * di);
                            var t = a - b;
                            sum += t * t;
                            n += di;
                        }
                    }
                    res.Value = Math.Sqrt(sum / (n - 1));
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
