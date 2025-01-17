using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
  public class CharacterService : ICharacterService
  {
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
      _context = context;
      _mapper = mapper;
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      Character character = _mapper.Map<Character>(newCharacter);

      character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

      _context.Characters.Add(character);
      await _context.SaveChangesAsync();
      serviceResponse.Data = await _context.Characters
          .Where(c => c.User.Id == GetUserId())
          .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      try
      {
        Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
        if (character != null)
        {
          _context.Characters.Remove(character);
          await _context.SaveChangesAsync();

          serviceResponse.Data = _context.Characters
              .Where(c => c.User.Id == GetUserId())
              .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        else
        {
          serviceResponse.Success = false;
          serviceResponse.Message = "Character not found.";
        }

      }
      catch (Exception e)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = e.Message;
      }
      return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
    {
      var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
      var dbCharacters = await _context.Characters
        .Include(c => c.Weapon)
        .Include(c => c.Skills)
        .Where(c => c.User.Id == GetUserId()).ToListAsync();
      serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var dbCharacter = await _context.Characters
        .Include(c => c.Weapon)
        .Include(c => c.Skills)
        .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      try
      {
        Character character = await _context.Characters
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
        if (character.User.Id == GetUserId())
        {

          character.Name = updatedCharacter.Name;
          character.HitPoints = updatedCharacter.HitPoints;
          character.Defense = updatedCharacter.Defense;
          character.Strength = updatedCharacter.Strength;
          character.Intelligence = updatedCharacter.Intelligence;
          character.Class = updatedCharacter.Class;

          await _context.SaveChangesAsync();

          serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        else
        {
          serviceResponse.Success = false;
          serviceResponse.Message = "Character not found.";
        }
      }
      catch (Exception e)
      {
        serviceResponse.Success = false;
        serviceResponse.Message = e.Message;
      }
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try
      {
        var character = await _context.Characters
          .Include(c => c.Weapon)
          .Include(c => c.Skills)
          .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
        if (character == null)
        {
          response.Success = false;
          response.Message = "Character does not exists.";
          return response;
        }

        var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
        if (skill == null)
        {
          response.Success = false;
          response.Message = "Skill does't exists.";
          return response;
        }

        character.Skills.Add(skill);
        await _context.SaveChangesAsync();

        response.Data = _mapper.Map<GetCharacterDto>(character);

      }
      catch (Exception e)
      {
        response.Success = false;
        response.Message = e.Message;
      }
      return response;
    }
  }
}