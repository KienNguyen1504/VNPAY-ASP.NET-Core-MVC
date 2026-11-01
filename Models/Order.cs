using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VnPayMvc.Web.Models.Enums;

namespace VnPayMvc.Web.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Mã tham chiếu đơn hàng gửi sang VNPAY (vnp_TxnRef). Duy nhất trên hệ thống.
        /// Định dạng gợi ý: ORD_yyyyMMddHHmmss_Id
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OrderCode { get; set; } = string.Empty;

        // Khoá ngoại liên kết sản phẩm (Phạm vi website bán hàng mẫu/MVP 1 sản phẩm per order)
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Tổng tiền đơn hàng (VND). Backend tính toán, không nhận từ Client.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [StringLength(255)]
        public string? OrderNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Property: Một đơn hàng có lịch sử/bản ghi thanh toán
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
