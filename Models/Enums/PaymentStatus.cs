namespace VnPayMvc.Web.Models.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,        // Đang chờ kết quả giao dịch
        Success = 1,        // Giao dịch thành công
        Failed = 2,         // Giao dịch thất bại
        Cancelled = 3,      // Khách hủy giao dịch
        Invalid = 4,        // Dữ liệu giao dịch không hợp lệ
        ReviewRequired = 5  // Giao dịch cần xác minh thêm (VD: mã 07)
    }
}
