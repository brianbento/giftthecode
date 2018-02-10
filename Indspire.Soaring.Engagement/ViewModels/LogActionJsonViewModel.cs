// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogActionJsonViewModel : JsonBaseViewModel
    {
        public LogActionJsonViewModel()
            : this(0, 0, string.Empty)
        {
        }

        public LogActionJsonViewModel(
            int pointsAwarded,
            int pointsBalance,
            string userNumber)
        {
            ResponseData = new LogActionResponseData
            {
                PointsAwarded = pointsAwarded,
                PointsBalance = pointsBalance,
                UserNumber = userNumber
            };
        }

        public LogActionResponseData ResponseData { get; set; }
    }
}
