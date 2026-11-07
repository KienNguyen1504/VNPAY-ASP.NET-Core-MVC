
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace VnPayMvc.Web.Services
{
    public static class VnPaySignatureVerifier
    {
        public static bool IsValid(
            IQueryCollection query,
            string hashSecret)
        {
            if (string.IsNullOrWhiteSpace(hashSecret))
                return false;

            if (!query.TryGetValue("vnp_SecureHash", out var hash)
                || hash.Count != 1)
                return false;

            // Không chấp nhận tham số VNPAY bị lặp.
            if (query.Any(x =>
                x.Key.StartsWith("vnp_", StringComparison.Ordinal)
                && x.Value.Count != 1))
                return false;

            // Loại bỏ chữ ký, sắp xếp các tham số.
            var parameters = query
                .Where(x =>
                    x.Key.StartsWith("vnp_", StringComparison.Ordinal)
                    && x.Key != "vnp_SecureHash"
                    && x.Key != "vnp_SecureHashType"
                    && !string.IsNullOrEmpty(x.Value.ToString()))
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .Select(x =>
                    WebUtility.UrlEncode(x.Key) + "="
                    + WebUtility.UrlEncode(x.Value.ToString()));

            string signData = string.Join("&", parameters);

            // Tạo chữ ký HMAC-SHA512.
            using var hmac = new HMACSHA512(
                Encoding.UTF8.GetBytes(hashSecret));

            byte[] expectedHash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(signData));

            try
            {
                byte[] receivedHash =
                    Convert.FromHexString(hash.ToString());

                return CryptographicOperations.FixedTimeEquals(
                    expectedHash, receivedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
