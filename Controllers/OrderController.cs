using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VnPayMvc.Web.Data;
using VnPayMvc.Web.Models;
using VnPayMvc.Web.Models.Enums;

namespace VnPayMvc.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Action tạo đơn hàng và tính tổng tiền tại Backend
        /// Nguyên tắc: Không nhận TotalAmount từ Client, tự lấy giá niêm yết từ DB để nhân số lượng.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int productId, int quantity, string? orderNote)
        {
            if (quantity <= 0)
            {
                return BadRequest("Số lượng sản phẩm đặt mua không hợp lệ.");
            }

            // 1. Lấy sản phẩm từ CSDL để đảm bảo giá chính xác tuyệt đối
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            if (product.StockQuantity < quantity)
            {
                return BadRequest("Số lượng sản phẩm trong kho không đủ.");
            }

            // 2. Tự tính tổng tiền tại Backend (Bảo mật tài chính)
            decimal calculatedTotalAmount = product.Price * quantity;

            // 3. Sinh mã đơn hàng OrderCode duy nhất (Dùng làm vnp_TxnRef)
            // Định dạng: ORD_yyyyMMddHHmmss_RandomDigits
            string orderCode = $"ORD_{DateTime.Now:yyyyMMddHHmmss}_{Random.Shared.Next(100, 999)}";

            // 4. Khởi tạo thực thể Order
            var newOrder = new Order
            {
                OrderCode = orderCode,
                ProductId = productId,
                Quantity = quantity,
                TotalAmount = calculatedTotalAmount,
                Status = OrderStatus.Pending,
                OrderNote = string.IsNullOrEmpty(orderNote) ? $"Thanh toan don hang {orderCode}" : orderNote,
                CreatedAt = DateTime.Now
            };

            // 5. Lưu đơn hàng vào cơ sở dữ liệu
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            // 6. Chuyển hướng tới trang xác nhận đơn hàng / chi tiết thanh toán
            return RedirectToAction("ConfirmOrder", new { id = newOrder.Id });
        }

        /// <summary>
        /// Trang hiển thị chi tiết đơn hàng trước khi bấm "Thanh toán qua VNPAY"
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            return View(order);
        }
    }
}
