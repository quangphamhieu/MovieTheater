using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Domain
{
    public class UserDiscount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DiscountId { get; set; }
        public bool Used { get; set; }
        public DateTime UsageDate { get; set; }
        public User User { get; set; }
        public Discount Discount { get; set; }
    }
}
