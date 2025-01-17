using System.Collections.Generic;

namespace dotnet_rpg.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        // One-to-Many relationship(One User multiple characters)
        public List<Character> Characters { get; set; }
    }
}