// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    using System.Collections.Generic;

    public class AwardsRow
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AwardNumber { get; set; }

        public int TimesAwards { get; set; }
    }

    public class RedemptionsRow
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string RedemptionNumber { get; set; }

        public int TimesRedeemed { get; set; }
    }

    public class AttendeeRow
    {
        public string UserNamber { get; set; }

        public string ExternalId { get; set; }

        public int Points { get; set; }
    }

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
