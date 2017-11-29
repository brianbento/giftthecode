namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Award
    {
        [DisplayName("Award ID")]
        public int AwardID { get; set; }

        [DisplayName("Vendor ID")]
        public int VendorID { get; set; }

        [DisplayName("Award Number")]
        public string EventNumber { get; set; }

        [DisplayName("Award Number")]
        public int Points { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Award Logs")]
        public IList<AwardLog> AwardLogs { get; set; }
    }
}