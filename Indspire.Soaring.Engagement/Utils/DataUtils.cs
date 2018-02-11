// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Utils
{
    using System;
    using System.Text;

    public static class DataUtils
    {
        public static string GenerateNumber()
        {
            var random = new Random();
            var sb = new StringBuilder();

            for (var i = 1; i < 6; i++)
            {
                sb.Append(random.Next(0, 9));
            }

            return sb.ToString();
        }
    }
}
