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

        [Test()]
        public void ChartTest()
        {

            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;
            double ro = 0.25;

            double paramMi = paramLambda / ro;

            var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);
            var simulator = new EventSimulator(queueSystem);
            simulator.RunSim();

            var chartMaker = new ChartMaker();
            chartMaker.SetData(simulator.P0Points);
            chartMaker.Show();

        }

        [Test()]
        public void WriteToCSVTest()
        {
            double paramLambda = 4;
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            string output = "";
            foreach (double ro in roValues)
            {
                double paramMi = paramLambda / ro;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);
                var simulator = new EventSimulator(queueSystem);
                simulator.RunSim();

                output += $"Wartości_dla_Ro_{ro}. Oczekiwane_p0_{1 - ro}\n";
                output += string.Join("\n", simulator.P0Points.Select(point => $"{point.X} {point.Y}"));
                output += "\n\n";
            }


            
            string path = @"C:\Users\Krzysztof Krupiński\Desktop\data.txt";
            using (var file = new System.IO.StreamWriter(path))
            {
                file.Write(output);
            }


            Console.WriteLine(output);
        }


    }
}