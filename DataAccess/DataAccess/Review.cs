using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DataAccess
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int CustomerId { get; set; }

        public int TourId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("TourId")]
        public virtual Tour Tour { get; set; } = null!;
    }
}
