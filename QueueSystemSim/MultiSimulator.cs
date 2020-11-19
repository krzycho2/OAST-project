using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    class MultiSimulator
    {
        public int nRuns { get; set; } = 100;

        public EventSimulator EventSimulator { get; private set; }

        public Stats GlobalStats { get; set; } = new Stats();

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
            double en = EventSimulator.QueueSystem.Stats.EN;
            double et = EventSimulator.QueueSystem.Stats.ET;
            double ew = EventSimulator.QueueSystem.Stats.EW;
            double eq = EventSimulator.QueueSystem.Stats.EQ;

            GlobalStats.AddEntry(ew, et, en, eq);


        }
    }
}
