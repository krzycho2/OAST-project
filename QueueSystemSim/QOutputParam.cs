using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class QOutputParams
    {
        public bool Filled { get; private set; } = false;
        public QOutputParam ET { get; private set; }
        public QOutputParam EW { get; private set; }
        public QOutputParam EQ { get; private set; }
        public QOutputParam EN { get; private set; }
        public double[] RoValues { get => ET.RoValues; }

        public List<QOutputParam> AllParams { get => new List<QOutputParam> { ET, EW, EN, EQ }; }

        public QOutputParams() { }

        public QOutputParams(double et, double expectedET, double ew, double expectedEW, double eq, double expectedEQ, double en, double expectedEN, double roValue)
        {
            ET = QOutputParam.CreateParamForSingleRo("ET", et, expectedET, roValue);
            EW = QOutputParam.CreateParamForSingleRo("EW", ew, expectedEW, roValue);
            EQ = QOutputParam.CreateParamForSingleRo("EQ", eq, expectedEQ, roValue);
            EN = QOutputParam.CreateParamForSingleRo("EN", en, expectedEN, roValue);
            Filled = true;
        }

        public void AddEntry(QOutputParams newParams)
        {
            if(!Filled)
            {
                ET = newParams.ET;
                EW = newParams.EW;
                EN = newParams.EN;
                EQ = newParams.EQ;
                Filled = true;
            }
            else
            {
                ET = QOutputParam.Compose(new List<QOutputParam> { ET, newParams.ET });
                EW = QOutputParam.Compose(new List<QOutputParam> { EW, newParams.EW });
                EN = QOutputParam.Compose(new List<QOutputParam> { EN, newParams.EN });
                EQ = QOutputParam.Compose(new List<QOutputParam> { EQ, newParams.EQ });
            }
            
        }

        public static QOutputParams Compose(List<QOutputParams> outputParamsList)
        {
            var composed = new QOutputParams();
            var ETs = outputParamsList.Select(oParams => oParams.ET).ToList();
            var EWs = outputParamsList.Select(oParams => oParams.EW).ToList();
            var EQs = outputParamsList.Select(oParams => oParams.EQ).ToList();
            var ENs = outputParamsList.Select(oParams => oParams.EN).ToList();

            composed.ET = QOutputParam.Compose(ETs);           
            composed.EW = QOutputParam.Compose(EWs);           
            composed.EQ = QOutputParam.Compose(EQs);           
            composed.EN = QOutputParam.Compose(ENs);

            return composed;
        }
    }
    public class QOutputParam
    {
        public string Name { get; set; }
        public List<QOutputParamValue> Values { get; set; } = new List<QOutputParamValue>();
        public double[] RoValues { get => Values.Select(val => val.Ro).ToArray(); }

        public static QOutputParam CreateParamForSingleRo(string name, double value, double expected, double ro)
        {
            var values = new List<QOutputParamValue>();
            values.Add(new QOutputParamValue { Ro = ro, Value = value, Expected = expected });
            return new QOutputParam { Name = name, Values = values };
        }

        public static QOutputParam Compose(List<QOutputParam> outputParams)
        {
            var values = new List<QOutputParamValue>();
            foreach (var oParam in outputParams)
                foreach (var value in oParam.Values)
                    values.Add(value);

            return new QOutputParam {Name = outputParams[0].Name, Values = values };
        }
    }

    public class QOutputParamValue
    {
        public double Ro { get; set; }
        public double Value { get; set; }
        public double Expected { get; set; }
        public double Error { get => Math.Abs(Value - Expected) / Expected; }

        public string sRo { get => Ro.ToString("0.00"); }
        public string sValue { get => Value.ToString("0.0000"); }
        public string sExpected { get => Expected.ToString("0.0000"); }
        public string sError { get => (Error).ToString("0.0%"); }
    }

}
