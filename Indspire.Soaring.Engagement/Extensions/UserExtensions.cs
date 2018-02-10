// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Extensions
{
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Utils;

    public static class UserExtensions
    {
        public static string GetQRCodeAsBase64(this Attendee user)
        {
            if (string.IsNullOrEmpty(user.UserNumber))
            {
                return null;
            }

            return QRCodeUtils.GenerateQRCodeAsBase64(user.UserNumber);
        }
    }
}
