
using Microsoft.AspNetCore.Mvc;

namespace VnpayMvc.Web.Controllers;

[ApiController]
[Route("Payment")]
public class VnPayIpnController : ControllerBase
{
    // Endpoint nhận callback IPN từ VNPAY
    [HttpGet("IPN")]
    public IActionResult Receive()
    {
        var txnRef = Request.Query["vnp_TxnRef"].ToString();
        var secureHash = Request.Query["vnp_SecureHash"].ToString();

        // Kiểm tra sự hiện diện của dữ liệu bắt buộc
        if (string.IsNullOrWhiteSpace(txnRef) ||
            string.IsNullOrWhiteSpace(secureHash))
        {
            return Ok(new
            {
                RspCode = "99",
                Message = "Input data required"
            });
        }

        // Step 10: Chỉ dựng endpoint và kiểm tra kết nối.
        // Chưa xác minh chữ ký hoặc cập nhật trạng thái đơn hàng.
        // Không được phản hồi 00 khi chưa xử lý giao dịch.
        return Ok(new
        {
            RspCode = "99",
            Message = "IPN processing not implemented"
        });
    }
}
