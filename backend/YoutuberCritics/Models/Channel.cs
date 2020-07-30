using System.ComponentModel.DataAnnotations;

namespace YoutuberCritics.Models 
{
    public class Channel 
    {
        public Channel() { }

        public Channel(string title, string youtubePath, string imageURL, string description, int subs)
        {
            this.Title = title;
            this.YoutubePath = youtubePath;
            this.ImageURL = imageURL;
            this.Description = description;
            this.Subscribers = subs;
        }

        [Key]
        public int ChannelID { get; set; }
        
        [MaxLength(50)]
        public string Title { get; set; }
        
        [MaxLength(50)]
        public string YoutubePath { get; set; }

        [MaxLength(256)]
        public string ImageURL { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public int Subscribers { get; set; }

        public int ReviewCount { get; set; }

        public double RatingAverage { get; set; }
        
    }
}