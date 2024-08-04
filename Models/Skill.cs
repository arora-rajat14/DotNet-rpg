using System.Collections.Generic;

namespace dotnet_rpg.Models
{
  public class Skill
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }

    // many-to-many relationship
    public List<Character> Characters { get; set; }
  }
}