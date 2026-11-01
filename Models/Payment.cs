using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VnPayMvc.Web.Models.Enums;

namespace VnPayMvc.Web.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Khoá ngoại liên kết đơn hàng
        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }

        /// <summary>
        /// Mã tham chiếu gửi cho VNPAY (vnp_TxnRef)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TxnRef { get; set; } = string.Empty;

        /// <summary>
        /// Số tiền thực tế gửi sang VNPAY (vnp_Amount = TotalAmount * 100)
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Mã giao dịch ghi nhận tại cổng VNPAY (vnp_TransactionNo)
        /// </summary>
        [StringLength(50)]
        public string? VnPayTransactionNo { get; set; }

        /// <summary>
        /// Mã ngân hàng/ví thanh toán (vnp_BankCode: NCB, VNPAYQR, VISA, ...)
        /// </summary>
        [StringLength(20)]
        public string? BankCode { get; set; }

        /// <summary>
        /// Loại thẻ/tài khoản (vnp_CardType: ATM, QRCODE, VISA, ...)
        /// </summary>
        [StringLength(20)]
        public string? CardType { get; set; }

        /// <summary>
        /// Mã phản hồi từ VNPAY (vnp_ResponseCode: 00 = Thành công, 24 = Hủy, ...)
        /// </summary>
        [StringLength(10)]
        public string? ResponseCode { get; set; }

        /// <summary>
        /// Trạng thái giao dịch VNPAY (vnp_TransactionStatus: 00 = Thành công)
        /// </summary>
        [StringLength(10)]
        public string? TransactionStatus { get; set; }

        /// <summary>
        /// Thời gian thanh toán phía VNPAY trả về (vnp_PayDate)
        /// </summary>
        public DateTime? PayDate { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Đánh dấu nguồn cập nhật dữ liệu (Nguồn chính: IPN, Nguồn bổ trợ: ReturnUrl)
        /// </summary>
        [StringLength(20)]
        public string ProcessedBy { get; set; } = "PENDING"; 

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Lưu toàn bộ Raw Query String nhận từ IPN để phục vụ kiểm tra/đối soát
        /// </summary>
        public string? RawIpnData { get; set; }
    }
}
