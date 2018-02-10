// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models.AttendeeViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateAttendeeViewModel
    {
        [Required]
        [DisplayName("External ID")]
        public string ExternalID { get; set; } =
            string.Empty;
    }
}
