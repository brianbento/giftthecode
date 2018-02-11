// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
