using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    class Mm1ContinuousService: Mm1QueueSystem
    {
        public Mm1ContinuousService(double paramMi, double paramLambda): base(paramMi, paramLambda) { }

        public override double ExpectedEQ { get => QueueParamRo / (1 - QueueParamRo); }
        public override double ExpectedEN { get => (2 - QueueParamRo) * QueueParamRo / (1 - QueueParamRo); }
        public override double ExpectedEW { get => QueueParamRo / ArrivalParamLambda / (1 - QueueParamRo); }
        public override double ExpectedET { get => (2 - QueueParamRo) * QueueParamRo / ArrivalParamLambda / (1 - QueueParamRo); }

        public override void ServiceStep()
        {
            while (nClientsWaiting == 0)
                ServiceImaginaryClient();

            ServiceClient();  
        }

        private void ServiceImaginaryClient() 
        {
            var imClient = new Client { ArrivalTime = ActualTime, ServiceStartTime = ActualTime };
            double serviceInterval = expDistribution.CreateNumber(LeaveParamMi);
            imClient.LeaveTime = imClient.ServiceStartTime + serviceInterval; // Unnessary, just to show logic

            ActualTime = imClient.LeaveTime;
        } 
    }
}
