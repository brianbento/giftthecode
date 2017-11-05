namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Redemption
    {
        public int RedemptionID { get; set; }

        [DisplayName("Redemption Number")]
        public string RedemptionNumber { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public IList<RedemptionLog> RedemptionLogs { get; set; }
    }
}