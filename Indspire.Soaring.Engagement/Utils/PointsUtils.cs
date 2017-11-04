using Indspire.Soaring.Engagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Utils
{
    public class PointsUtils
    {
        public static int GetPointsForUser(int userID, ApplicationDbContext context)
        {
            int points = context.AwardLog.Where(i => i.UserID == userID).Sum(i => i.Points);

            return points;
        }
    }
}
