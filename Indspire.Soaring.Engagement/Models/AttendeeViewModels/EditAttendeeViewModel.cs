// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models.AttendeeViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditAttendeeViewModel
    {
        [Required]
        [DisplayName("External ID")]
        public string ExternalID { get; set; }

        [DisplayName("User ID")]
        public int UserID { get; set; }
    }
}
