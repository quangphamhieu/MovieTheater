using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; }
        public decimal Price { get; set; }

        public SeatDto() { }

        public SeatDto(MT.Domain.Seat seat)
        {
            Id = seat.Id;
            SeatCode = seat.SeatCode;
            SeatType = seat.SeatType;
            Price = seat.Price;
        }
    }
}
