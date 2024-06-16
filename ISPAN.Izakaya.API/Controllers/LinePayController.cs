using ISPAN.Izakaya.BLL_Service_;
using ISPAN.Izakaya.BLL_Service_.Dtos;
using Microsoft.AspNetCore.Mvc;



namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;

        public LinePayController(LinePayService linePayService)
        {
            _linePayService = linePayService;
        }

        [HttpPost("Request")]
        public async Task<IActionResult> CreateOrder(LinePayOrderDetail orderDetail)
        {
            try
            {
                var response = await _linePayService.SendPaymentRequest(orderDetail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, [FromBody] PaymentConfirmDto dto)
        {
            try
            {
                var response = await _linePayService.ConfirmPayment(transactionId, orderId, dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("Cancel")]
        public IActionResult CancelTransaction([FromQuery] string transactionId)
        {
            try
            {
                var message = _linePayService.TransactionCancel(transactionId);
                return Ok(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
