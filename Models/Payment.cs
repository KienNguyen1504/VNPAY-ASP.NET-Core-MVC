using System;
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

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order? Order { get; set; }

        [Required]
        [StringLength(100)]
        public string TxnRef { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string? BankCode { get; set; }

        [StringLength(100)]
        public string? TransactionNo { get; set; }

        [StringLength(10)]
        public string? ResponseCode { get; set; }

        [StringLength(10)]
        public string? TransactionStatus { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? PayDate { get; set; }
    }
}
