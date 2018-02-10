// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    using Indspire.Soaring.Engagement.Database;

    public class RedemptionScanViewModel
    {
        public string RedemptionNumber { get; set; }

        public bool HasRedemptionNumber { get; set; }

        public bool RedemptionNumberInvalid { get; set; }

        public Redemption Redemption { get; set; }
    }
}
