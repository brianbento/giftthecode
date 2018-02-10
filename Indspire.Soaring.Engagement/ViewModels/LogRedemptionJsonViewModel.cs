// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class LogRedemptionJsonViewModel : JsonBaseViewModel
    {
        public LogRedemptionJsonViewModel()
        {
            ResponseData = new LogRedemptionResponseData();
        }

        public LogRedemptionResponseData ResponseData { get; set; }
    }
}
