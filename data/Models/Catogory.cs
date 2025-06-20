using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catogory
{
    [Table("Mycatogory")]
    public class Catogory
    {
        [Key]
        public int CatogoryId { get; set; }

        [Required(ErrorMessage = "Catogory name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Catogory Description is required.")]
        [StringLength(100)]
        public string Description { get; set; }

    }
}