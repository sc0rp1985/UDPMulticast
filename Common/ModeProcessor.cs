using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ModeProcessor : IProcessor
    {
        public string Name => "Mode";

        public async Task<StatResult> Process(ulong[] data)
        {
            return await Task.Run(() =>
            {
                var res = new StatResult {
                    StatName = Name,
                };
                try
                {
                    ulong max = 0;
                    ulong mode = 0;
                    for (var i = 0; i < data.Length; i++)
                    {
                        var di = data[i];
                        if (di > max)
                        {
                            max = di;
                            mode = (ulong)i;
                        }

                    }
                    res.Value = mode;
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
