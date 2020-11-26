using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class QOutputParam
    {
        public string Name { get; set; }
        public List<QOutputParamValue> Values { get; set; } = new List<QOutputParamValue>();
        public double[] RoValues { get => Values.Select(val => val.Ro).ToArray(); }
    }

    public class QOutputParamValue
    {
        public double Ro { get; set; }
        public double Value { get; set; }
        public double Expected { get; set; }

        public string sRo { get => Ro.ToString("0.00"); }
        public string sValue { get => Value.ToString("0.0000"); }
        public string sExpected { get => Expected.ToString("0.0000"); }
    }

}
