
using Indspire.Soaring.Engagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class AwardScanViewModel
    {
        public string AwardNumber { get; set; }
        public bool HasAwardNumber { get; set; }
        public bool AwardNumberInvalid { get; set; }
        public Award Award { get; set; }
    }
}
