using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class Client
    {
        public double ArrivalTime { get; set; }
        public double ServiceStartTime { get; set; } = -1;
        public double LeaveTime { get; set; }

        // getters
        public double WaitForServiceTime { get => Math.Abs(ServiceStartTime - ArrivalTime); }

        public double SystemPassInterval { get => Math.Abs(LeaveTime - ArrivalTime); }

        public bool ServiceStarted { get => ServiceStartTime != -1; }

    }
}
