// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class CheckBalanceJsonViewModel : JsonBaseViewModel
    {
        public CheckBalanceJsonViewModel()
            : this(0, string.Empty)
        {
        }

        public CheckBalanceJsonViewModel(int pointsBalance, string userNumber)
        {
            this.ResponseData = new CheckBalanceResponseData
            {
                PointsBalance = pointsBalance,
                UserNumber = userNumber
            };
        }

        public CheckBalanceResponseData ResponseData { get; set; }
    }
}
