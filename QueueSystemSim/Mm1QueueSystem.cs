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
        public Stats Stats { get; private set; } = new Stats();

        public virtual double ExpectedEQ { get => Math.Pow(QueueParamRo, 2.0) / (1 - QueueParamRo); }
        public virtual double ExpectedEN { get => QueueParamRo / (1 - QueueParamRo); }
        public virtual double ExpectedEW { get => QueueParamRo / (LeaveParamMi - ArrivalParamLambda); }
        public virtual double ExpectedET { get => 1 / (LeaveParamMi - ArrivalParamLambda); }

        public int nClientsInSystem
        {
            get => Clients.Where(c => c.ArrivalTime <= ActualTime ).Count();
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
            set
            {
                Clients[0] = value;
            }
        }

        public Mm1QueueSystem(double paramMi, double paramLambda)
        {
            LeaveParamMi = paramMi;
            ArrivalParamLambda = paramLambda;
        }

        public virtual void ServiceStep()
        {
            if (nClientsWaiting == 0)
                ActualTime = CurrentClient.ArrivalTime; // Skip time

            ServiceClient();
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        protected void ServiceClient() 
        {
            CurrentClient.ServiceStartTime = ActualTime;

            double serviceInterval = expDistribution.CreateNumber(LeaveParamMi);
            CurrentClient.LeaveTime = CurrentClient.ServiceStartTime + serviceInterval;

            ActualTime = CurrentClient.LeaveTime;

            Stats.AddEntry(CurrentClient.WaitForServiceTime, CurrentClient.SystemPassInterval, nClientsInSystem, nClientsWaiting);

            Clients.RemoveAt(0);
        }

        public void InitQueue(int nInitClients)
        {
            Clients = new List<Client>();
            double time = 0;
            for (int i = 0; i < nInitClients; i++)
            {
                time += expDistribution.CreateNumber(ArrivalParamLambda);
                var client = new Client { ArrivalTime = time };
                AddClient(client);
            }

            ActualTime = Clients[0].ArrivalTime;
        }
    }
}
