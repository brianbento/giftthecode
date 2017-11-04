using Indspire.Soaring.Engagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Indspire.Soaring.Engagement.Utils;

namespace Indspire.Soaring.Engagement.Extensions
{
    public static class UserExtensions
    {
        public static string GetQRCodeAsBase64(this User user)
        {
            if(string.IsNullOrEmpty(user.UserNumber))
            {
                return null;
            }
            return QRCodeUtils.GenerateQRCodeAsBase64(user.UserNumber);
        }
    }
}
