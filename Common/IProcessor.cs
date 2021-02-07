using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{    
    public interface IProcessor
    {
        string Name { get; }
        Task<StatResult> Process(ulong[] data);
    }

    public class StatResult
    {
        public string StatName { get; set; }
        public double Value { get; set; }
    }


}
