using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Domain
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUsage { get; set; }
        public ICollection<UserDiscount> UserDiscounts { get; set; }
        public ICollection<TicketDiscount> TicketDiscounts { get; set; }
    }
}
