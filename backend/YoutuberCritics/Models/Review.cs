using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YoutuberCritics.Models
{
    public class Review
    {

        [Key]
        public int ReviewID { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Text { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }
        
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        
        public int ChannelID { get; set; }
        [ForeignKey("ChannelID")]
        public Channel Channel { get; set; }
        
    }
}