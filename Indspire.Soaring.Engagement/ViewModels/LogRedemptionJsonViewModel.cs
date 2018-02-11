// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogRedemptionJsonViewModel : JsonBaseViewModel
    {
        public LogRedemptionJsonViewModel()
        {
            this.ResponseData = new LogRedemptionResponseData();
        }

        public LogRedemptionResponseData ResponseData { get; set; }
    }
}
