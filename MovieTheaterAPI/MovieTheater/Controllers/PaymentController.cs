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

        // API tạo mã QR thanh toán
        [HttpGet("{ticketId}/generate-qr")]
        public async Task<IActionResult> GenerateQr(int ticketId)
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

        [HttpPost("{ticketId}/verify")]
        public async Task<IActionResult> VerifyPayment(int ticketId)
        {
            try
            {
                var result = await _vietQrService.VerifyPaymentAsync(ticketId);
                if (result)
                    return Ok(new { message = "Payment verified successfully." });
                return BadRequest(new { message = "Payment not verified or pending." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
