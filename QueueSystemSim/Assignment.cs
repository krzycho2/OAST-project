using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class Assignment
    {
        public string AllOutput { get; private set; } = "";

        public void Run()
        {
            Task1();
            Task2();
        }

        /// <summary>
        /// 1. Symulacja kolejki M/M/1
        /// </summary>
        public void Task1()
        {
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;

            var paramET = new QOutputParam { Name = "ET" };

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);

                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();

                paramET.Values.Add(new QOutputParamValue { Ro = paramRo, Expected = queueSystem.ExpectedET, Value = simulator.GlobalStats.ET });  
            }

            string output = Task1Intro;
            output += PrettyPrint2(new List<QOutputParam> { paramET });

            Console.Write(output);
            AllOutput += output;

            // draw 3 graphs
        }


        public void Task2()
        {
            var ET = new QOutputParam { Name = "ET" };
            var EW = new QOutputParam { Name = "EW" };
            var EN = new QOutputParam { Name = "EN" };
            var EQ = new QOutputParam { Name = "EQ" };
            var outputParams = new List<QOutputParam> { ET, EW, EN, EQ };

            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;

            for (int i = 0; i < roValues.Length; i++)
            {
                double paramRo = roValues[i];
                double paramMi = paramLambda / paramRo;

                var queueSystem = new Mm1QueueSystem(paramMi, paramLambda);

                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();

                foreach (var paramName in new string[] { "ET", "EW", "EN", "EQ" })
                {
                    outputParams.Find(param => param.Name == paramName).
                    Values.Add(new QOutputParamValue { Ro = paramRo, Expected = queueSystem.ExpectedET, Value = simulator.GlobalStats.ET });
                }
            }

            string output = Task2Intro;
            output += PrettyPrint2(outputParams);
            Console.Write(output);
            AllOutput += output;
        }

        public void Task3()
        {

        }

        private string PrettyPrint2(List<QOutputParam> outputParams)
        {
            double[] roValues = outputParams[0].RoValues;
            string output = "";
            output += ("\t\t\t\t\t\t   Ro\n");
            output += ($"\t\t\t\t| {roValues[0]:0.00}\t | {roValues[1]:0.00}\t  | {roValues[2]:0.00}   |\n");
            output += ("\t\t\t\t----------------------------\n");

            foreach (var oParam in outputParams)
            {
                var expecteds = oParam.Values.Select((param) => param.sExpected).ToList();
                var values = oParam.Values.Select((param) => param.sValue).ToList();

                string expectedString = "\tOczekiwane\t|";
                string valuesString = "\tUzyskane\t|";

                for (int i = 0; i < roValues.Length; i++)
                {
                    expectedString += $" {expecteds[i]} |";
                    valuesString += $" {values[i]} |";
                }

                expectedString += $"\n";
                valuesString += $"\n";

                output += ($"{oParam.Name}\t\t\t\t\n");
                output += expectedString;
                output += valuesString;
                output += ("\t\t\t\t----------------------------\n");
            }

            return output;
        }

        

        /// <summary>
        /// 2.3 Dla kolejki M/M/1 o pracy ciągłej, oszacować prawdopodobieństwo, że serwer jest zajęty obsługą klienta wyimaginowanego.
        /// </summary>
        public void Task2_3()
        {
            Console.WriteLine("2.3");
        }

        public void WriteOutputToFile()
        {
            Console.WriteLine("Zapis do pliku");
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
                output += "1. Symulacja kolejki M/M/1\n";
                output += "a) porównanie E[T] z wynikami teoretycznymi dla Ro = [0.25, 0.5, 0,75],\n";
                output += "b) narysowanie wykresu zbieżności prawdopodobieństwa p0(t) do wartości p0 z rozkładu stacjonarnego.\n\n";
                return output;
            }
        }
    }
}
