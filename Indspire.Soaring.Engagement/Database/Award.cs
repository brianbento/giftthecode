namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class Award
    {
        public int AwardID { get; set; }

        public int VendorID { get; set; }

        public string EventNumber { get; set; }

        public int Points { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}