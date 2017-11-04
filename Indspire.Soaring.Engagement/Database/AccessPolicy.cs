namespace Indspire.Soaring.Engagement.Database
{
    using System;

    public class AccessPolicy
    {
        public int AccessPolicyID { get; set; }

        public string AccessPolicyKey { get; set; }

        public string Domain { get; set; }

        public bool Deleted { get; set; } = false;
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }
    }
}








