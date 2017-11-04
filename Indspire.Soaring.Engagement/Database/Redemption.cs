namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class Redemption
    {
        public int RedemptionID { get; set; }

        public string RedemptionNumber { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool Deleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}