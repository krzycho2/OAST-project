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
    public class MultiSimulatorTests
    {
        [Test()]
        public void MultiSimulatorTest()
        {
            double[] roValues = new double[] { 0.25, 0.5, 0.75 };
            double paramLambda = 4;

            foreach (double ro in roValues)
            {
                double paramMi = paramLambda / ro;

                var queueSystem = new Mm1Continuous(paramMi, paramLambda);
                var simulator = new MultiSimulator(queueSystem);
                simulator.RunSim();
                Console.WriteLine($"Ro: {ro}, P: " + simulator.ImgClientServicedProbability);
            }
        }

  
    }
}