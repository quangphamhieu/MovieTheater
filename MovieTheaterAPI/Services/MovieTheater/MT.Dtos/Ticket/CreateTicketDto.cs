using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Ticket
{
    public class TicketCreateRequest
    {
        [Required]
        public int ShowTimeId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Cần ít nhất một ghế để đặt vé")]
        public List<int> SeatIds { get; set; }
    }
}
