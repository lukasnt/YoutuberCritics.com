
using System.Collections.Generic;
using System.Linq;
using YoutuberCritics.Models;

namespace YoutuberCritics.Services
{
    
    public class CacheService
    {
        private IEnumerable<Channel> _channels;
        private IEnumerable<Review> _reviews;

        public CacheService() {
            _channels = new List<Channel>();
            _reviews = new List<Review>();
        }

        public IEnumerable<Channel> GetChannels()
        {
            return _channels;
        }

        public IEnumerable<Review> GetReviews()
        {
            return _reviews;
        }

        public void SetChannels(IEnumerable<Channel> channels)
        {
            _channels = channels;
        }

        public void SetReviews(IEnumerable<Review> reviews)
        {
            _reviews = reviews;
        }

        public void AddChannel(Channel channel)
        {
            ((List<Channel>) _channels).Add(channel);
        }

        public void AddReview(Review review)
        {
            ((List<Review>) _reviews).Insert(0, review);
        }
    }
}