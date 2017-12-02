namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Award
    {
        public int AwardID { get; set; }

        public int VendorID { get; set; }

        [DisplayName("Award Number")]
        public string AwardNumber { get; set; }

        public int Points { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public IList<AwardLog> AwardLogs { get; set; }
    }
}