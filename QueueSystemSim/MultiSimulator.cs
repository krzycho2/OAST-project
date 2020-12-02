using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class MultiSimulator
    {
        private double imgClientServiceProbaSum = 0.0;

        public int nRuns { get; set; } = 500;

        public EventSimulator EventSimulator { get; private set; }

        public Stats GlobalStats { get; set; } = new Stats();

        public double ImgClientServicedProbability { get => imgClientServiceProbaSum / nRuns; }

        public QOutputParams OutputParams 
        {
            get => new QOutputParams(
                GlobalStats.ET, EventSimulator.QueueSystem.ExpectedET,
                GlobalStats.EW, EventSimulator.QueueSystem.ExpectedEW,
                GlobalStats.EN, EventSimulator.QueueSystem.ExpectedEN,
                GlobalStats.EQ, EventSimulator.QueueSystem.ExpectedEQ,
                EventSimulator.QueueSystem.QueueParamRo);
        }
        

        public MultiSimulator(Mm1QueueSystem queueSystem)
        {
            EventSimulator = new EventSimulator(queueSystem);
        }

        public void RunSim()
        {
            for (int i = 0; i < nRuns; i++)
            {
                EventSimulator.RunSim();
                CollectStats();
            }
        }

        private void CollectStats()
        {
            double en = EventSimulator.EN;
            double et = EventSimulator.ET;
            double ew = EventSimulator.EW;
            double eq = EventSimulator.EQ;

            GlobalStats.AddEntry(ew, et, en, eq);
            imgClientServiceProbaSum += EventSimulator.QueueSystem.TimeStats.ImgClientServicedProbability;
        }

    }
}
