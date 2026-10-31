using System.Globalization;

namespace VnpayMvc.Web.Helpers
{
    /// <summary>
    /// Định dạng hiển thị dùng chung cho mọi View, để giá tiền và thời gian giống nhau ở mọi trang.
    /// Trong View: @Model.TotalAmount.ToVnd()  →  "250.000 ₫"
    /// </summary>
    public static class DisplayFormat
    {
        private static readonly CultureInfo Vi = CultureInfo.GetCultureInfo("vi-VN");

        /// <summary>Hiển thị tiền VND: 250000 → "250.000 ₫" (không có phần thập phân).</summary>
        public static string ToVnd(this decimal amount) =>
            amount.ToString("#,##0", Vi) + " ₫";

        public static string ToVnd(this long amount) => ((decimal)amount).ToVnd();

        public static string ToVnd(this int amount) => ((decimal)amount).ToVnd();

        /// <summary>Hiển thị thời gian: "01/11/2026 14:05".</summary>
        public static string ToDisplayTime(this DateTime time) =>
            time.ToString("dd/MM/yyyy HH:mm", Vi);
    }
}
