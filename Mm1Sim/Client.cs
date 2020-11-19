using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mm1Sim
{
    public class Client
    {
        public int ID { get; set; }
        public double ArrivalTime { get; set; }
        public double ServiceStartTime { get; set; }
        public double LeaveTime { get; set; }

        public double WaitInterval
        {
            get => Math.Abs(ServiceStartTime - ArrivalTime); 
        }

        public double ServiceInterval 
        {
            get => Math.Abs(LeaveTime - ServiceStartTime);
        }

        public double SystemPassInterval
        {
            get => WaitInterval + ServiceInterval;
        }
    }
}
