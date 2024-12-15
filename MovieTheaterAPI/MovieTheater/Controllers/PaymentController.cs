using Microsoft.AspNetCore.Mvc;
using MT.Applicationservices.Module.Abstracts;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVietQrService _vietQrService;

        public PaymentController(IVietQrService vietQrService)
        {
            _vietQrService = vietQrService;
        }

        [HttpGet("{ticketId}/generate-payment-qr")]
        public async Task<IActionResult> GeneratePaymentQr(int ticketId)
        {
            try
            {
                var qrUrl = await _vietQrService.GeneratePaymentQrAsync(ticketId);
                return Ok(new { qrUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{ticketId}/send-email")]
        public async Task<IActionResult> SendEmail(int ticketId)
        {
            try
            {
                await _vietQrService.SendPaymentSuccessEmailAsync(ticketId);
                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
