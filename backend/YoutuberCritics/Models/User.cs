using System.ComponentModel.DataAnnotations;

namespace YoutuberCritics.Models
{
    public class User 
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}