using System;
using System.Threading.Tasks;
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

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(int productId, int quantity)
        {
            // 1. Kiểm thử số lượng không hợp lệ
            if (quantity <= 0)
            {
                return BadRequest("Số lượng đặt mua phải lớn hơn 0.");
            }

            // 2. Lấy thông tin sản phẩm từ Database tại Backend (Ngăn chặn can thiệp giá từ Client)
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại trong hệ thống.");
            }

            // 3. Kiểm thử và kiểm tra số lượng tồn kho khả dụng
            if (product.StockQuantity < quantity)
            {
                return BadRequest($"Số lượng tồn kho không đủ. Tồn kho hiện tại: {product.StockQuantity}.");
            }

            // 4. Tính toán tổng tiền an toàn tại máy chủ (Backend Price Calculation)
            decimal totalAmount = product.Price * quantity;

            // 5. Sinh mã đơn hàng có xác suất trùng cực thấp (Kết hợp GUID ngắn và Timestamp)
            string uniqueSuffix = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            string orderCode = $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{uniqueSuffix}";

            var order = new Order
            {
                ProductId = product.Id,
                Quantity = quantity,
                TotalAmount = totalAmount,
                OrderCode = orderCode,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now
            };

            // 6. Thực thi giao dịch lưu bản ghi an toàn vào cơ sở dữ liệu
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Checkout", new { id = order.Id });
        }

        public async Task<IActionResult> Checkout(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}


