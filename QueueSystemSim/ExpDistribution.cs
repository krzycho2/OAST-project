using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QueueSystemSim
{
    public class ExpDistribution
    {
        private readonly RNGCryptoServiceProvider RNG = new RNGCryptoServiceProvider();

        public double CreateNumber(double param)
        {
            double value;
            do
            {
                value = -Math.Log(RandomDouble) / param;
            }
            while (double.IsInfinity(value) || double.IsNaN(value));


            return value;
        }

        public double RandomDouble
        {
            get
            {
                byte[] random = new byte[1];
                RNG.GetBytes(random);
                return random[0] / 255.0; // normalization to 1
            }
        }

        ~ExpDistribution()
        {
            RNG.Dispose();
        }
    }
}
