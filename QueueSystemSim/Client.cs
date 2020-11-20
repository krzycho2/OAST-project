using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class Client
    {
        public QEvent Arrival { get; set; } 
        public QEvent ServiceStart { get; set; }
        public QEvent Leave { get; set; }
        public List<QEvent> AllEvents { get => new List<QEvent> { Arrival, ServiceStart, Leave }; }

        // getters
        public double WaitInterval { get => Math.Abs(ServiceStart.Time - Arrival.Time); }

        public double SystemPassInterval { get => Math.Abs(Leave.Time - Arrival.Time); }

        public bool ServiceStarted { get => ServiceStart.Time != -1; }

        public Client (double arrivalTime = -1, double serviceStartTime = -1, double leaveTime = -1)
        {
            Arrival = new QEvent { Type = QEventType.Arrival, Time = arrivalTime };
            ServiceStart = new QEvent { Type = QEventType.ServiceStart, Time = serviceStartTime };
            Leave = new QEvent { Type = QEventType.Leave, Time = leaveTime };
        }

        public void Test()
        {

        }
    }
}
