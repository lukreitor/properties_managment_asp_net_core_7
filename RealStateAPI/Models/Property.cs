using System.Text.Json.Serialization;

namespace RealStateAPI.Models
{
    public class Property
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Detail { get; set; }
        public String Address { get; set; }
        public String ImageUrl { get; set; }
        public int Price { get; set; }
        public bool IsTreding { get; set;} 
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
