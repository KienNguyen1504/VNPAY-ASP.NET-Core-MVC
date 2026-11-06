namespace VnPayMvc.Web.Models.Enums
{
    public enum OrderStatus
    {
        Pending = 0,    // Đơn hàng chờ thanh toán
        Paid = 1,       // Đã thanh toán thành công
        Cancelled = 2   // Đơn hàng bị hủy
    }
}
