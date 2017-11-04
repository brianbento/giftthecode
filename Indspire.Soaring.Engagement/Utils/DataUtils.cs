using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Utils
{
    public class DataUtils
    {
  
        public string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;

            for (i = 1; i < 6; i++)
            {
                r += random.Next(0, 9).ToString();
            }

            return r;
        }
    }
}
