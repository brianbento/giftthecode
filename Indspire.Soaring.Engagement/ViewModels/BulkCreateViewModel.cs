// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class BulkCreateViewModel
    {
        [DisplayName("Attendees Created")]
        public int AmountCreated { get; set; } = 0;

        [Required]
        [DisplayName("Attendees to Create")]
        [Range(1, 2000, ErrorMessage = "Amount to create must be greater than 0 and less than 2000.")]
        public int Amount { get; set; } = 0;
    }
}
