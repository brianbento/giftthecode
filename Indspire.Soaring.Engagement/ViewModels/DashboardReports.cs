// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    using System.Collections.Generic;

    public class DashboardReports
    {
        public IList<AwardsRow> AwardsList { get; set; } =
            new List<AwardsRow>();

        public List<AttendeeRow> AttendeeList { get; set; } =
            new List<AttendeeRow>();

        public List<RedemptionsRow> RedemptionsList { get; set; } =
            new List<RedemptionsRow>();
    }
}
