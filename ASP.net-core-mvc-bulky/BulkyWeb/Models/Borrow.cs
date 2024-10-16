using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models

{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NameBook { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int NumerBorrow { get; set; }

        [Required]
        public DateTime TimeBorrow { get; set; }
    }
}
