using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ProductSQLTest2.Models
{
    [Table("Products", Schema="")]
    public class Products
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        [Display(Name = "Product Id")]
        public Int32 ProductId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public Int32 Quantity { get; set; }

        [Required]
        [Display(Name = "Box Size")]
        public Int32 BoxSize { get; set; }

        [Required]
        [Display(Name = "Price")]
        public Decimal Price { get; set; }


    }
}
 
