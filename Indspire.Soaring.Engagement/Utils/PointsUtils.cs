// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Utils
{
    using System.Linq;
    using Indspire.Soaring.Engagement.Data;

    public static class PointsUtils
    {
        public static int GetPointsForUser(int userID, ApplicationDbContext context)
        {
            var points = context.AwardLog
                .Where(i => i.UserID == userID)
                .Sum(i => i.Points);

            return points;
        }
    }
}
