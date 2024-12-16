using Microsoft.AspNetCore.Mvc;
using MT.Applicationservices.Module.Abstracts;
using MT.Dtos.Discount;


namespace WSWEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDto>>> GetAllDiscounts()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDto>> GetDiscountById(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null) return NotFound();
            return Ok(discount);
        }

        [HttpPost]
        public async Task<ActionResult<DiscountDto>> CreateDiscount(CreateDiscountDto createDiscountDto)
        {
            var discount = await _discountService.CreateDiscountAsync(createDiscountDto);
            return CreatedAtAction(nameof(GetDiscountById), new { id = discount.Id }, discount);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DiscountDto>> UpdateDiscount(int id, UpdateDiscountDto updateDiscountDto)
        {
            var discount = await _discountService.UpdateDiscountAsync(id, updateDiscountDto);
            if (discount == null) return NotFound();
            return Ok(discount);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            var result = await _discountService.DeleteDiscountAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("apply")]
        public async Task<ActionResult> ApplyDiscount(int userId, [FromBody] string code)
        {
            var result = await _discountService.ApplyDiscountAsync(userId, code);
            if (!result) return BadRequest("Invalid discount code or usage limit reached.");
            return Ok();
        }
    }
}
