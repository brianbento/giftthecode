namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class User
    {
        public int UserID { get; set; }

        [DisplayName("User Number")]
        public string UserNumber { get; set; }

        public string ExternalID { get; set; }

        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public IList<RedemptionLog> RedemptionLogs { get; set; }

        public IList<AwardLog> AwardLogs { get; set; }
    }
}
