namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class AwardLog
    {
        public int AwardLogID { get; set; }
        
        public int ActionID { get; set; }
        
        public int Points { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
