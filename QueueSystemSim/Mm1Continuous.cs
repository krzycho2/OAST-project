using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    class Mm1Continuous : Mm1QueueSystem
    {
        public bool IsFixedServiceInterval { get; set; } = false;
        // Expected values
        public override double ExpectedEQ { get => QueueParamRo / (1 - QueueParamRo); }
        public override double ExpectedEN { get => (2 - QueueParamRo) * QueueParamRo / (1 - QueueParamRo); }
        public override double ExpectedEW { get => QueueParamRo / ArrivalParamLambda / (1 - QueueParamRo); }
        public override double ExpectedET { get => (2 - QueueParamRo) * QueueParamRo / ArrivalParamLambda / (1 - QueueParamRo); }

        public Mm1Continuous(double paramMi, double paramLambda) : base(paramMi, paramLambda) { }

        public override void ServiceStep()
        {
            while (nClientsWaiting == 0)
            {
                ServiceImaginaryClient();
            }

            double serviceInterval = CalculateServiceInterval();
            ServiceClient(serviceInterval);
        }

        protected override double CalculateServiceInterval()
        {
            if (IsFixedServiceInterval)
                return 1 / LeaveParamMi;

            else
                return expDistribution.CreateNumber(LeaveParamMi);
        }


        private void ServiceImaginaryClient()
        {
            var imClient = new Client(ActualTime, ActualTime);
            double serviceInterval = expDistribution.CreateNumber(LeaveParamMi);
            imClient.Leave.Time = imClient.ServiceStart.Time + serviceInterval; // Unnessary, just to show logic

            ActualTime = imClient.Leave.Time;
        }
    }
}
