﻿namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class Instance
    {
        public int InstanceID { get; set; }

        [DisplayName("Event Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public bool DefaultInstance { get; set; }

        [DisplayName("Awards")]
        public IList<Award> Awards { get; set; }

        [DisplayName("Attendees")]
        public IList<Attendee> Attendees { get; set; }

        [DisplayName("Redemptions")]
        public IList<Redemption> Redemptions { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
