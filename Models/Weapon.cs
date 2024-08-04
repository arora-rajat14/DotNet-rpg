namespace dotnet_rpg.Models
{
  public class Weapon
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Damage { get; set; }
    public Character Character { get; set; }

    // By below convention EF knows it is a foreign key of Character class.
    public int CharacterId { get; set; }
  }
}