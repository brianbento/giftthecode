
using Indspire.Soaring.Engagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class RedemptionScanViewModel
    {
        public string RedemptionNumber { get; set; }
        public bool HasRedemptionNumber { get; set; }
        public bool RedemptionNumberInvalid { get; set; }
        public Redemption Redemption { get; set; }
    }
}
