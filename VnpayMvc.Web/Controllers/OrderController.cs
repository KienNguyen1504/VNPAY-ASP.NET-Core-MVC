using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VnPayMvc.Web.Data;
using VnPayMvc.Web.Models;
using VnPayMvc.Web.Models.Enums;

namespace VnpayMvc.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----------------------------------------------------------------
        // CreateOrder & Checkout: logic tạo đơn của Phương (giữ nguyên).
        // ----------------------------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Số lượng mua phải lớn hơn 0.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            if (product.StockQuantity < quantity)
            {
                return BadRequest("Số lượng tồn kho không đủ.");
            }

            decimal totalAmount = product.Price * quantity;
            string orderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);

            var order = new Order
            {
                ProductId = product.Id,
                Quantity = quantity,
                TotalAmount = totalAmount,
                OrderCode = orderCode,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now
            };

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

        // ----------------------------------------------------------------
        // Result: trang kết quả thanh toán (Nhi).
        // Trạng thái LẤY TỪ DATABASE, không tự kết luận từ URL query string.
        // ----------------------------------------------------------------

        public async Task<IActionResult> Result(int id)
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
