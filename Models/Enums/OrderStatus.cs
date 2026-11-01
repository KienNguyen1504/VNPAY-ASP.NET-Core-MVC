namespace VnPayMvc.Web.Models.Enums
{
    /// <summary>
    /// Trạng thái nghiệp vụ của Đơn hàng (Order)
    /// </summary>
    public enum OrderStatus
    {
        Pending = 0,    // Đơn hàng mới khởi tạo, chờ thanh toán qua VNPAY
        Paid = 1,       // Đã nhận xác nhận thanh toán thành công từ VNPAY IPN
        Failed = 2,     // Thanh toán thất bại (lỗi thẻ, sai OTP, hết hạn)
        Cancelled = 3   // Người dùng chủ động hủy giao dịch tại cổng VNPAY
    }

    /// <summary>
    /// Trạng thái chi tiết giao dịch thanh toán (Payment)
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 0,    // Đã tạo URL VNPAY, chờ kết quả từ VNPAY
        Success = 1,    // IPN xác nhận thanh toán thành công (ResponseCode 00 & TransactionStatus 00)
        Failed = 2,     // Giao dịch thanh toán không thành công
        Invalid = 3     // Sai chữ ký checksum hoặc không qua được các bước kiểm tra IPN
    }
}
