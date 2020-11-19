using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class EventSimulator
    {
        private ExpDistribution expDistribution = new ExpDistribution();

        public EpisodePool EpisodePool { get; private set; } = new EpisodePool();
        public Mm1QueueSystem QueueSystem { get; private set; }

        public int nSteps { get; private set; }

        public int nWarmupSteps { get; set; } = 500;
        public int nInitClients { get; set; } = 5;
        public int nInitEpisodes { get; set; } = 2;

        public double SimStopByET { get => 1e-7 * QueueSystem.LeaveParamMi; }

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
        }

        public void Warmup() 
        {
            for (int i = 0; i < nWarmupSteps; i++)
                SimStep();

            QueueSystem.Stats.Reset();
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
                deltaET = Math.Abs(QueueSystem.Stats.ET - lastET);
                lastET = QueueSystem.Stats.ET;
            }
        }

        public void SimStep() 
        {
            // GET
            AddEpisodeToQueue();

            // EXECUTE
            QueueSystem.ServiceStep();

            // PUT
            PutArrivalEpisodeOnPool();
        }

        private void AddEpisodeToQueue()
        {
            var episode = EpisodePool.GET();
            if (episode.Type == EpisodeType.Arrival)
            {
                var client = new Client { ArrivalTime = episode.Time };
                QueueSystem.AddClient(client);
            }
        }

        private void PutArrivalEpisodeOnPool()
        {
            var queueLastArrival = QueueSystem.Clients.Last().ArrivalTime;
            double newArrivalTime = queueLastArrival + expDistribution.CreateNumber(QueueSystem.ArrivalParamLambda);

            EpisodePool.PUT(EpisodeType.Arrival, newArrivalTime);
        }

        public void InitEpisodes()
        {
            EpisodePool = new EpisodePool();
            PutArrivalEpisodeOnPool();
        }

        public void Reset()
        {

        }

    }
}
