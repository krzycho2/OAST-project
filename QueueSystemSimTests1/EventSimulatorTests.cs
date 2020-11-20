using NUnit.Framework;
using QueueSystemSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim.Tests
{
    [TestFixture()]
    public class EventSimulatorTests
    {
        [Test()]
        public void EventSimulatorTest_Get_EN()
        {
            var events = new List<QEvent>
            {
                new QEvent {Type = QEventType.Arrival, Time = 1},
                new QEvent {Type = QEventType.Arrival, Time = 2},
                new QEvent {Type = QEventType.Leave, Time = 3},
                new QEvent {Type = QEventType.Arrival, Time = 4},
                new QEvent {Type = QEventType.Arrival, Time = 5},
                new QEvent {Type = QEventType.Leave, Time = 6},
                new QEvent {Type = QEventType.Arrival, Time = 7}
            };

            var simulator = new EventSimulator(new Mm1QueueSystem(1, 1));
            simulator.EventPool.AddOldEvents(events);

            double wanted = 11.0 / 6.0;
            Assert.AreEqual(wanted, simulator.EN);
        }


    }
}