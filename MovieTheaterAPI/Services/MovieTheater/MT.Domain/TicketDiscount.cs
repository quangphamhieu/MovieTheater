using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Domain
{
    public class TicketDiscount
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int DiscountId { get; set; }
        public Ticket Ticket { get; set; }
        public Discount Discount { get; set; }
    }
}
