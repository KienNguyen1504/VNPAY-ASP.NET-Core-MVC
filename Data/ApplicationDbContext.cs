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

            // Cấu hình Unique Index bảo đảm tính duy nhất tuyệt đối của OrderCode ở cấp độ Cơ sở dữ liệu
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderCode)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.TxnRef)
                .IsUnique();

            // Cấu hình SQL Check Constraint không cho phép giá trị âm ở StockQuantity
            modelBuilder.Entity<Product>()
                .ToTable(b => b.HasCheckConstraint("CK_Product_StockQuantity_NonNegative", "[StockQuantity] >= 0"));

            // Cấu hình Restrict Delete giữa Product và Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Cascade Delete giữa Order và Payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}