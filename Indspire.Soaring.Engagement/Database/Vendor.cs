namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class Vendor
    {
        public int VendorID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}