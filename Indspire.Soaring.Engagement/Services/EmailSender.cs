// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Services
{
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
