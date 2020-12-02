using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QueueSystemSim
{
    public class EventSimulator
    {
        private ExpDistribution expDistribution = new ExpDistribution();

        public EventPool EventPool { get; private set; } = new EventPool();
        public Mm1QueueSystem QueueSystem { get; private set; }

        public int nWarmupSteps { get; set; } = 500;
        public int nInitClients { get; set; } = 5;
        public int nInitEpisodes { get; set; } = 2;
        public int nSteps { get; set; }
        public double SimStopByET { get => 1e-7 * QueueSystem.LeaveParamMi; }

        public List<Point> P0Points { get; set; } = new List<Point>();

        public double EN { get => CalculateEN(); }
        public double EQ { get => CalculateEQ(); }
        public double ET { get => QueueSystem.TimeStats.ET; }
        public double EW { get => QueueSystem.TimeStats.EW; }

        public EventSimulator (Mm1QueueSystem queueSystem) // Dependency Injection
        {
            QueueSystem = queueSystem;
        }

        public void RunSim() 
        {
            Init();
            Warmup();
            ActualSim();
            
        }

        public void Init() 
        {
            QueueSystem.InitQueue(nInitClients);
            InitEpisodes();
            P0Points = new List<Point>();
        }

        public void Warmup() 
        {
            for (int i = 0; i < nWarmupSteps; i++)
                SimStep();

            QueueSystem.TimeStats.Reset();
        }

        public void ActualSim()
        {
            nSteps = 0;
            double deltaET = 1000; // difference in parameters between steps
            double lastET = 1000;
            while (deltaET > SimStopByET)
            {
                SimStep();
                nSteps++;
                deltaET = Math.Abs(QueueSystem.TimeStats.ET - lastET);
                lastET = QueueSystem.TimeStats.ET;
            }
        }

        public void SimStep() 
        {
            // GET
            AddEventToQueue();

            // EXECUTE
            QueueSystem.ServiceStep();
            CollectResults();

            // PUT
            PutArrivalEpisodeOnPool();
        }

        private void CollectResults()
        {
            EventPool.AddOldEvents(QueueSystem.LastEvents);
            P0Points.Add(new Point { X = QueueSystem.ActualTime, Y = QueueSystem.NoClientsInterval / QueueSystem.ActualTime });
        }

        private void AddEventToQueue()
        {
            var episode = EventPool.GET();
            if (episode.Type == QEventType.Arrival)
            {
                var client = new Client { Arrival = episode };
                QueueSystem.AddClient(client);
            }
        }

        private void PutArrivalEpisodeOnPool()
        {
            var queueLastArrival = QueueSystem.Clients.Last().Arrival.Time;
            double newArrivalTime = queueLastArrival + expDistribution.CreateNumber(QueueSystem.ArrivalParamLambda);

            EventPool.PUT(QEventType.Arrival, newArrivalTime);
        }

        public void InitEpisodes()
        {
            EventPool = new EventPool();
            PutArrivalEpisodeOnPool();
        }

        private double CalculateEN()
        {
            return CountClients(QEventType.Leave);
        }

        private double CalculateEQ()
        {
            return CountClients(QEventType.ServiceStart);
        }

        private double CountClients(QEventType type)
        {
            var events = EventPool.OldEvents.
                Where((qEvent) => qEvent.Type == QEventType.Arrival || qEvent.Type == type). // Filter Arrival and given events
                OrderBy((qEvent) => qEvent.Time). // Sort by time
                ToList();

            double interval = events.Last().Time - events.First().Time;
            double sum = 0;
            int tempNClients = 0;
            for (int i = 0; i < events.Count() - 1; i++)
            {
                var qEvent = events[i];
                if (qEvent.Type == QEventType.Arrival)
                    tempNClients++;

                else
                    tempNClients--;

                double intervalBetweenEvents = events[i + 1].Time - events[i].Time;
                sum += (tempNClients * intervalBetweenEvents);
            }

            return sum / interval;
        }

    }
}
