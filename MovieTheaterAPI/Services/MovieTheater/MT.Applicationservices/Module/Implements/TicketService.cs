using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Seat;
using MT.Dtos.Ticket;
using MT.Infrastructure;
using MT.Shared.Constant.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class TicketService : MovieTheaterBaseService, ITicketService

    {
        private readonly IDiscountService _discountService;
        public TicketService(ILogger<TicketService> logger, MovieTheaterDbContext dbContext, IDiscountService discountService)
            : base(logger, dbContext)
        {
            _discountService = discountService;
        }

        public async Task<TicketDto> CreateTicketAsync(TicketCreateRequest ticketCreateRequest, int userId)
        {
            _logger.LogInformation($"Creating ticket for ShowTime {ticketCreateRequest.ShowTimeId} by User {userId}.");


            // Kiểm tra ShowTime
            var showTime = await _dbContext.ShowTimes
                .Include(st => st.Movie)
                .Include(st => st.CinemaRoom.Cinema)
                .FirstOrDefaultAsync(st => st.Id == ticketCreateRequest.ShowTimeId);

            if (showTime == null)
            {
                _logger.LogWarning($"ShowTime {ticketCreateRequest.ShowTimeId} not found.");
                throw new Exception("ShowTime not found");
            }

            // Kiểm tra danh sách ghế
            if (ticketCreateRequest.SeatIds == null || !ticketCreateRequest.SeatIds.Any())
            {
                _logger.LogWarning($"No seats selected for ShowTime {ticketCreateRequest.ShowTimeId}.");
                throw new Exception("No seats selected");
            }

            // Kiểm tra tính khả dụng của ghế
            var seats = await _dbContext.Seats
                .Where(s => ticketCreateRequest.SeatIds.Contains(s.Id) &&
                            !_dbContext.TicketSeats.Any(ts => ts.SeatId == s.Id && ts.Ticket.ShowTimeId == ticketCreateRequest.ShowTimeId && ts.Status == "Booked"))
                .ToListAsync();

            if (seats.Count != ticketCreateRequest.SeatIds.Count)
            {
                _logger.LogWarning($"One or more seats are already booked for ShowTime {ticketCreateRequest.ShowTimeId}.");
                throw new Exception("One or more seats are already booked");
            }
            var totalPrice = seats.Sum(s => s.Price);
            if (!string.IsNullOrEmpty(ticketCreateRequest.DiscountCode))
            {
                var discountApplied = await _discountService.ApplyDiscountAsync(userId, ticketCreateRequest.DiscountCode);
                if (discountApplied)
                {
                    var discount = await _dbContext.Discounts.SingleOrDefaultAsync(d => d.Code == ticketCreateRequest.DiscountCode);
                    if (discount != null)
                    {
                        totalPrice -= totalPrice * ((decimal)discount.Percentage / 100);
                    }
                }
            }
            // Tính tổng giá tiền và tạo ticket
            var ticket = new Ticket
            {
                UserId = userId,
                ShowTimeId = ticketCreateRequest.ShowTimeId,
                TotalPrice = totalPrice,
                BookingTime = DateTime.UtcNow,
                Status = StatusConstants.Ticket.Active,
            };

            _dbContext.Tickets.Add(ticket);
            await _dbContext.SaveChangesAsync();

            // Tạo danh sách TicketSeat
            foreach (var seat in seats)
            {
                var ticketSeat = new TicketSeat
                {
                    TicketId = ticket.Id,
                    SeatId = seat.Id,
                    Status = StatusConstants.Ticket.Booked,
                };
                _dbContext.TicketSeats.Add(ticketSeat);
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Ticket {ticket.Id} created successfully for ShowTime {ticketCreateRequest.ShowTimeId}.");

            return new TicketDto
            {
                TicketId = ticket.Id,
                MovieTitle = showTime.Movie.Title,
                CinemaName = showTime.CinemaRoom.Cinema.Name,
                RoomName = showTime.CinemaRoom.Name,
                ShowTime = showTime.StartTime,
                Seats = seats.Select(s => new SeatDto(s)).ToList(),
                TotalPrice = ticket.TotalPrice,
                BookingTime = ticket.BookingTime,
                Status = ticket.Status,
                PaymentStatus = ticket.PaymentStatus,
            };
        }

        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            _logger.LogInformation("Fetching all tickets.");

            // Kiểm tra và cập nhật trạng thái vé
            await UpdateTicketStatusAsync();

            return await _dbContext.Tickets
                .Include(t => t.ShowTime)
                .Include(t => t.TicketSeats)
                    .ThenInclude(ts => ts.Seat)
                .Select(t => new TicketDto
                {
                    TicketId = t.Id,
                    MovieTitle = t.ShowTime.Movie.Title,
                    CinemaName = t.ShowTime.CinemaRoom.Cinema.Name,
                    RoomName = t.ShowTime.CinemaRoom.Name,
                    ShowTime = t.ShowTime.StartTime,
                    Seats = t.TicketSeats.Select(ts => new SeatDto
                    {
                        Id = ts.Seat.Id,
                        SeatCode = ts.Seat.SeatCode,
                        SeatType = ts.Seat.SeatType,
                        Price = ts.Seat.Price
                    }).ToList(),
                    TotalPrice = t.TotalPrice,
                    BookingTime = t.BookingTime,
                    Status = t.Status,
                    PaymentStatus = t.PaymentStatus,
                })
                .ToListAsync();
        }
        public async Task UpdateTicketStatusAsync()
        {
            _logger.LogInformation("Updating ticket and seat statuses...");

            // Lấy danh sách vé còn hạn nhưng lịch chiếu đã qua
            var expiredTickets = await _dbContext.Tickets
                .Include(t => t.ShowTime)
                .Include(t => t.TicketSeats)
                .Where(t => t.Status == "Còn hạn" && t.ShowTime.StartTime < DateTime.UtcNow)
                .ToListAsync();

            if (!expiredTickets.Any())
            {
                _logger.LogInformation("No tickets to update.");
                return;
            }

            foreach (var ticket in expiredTickets)
            {
                // Đổi trạng thái vé thành "Hết hạn"
                ticket.Status = "Hết hạn";

                // Đặt lại trạng thái ghế trong TicketSeat
                foreach (var ticketSeat in ticket.TicketSeats)
                {
                    ticketSeat.Status = "Available";
                }

                _logger.LogInformation($"Updated ticket {ticket.Id} and reset related seats.");
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<TicketDto> GetTicketByIdAsync(int ticketId)
        {
            _logger.LogInformation($"Fetching ticket with ID {ticketId}.");

            // Lấy vé và các thông tin liên quan qua Include
            var ticket = await _dbContext.Tickets
                .Include(t => t.ShowTime)
                    .ThenInclude(st => st.Movie) // Join đến Movie
                .Include(t => t.ShowTime.CinemaRoom)
                    .ThenInclude(cr => cr.Cinema) // Join đến Cinema
                .Include(t => t.TicketSeats) // Join đến TicketSeats
                    .ThenInclude(ts => ts.Seat) // Join đến Seat
                .FirstOrDefaultAsync(t => t.Id == ticketId); // Lọc theo TicketId
           

            if (ticket == null)
            {
                _logger.LogWarning($"Ticket with ID {ticketId} not found.");
                return null;
            }

            // Kiểm tra null và log cảnh báo nếu dữ liệu không đầy đủ
            if (ticket.ShowTime?.Movie == null || ticket.ShowTime.CinemaRoom?.Cinema == null || ticket.TicketSeats == null)
            {
                _logger.LogWarning($"Incomplete data for ticket {ticketId}.");
                throw new Exception("Incomplete ticket data.");
            }

            // Tạo đối tượng TicketDto để trả về
            var ticketDto = new TicketDto
            {
                TicketId = ticket.Id,
                MovieTitle = ticket.ShowTime.Movie.Title,
                CinemaName = ticket.ShowTime.CinemaRoom.Cinema.Name,
                RoomName = ticket.ShowTime.CinemaRoom.Name,
                ShowTime = ticket.ShowTime.StartTime,
                Seats = ticket.TicketSeats.Select(ts => new SeatDto
                {
                    Id = ts.Seat.Id,
                    SeatCode = ts.Seat.SeatCode,
                    SeatType = ts.Seat.SeatType,
                    Price = ts.Seat.Price
                }).ToList(),
                TotalPrice = ticket.TotalPrice,
                BookingTime = ticket.BookingTime,
                Status = ticket.Status,
                PaymentStatus = ticket.PaymentStatus,
            };

            _logger.LogInformation($"Successfully fetched ticket with ID {ticketId}.");
            return ticketDto;
        }
        public async Task UpdatePaymentStatusAsync(int ticketId)
        {
            _logger.LogInformation($"Updating payment status for ticket with ID {ticketId}.");

            var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
            {
                _logger.LogWarning($"Ticket with ID {ticketId} not found.");
                throw new Exception("Ticket not found");
            }

            // Cập nhật trạng thái thanh toán
            ticket.PaymentStatus = "đã thanh toán";

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Payment status for ticket {ticketId} updated to 'đã thanh toán'.");
        }
    }
}
