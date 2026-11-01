using Microsoft.EntityFrameworkCore;
using VnPayMvc.Web.Models;
using VnPayMvc.Web.Models.Enums;

namespace VnPayMvc.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình Index Unique cho OrderCode (vnp_TxnRef)
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            // Seed Data 5 sản phẩm mẫu
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Tai nghe Bluetooth Sony WH-1000XM5",
                    Description = "Tai nghe chống ồn cao cấp, thời lượng pin 30 giờ",
                    Price = 6490000m,
                    ImageUrl = "/images/products/sony-wh1000xm5.jpg",
                    StockQuantity = 15,
                    CreatedAt = new DateTime(2026, 11, 1, 8, 0, 0)
                },
                new Product
                {
                    Id = 2,
                    Name = "Bàn phím cơ Keychron K2 V2",
                    Description = "Bàn phím cơ không dây Layout 84 phím, Switch Gateron",
                    Price = 1990000m,
                    ImageUrl = "/images/products/keychron-k2.jpg",
                    StockQuantity = 30,
                    CreatedAt = new DateTime(2026, 11, 1, 8, 0, 0)
                },
                new Product
                {
                    Id = 3,
                    Name = "Chuột không dây Logitech MX Master 3S",
                    Description = "Chuột công thái học cao cấp, cảm biến 8000 DPI",
                    Price = 2450000m,
                    ImageUrl = "/images/products/mx-master-3s.jpg",
                    StockQuantity = 20,
                    CreatedAt = new DateTime(2026, 11, 1, 8, 0, 0)
                },
                new Product
                {
                    Id = 4,
                    Name = "Màn hình Dell UltraSharp U2723QE 27 inch 4K",
                    Description = "Màn hình 4K IPS Black, độ bao phủ màu 98% DCI-P3",
                    Price = 12890000m,
                    ImageUrl = "/images/products/dell-u2723qe.jpg",
                    StockQuantity = 10,
                    CreatedAt = new DateTime(2026, 11, 1, 8, 0, 0)
                },
                new Product
                {
                    Id = 5,
                    Name = "Loa Bluetooth JBL Charge 5",
                    Description = "Loa di động chống nước IP67, công suất 40W",
                    Price = 3590000m,
                    ImageUrl = "/images/products/jbl-charge-5.jpg",
                    StockQuantity = 25,
                    CreatedAt = new DateTime(2026, 11, 1, 8, 0, 0)
                }
            );
        }
    }
}
