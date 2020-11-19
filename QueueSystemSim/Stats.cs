﻿using System;
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
        private double nClientsWaitingSum = 0;
        private double nClientsInSystemSum = 0;


        public int nMeasurements { get; private set; } = 0;

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

        public void AddEntry(double waitInterval, double systemPassInterval, double nClientsInSystem, double nClientsWaiting)
        { 
            nClientsInSystemSum += nClientsInSystem;
            servicePassIntervalSum += systemPassInterval;
            waitForServiceIntervalSum += waitInterval;
            nClientsWaitingSum += nClientsWaiting;

            nMeasurements++;
        }

        public void Reset()
        {
            waitForServiceIntervalSum = 0.0;
            nClientsInSystemSum = 0.0;
            servicePassIntervalSum = 0.0;
            nClientsWaitingSum = 0.0;

            nMeasurements = 0;
        }

    }
}