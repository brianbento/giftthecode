namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class Action
    {
        public int ActionID { get; set; }

        public int VendorID { get; set; }

        public string EventNumber { get; set; }

        public int Points { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}