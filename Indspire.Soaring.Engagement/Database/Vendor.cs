// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Database
{
    using System;
    using System.ComponentModel;

    public class Vendor
    {
        [DisplayName("Vendor ID")]
        public int VendorID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Deleted")]
        public bool Deleted { get; set; } = false;

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}