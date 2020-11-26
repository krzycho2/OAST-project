using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class Mm1QueueSystem
    {
        protected readonly ExpDistribution expDistribution = new ExpDistribution();

        public double ActualTime { get; protected set; }

        public double LeaveParamMi { get; set; }
        public double ArrivalParamLambda { get; set; }
        public double QueueParamRo { get => ArrivalParamLambda / LeaveParamMi; }
        
        public List<Client> Clients { get; protected set; } = new List<Client>();
        public Stats TimeStats { get; private set; } = new Stats();
        public List<QEvent> LastEvents { get; private set; } = new List<QEvent>();

        public virtual double ExpectedEQ { get => Math.Pow(QueueParamRo, 2.0) / (1 - QueueParamRo); }
        public virtual double ExpectedEN { get => QueueParamRo / (1 - QueueParamRo); }
        public virtual double ExpectedEW { get => QueueParamRo / (LeaveParamMi - ArrivalParamLambda); }
        public virtual double ExpectedET { get => 1 / (LeaveParamMi - ArrivalParamLambda); }

        public int nClientsInSystem
        {
            get => Clients.Where(c => c.Arrival.Time <= ActualTime ).Count();
        }

        public int nClientsWaiting 
        {
            get {
                int all = nClientsInSystem;

                if (all > 1)
                    return nClientsInSystem - 1;

                else if (all == 1)
                {
                    if (CurrentClient.ServiceStarted)
                        return 0;

                    else
                        return 1;
                }
                else
                    return 0;

            } 
        }

        public Client CurrentClient 
        {
            get => Clients[0];
            set => Clients[0] = value;
        }

        public Mm1QueueSystem(double paramMi, double paramLambda)
        {
            LeaveParamMi = paramMi;
            ArrivalParamLambda = paramLambda;
        }

        public virtual void ServiceStep()
        {
            while (nClientsWaiting == 0)
            {
                ActualTime = CurrentClient.Arrival.Time; // Skip timebv 
            }

            double serviceInterval = CalculateServiceInterval();
            ServiceClient(serviceInterval);
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        protected void ServiceClient (double serviceInterval) 
        {
            CurrentClient.ServiceStart.Time = ActualTime;
            CurrentClient.Leave.Time = CurrentClient.ServiceStart.Time + serviceInterval;
            ActualTime = CurrentClient.Leave.Time;
            
            CollectResults();
            Clients.RemoveAt(0);
        }

        protected virtual double CalculateServiceInterval()
        {
            return expDistribution.CreateNumber(LeaveParamMi);
        }

        public void InitQueue(int nInitClients)
        {
            Clients = new List<Client>();
            double time = 0;
            for (int i = 0; i < nInitClients; i++)
            {
                time += expDistribution.CreateNumber(ArrivalParamLambda);
                AddClient( new Client(time) );
            }
            ActualTime = CurrentClient.Arrival.Time;
        }
        
        public void CollectResults()
        {
            LastEvents = CurrentClient.AllEvents;
            TimeStats.AddEntry(CurrentClient.WaitInterval, CurrentClient.SystemPassInterval);
        }


    }
}
