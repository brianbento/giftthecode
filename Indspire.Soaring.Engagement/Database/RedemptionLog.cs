using System;

namespace Indspire.Soaring.Engagement.Database
{

    public class RedemptionLog
    {
        public int RedemptionLogID { get; set; }

        public int RedemptionID { get; set; }

        public Redemption Redemption { get; set; }

        public int UserID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
