namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class AwardLog
    {
        public int AwardLogID { get; set; }
        
        public int AwardID { get; set; }

        public Award Award { get; set; }
        
        public int Points { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
