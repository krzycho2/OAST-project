using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mm1Sim
{
    class Mm1Queue
    {
        const int ARRIVALS_COUNT = 100;

        public double QueueParamRo { get; private set; }
        public double ArrivalParamLambda { get; private set; }
        public double DepartureParamMi { get => ArrivalParamLambda / QueueParamRo; }

        double Time { get; set; } = 0.0;
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        private ExpDistribution ExpDistribution { get; set; } = new ExpDistribution();
        private List<Client> Clients { get; set; } = new List<Client>();

        public List<double> TimesOfArrival
        {
            get => Episodes 
                .Where(episode => episode.EpisodeType == EpisodeType.Arrival)
                .Select(episode => episode.Time).ToList();
        }

        public List<double> TimesOfDeparture
        { 
            get => Episodes
                .Where(episode => episode.EpisodeType == EpisodeType.Departure)
                .Select(episode => episode.Time).ToList();
        }

        public List<double> AllTimes
        {
            get => Episodes.Select(episode => episode.Time).ToList();
        }

        public List<int> ClientsCountInTime
        {
            get
            {
                var clientsCounts = new int[Episodes.Count].ToList();
                clientsCounts[0] = 1;
                for (int i = 1; i < Episodes.Count;  i++)
                {
                    var episode = Episodes[i];
                    if (episode.EpisodeType == EpisodeType.Arrival)
                        clientsCounts[i] = clientsCounts[i - 1] + 1;
                    
                    else
                        clientsCounts[i] = clientsCounts[i - 1] - 1;
                }
                return clientsCounts;
            }
        }

        public double MeanClientsCount
        {
            get => ClientsCountInTime.Average();
        }

        public List<double> ServiceTimes
        {
            get
            {
                //var times = new List<double>();
                //var IDs = Episodes.Select(episode => episode.ClientID).Distinct();
                //foreach(int id in IDs)
                //{
                //    var pair = Episodes.FindAll(episode => episode.ClientID == id);
                //    double serviceTime = Math.Abs(pair[1].Time - pair[0].Time);
                //    times.Add(serviceTime);
                //}
                //return times;
                return Clients.Select(client => client.ServiceInterval).ToList();
            }
        }

        public double MeanServiceTime
        {
            get => ServiceTimes.Average();
        }

        public Mm1Queue(double queueParamRo, double arrivalParamLambda = 1)
        {
            QueueParamRo = queueParamRo;
            ArrivalParamLambda = arrivalParamLambda;
        }

        public void GenerateEpisodes()
        {
            foreach (var arrival in GenerateArrivals())
                Episodes.Add(arrival);

            foreach (var departure in GenerateDepartures())
                Episodes.Add(departure);

            Episodes = Episodes.OrderBy(episode => episode.Time).ToList();
        }

        public List<Episode> GenerateArrivals()
        {
            var arrivals = new List<Episode>();
            double arrivalTime = Time;
            for (int i = 0; i < ARRIVALS_COUNT; i++)
            {
                arrivals.Add(new Episode { EpisodeType = EpisodeType.Arrival, Time = arrivalTime, ClientID = i });
                arrivalTime += ExpDistribution.CreateNumber(ArrivalParamLambda);
                Clients.Add(new Client { ArrivalTime = arrivalTime, ID = i });
            }

            return arrivals;
        }

        public List<Episode> GenerateDepartures()
        {
            var departures = new List<Episode>();
            double serviceStartTime = TimesOfArrival[0];
            for (int i = 0; i < TimesOfArrival.Count; i++)
            {
                double leaveTime = serviceStartTime + ExpDistribution.CreateNumber(DepartureParamMi);
                departures.Add(new Episode { EpisodeType = EpisodeType.Departure, Time = leaveTime, ClientID = i });

                Clients.Find(client => client.ID == i).ServiceStartTime = serviceStartTime;
                Clients.Find(client => client.ID == i).LeaveTime = leaveTime;

                serviceStartTime = leaveTime;
            }

            return departures;
        }

    }

}
