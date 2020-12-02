using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class Stats
    {
        private double waitForServiceIntervalSum = 0.0;
        private double servicePassIntervalSum = 0.0;
        private double imgClientIntervalSum = 0.0;
        private double nClientsWaitingSum = 0;
        private double nClientsInSystemSum = 0;
        
        public int nMeasurements { get; private set; } = 0;
        public double FullServiceTime { get; set; } = 0.0;

        // Średnia liczba klientów w systemie
        public double EN
        {
            get => nClientsInSystemSum / nMeasurements;
        }

        // Średni czas przejścia przez system
        public double ET
        {
            get => servicePassIntervalSum / nMeasurements;
        }

        // Średni czas oczekiwania na obsługę
        public double EW
        {
            get => waitForServiceIntervalSum / nMeasurements;
        }

        // Średnia liczba klientów oczekujących
        public double EQ
        {
            get => nClientsWaitingSum / nMeasurements;
        }

        public double ImgClientServicedProbability { get => imgClientIntervalSum / FullServiceTime; }

        public void AddEntry(double waitInterval=0, double systemPassInterval=0, double nClientsInSystem=0, double nClientsWaiting=0)
        { 
            servicePassIntervalSum += systemPassInterval;
            waitForServiceIntervalSum += waitInterval;

            nClientsWaitingSum += nClientsWaiting;
            nClientsInSystemSum += nClientsInSystem;

            nMeasurements++;
        }

        public void AddImgClientService(double actualTime, double imgClientInterval)
        {
            FullServiceTime = actualTime;
            imgClientIntervalSum += imgClientInterval;
        }

        public void Reset()
        {
            waitForServiceIntervalSum = 0.0;
            nClientsInSystemSum = 0.0;
            servicePassIntervalSum = 0.0;
            nClientsWaitingSum = 0.0;
            imgClientIntervalSum = 0.0;

            nMeasurements = 0;
            FullServiceTime = 0;
        }

    }
}
