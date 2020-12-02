using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QueueSystemSim
{
    public class Assignment
    {
        public string AllOutput { get; private set; } = "";

        public void Run()
        {
            Task1();
            //Task2();
            //Task3();
        }

        /// <summary>
        /// 1. Symulacja kolejki M/M/1
        /// </summary>
        public void Task1()
        {
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;
            var outputParams = new QOutputParams();
            var p0Points = new List< List<Point> >();
            var expectedP0Values = new List<double>();

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);

                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();

                outputParams.AddEntry(simulator.OutputParams);
                p0Points.Add(simulator.EventSimulator.P0Points);
                expectedP0Values.Add(simulator.EventSimulator.QueueSystem.ExpectedP0);
            }

            string output = Task1Intro;
            output += PrettyPrint(outputParams);

            SaveP0Points(p0Points);
            output += "\n\nPunkty wykresów zbieżności P0 zapisano w pliku 'p0data.txt' na Pulpicie";

            Console.Write(output);
            AllOutput += output;

        }

        public void SaveP0Points(List<List<Point>> pointsPerRo)
        {
            double paramLambda = 4;
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            string output = "";
            for (int i = 0; i < pointsPerRo.Count; i++)
            {
                var points = pointsPerRo[i];
                double ro = roValues[i];
                double paramMi = paramLambda / ro;

                output += $"Wartości_dla_Ro_{ro}. Oczekiwane_p0_{1 - ro}\n";
                output += string.Join("\n", points.Select(point => $"{point.X} {point.Y}"));
                output += "\n\n";
            }


            string path = $@"{ Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\p0data.txt";
            using (var file = new System.IO.StreamWriter(path))
            {
                file.Write(output);
            }

        }

        public void Task2()
        {

            var outputParams = new QOutputParams();

            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;
            double[] imgClientsProbas = new double[3];

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;
                var queueSystem = new Mm1Continuous(paramMi, paramLambda);
                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();

                outputParams.AddEntry(simulator.OutputParams);
                imgClientsProbas[i] = simulator.ImgClientServicedProbability;
            }

            string output = Task2Intro;
            output += PrettyPrint(outputParams);
            output += PrettyPrintImgClientProbability(roValues, imgClientsProbas);
            Console.Write(output);
            AllOutput += output;
        }

        public void Task3()
        {
            var outputParams = new QOutputParams();
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;

            double[] imgClientsProbas = new double[3];

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1Continuous(paramMi, paramLambda);
                queueSystem.IsFixedServiceInterval = true;
                var simulator = new MultiSimulator(queueSystem);

                simulator.RunSim();

                outputParams.AddEntry(simulator.OutputParams);
                imgClientsProbas[i] = simulator.ImgClientServicedProbability;
            }

            string output = Task3Intro;
            output += PrettyPrint(outputParams);
            output += PrettyPrintImgClientProbability(roValues, imgClientsProbas);
            Console.Write(output);
            AllOutput += output;
        }

        private string PrettyPrint(QOutputParams outputParams)
        {
            double[] roValues = outputParams.RoValues;
            string output = "";
            output += ("\t\t\t\t\t   Ro\n");
            output += ($"\t\t\t| {roValues[0]:0.00}\t | {roValues[1]:0.00}\t  | {roValues[2]:0.00}   |\n");
            output += ("\t\t\t----------------------------\n");

            foreach (var oParam in outputParams.AllParams)
            {
                var expecteds = oParam.Values.Select((param) => param.sExpected).ToList();
                var values = oParam.Values.Select((param) => param.sValue).ToList();
                var errors = oParam.Values.Select((param) => param.sError).ToList();

                string expectedString = "\tOczekiwane\t|";
                string valuesString = "\tUzyskane\t|";
                string errorsString  = "\tBład wzg.\t|";

                for (int i = 0; i < roValues.Length; i++)
                {
                    expectedString += $" {expecteds[i]} |";
                    valuesString += $" {values[i]} |";
                    errorsString += $" {errors[i]}   |";
                }

                expectedString += $"\n";
                valuesString += $"\n";
                errorsString += $"\n";

                output += ($"{oParam.Name}\t\t\t\t\n");
                output += expectedString;
                output += valuesString;
                output += errorsString;
                output += ("\t\t\t----------------------------\n");
            }

            return output;
        }

        private string PrettyPrintImgClientProbability(double[] roValues, double[] probas)
        {
            string output = "\n";
            output += "Prawdopodobieństwo, że serwer jest zajęty obsługą klienta wyimaginowanego:\n\n";
            output += ($"Ro\t| {roValues[0]:0.00}  | {roValues[1]:0.00}  | {roValues[2]:0.00}  |\n");
            output += ("\t----------------------------\n");

            output += $"P\t| {probas[0]:0.0%}\t| {probas[1]:0.0%}\t| {probas[2]:0.0%}\t|\n";

            return output;
        }

        

        public string Task1Intro
        {
            get
            {
                string output = "\n";
                output += "1. Symulacja kolejki M/M/1\n";
                output += "a) porównanie E[T] z wynikami teoretycznymi dla Ro = [0.25, 0.5, 0,75],\n";
                output += "b) narysowanie wykresu zbieżności prawdopodobieństwa p0(t) do wartości p0 z rozkładu stacjonarnego.\n\n";
                return output;
            }
        }

        public string Task2Intro
        {
            get
            {
                string output = "\n";
                output += "2. Symulacja kolejki M/M/1 o pracy ciągłej i wykładniczym rozkładzie czasu obsługi.\n";
                output += "a) porównanie E[Q], E[T], E[W], E[N] z wynikami teoretycznymi,\n";
                output += "b) oszacowanie prawdopodobieństwa, że serwer zajęty obsługą klienta wyimaginowanego\n";
                output += "dla Ro = [0.25, 0.5, 0,75].\n\n"; 
                return output;
            }
        }

        public string Task3Intro
        {
            get
            {
                string output = "\n";
                output += "3. Symulacja kolejki M/M/1 o pracy ciągłej i stałym czasie obsługi.\n";
                return output;
            }
        }
    }
}
