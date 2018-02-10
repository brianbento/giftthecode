// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models
{
    using Indspire.Soaring.Engagement.Database;

    public class TopAward
    {
        public int AwardID { get; set; }

        public Award Award { get; set; }

        public int TotalPoints { get; set; }
    }
}
