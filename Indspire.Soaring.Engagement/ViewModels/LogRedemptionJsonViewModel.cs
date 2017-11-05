using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogRedemptionJsonViewModel : JsonBaseViewModel
    {
        public LogRedemptionJsonViewModel()
        {
            ResponseData = new LogRedemptionResponseData();
        }
        public LogRedemptionResponseData ResponseData = new LogRedemptionResponseData();
    }

    public class LogRedemptionResponseData
    {
        public bool Success { get; set; }
        public int PointsShort { get; set; }
        public int PointsBalance { get; set; }
        public string UserNumber { get; set; }
    }
}
