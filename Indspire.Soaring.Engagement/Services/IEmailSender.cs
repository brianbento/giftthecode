// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Services
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
