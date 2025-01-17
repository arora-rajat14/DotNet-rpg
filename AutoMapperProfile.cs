using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;

namespace dotnet_rpg
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      // CreateMap<TSource, TDestination>();
      CreateMap<Character, GetCharacterDto>();
      CreateMap<AddCharacterDto, Character>();
      CreateMap<Weapon, GetWeaponDto>();
      CreateMap<Skill, GetSkillDto>();
    }
  }
}
