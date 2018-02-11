// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Models
{
    using Indspire.Soaring.Engagement.Database;

    public class AttendeeLabel
    {
        public AttendeeLabel()
            : this(null)
        {
        }

        public AttendeeLabel(Attendee attendee)
        {
            this.UserNumber = attendee?.UserNumber;
        }

        public string UserNumber { get; set; }
    }
}
