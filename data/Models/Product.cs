
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clasProduct
{
    [Table("MyProducts")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Provider name is required.")]
        [StringLength(100)]
        public string Provider { get; set; }
        public void Display()
        {
            Console.WriteLine($"ID: {ProductId}, Name: {Name}, Provider: {Provider}");
        }
    }
}