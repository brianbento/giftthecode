namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class User
    {
        public int UserID { get; set; }

        public string UserNumber { get; set; }

        public string ExternalID { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
