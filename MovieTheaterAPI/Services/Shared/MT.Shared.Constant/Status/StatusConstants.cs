using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Shared.Constant.Status
{
    public static class StatusConstants
    {
        public static class Ticket
        {
            public const string Active = "Còn hạn";
            public const string Expired = "Hết hạn";
            public const string Booked = "Booked";
            public const string Available = "Available";
        }

        public static class Discount
        {
            public const string Active = "Active";
            public const string Expired = "Expired";
            public const string Used = "Used";
        }
    }
}
