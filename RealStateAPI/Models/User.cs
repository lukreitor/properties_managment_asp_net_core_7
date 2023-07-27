namespace RealStateAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Password { get; set; }
        public ICollection<Property> Properties { get; set; } // this will be a list of properties
        
    }
}
