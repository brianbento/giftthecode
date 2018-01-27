using Indspire.Soaring.Engagement.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class AttendeeLabel
    {
        public string UserNumber { get; set; }

        public AttendeeLabel() { }

        public AttendeeLabel(Attendee attendee) 
        {
            UserNumber = attendee.UserNumber;
        }
    }
}
