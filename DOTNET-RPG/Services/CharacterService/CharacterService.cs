using AutoMapper;
using DOTNET_RPG.Data;
using DOTNET_RPG.Dtos.Character;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DOTNET_RPG.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private static List<Character> characters = new List<Character>()
		{
			new Character(),
			new Character
			{
				Id =1, Name = "Peter"
			}
		};
		private readonly IMapper _mapper;

		private readonly DataContext _context;

		private readonly IHttpContextAccessor _httpContextAccessor;

		public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
		{
			_mapper = mapper;
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

		public async Task<ServiceRespone<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
		{
			var serviceRespone = new ServiceRespone<List<GetCharacterDto>>();
			Character character = _mapper.Map<Character>(newCharacter);
			character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
			_context.Characters.Add(character);
			await _context.SaveChangesAsync();
			serviceRespone.Data = await _context.Characters
				.Where(u => u.User.Id == GetUserId())
				.Select(c => _mapper.Map<GetCharacterDto>(c))
				.ToListAsync();
			return serviceRespone;
		}

		public async Task<ServiceRespone<List<GetCharacterDto>>> GetAllCharacter()
		{
			var serviceRespone = new ServiceRespone<List<GetCharacterDto>>();
			var dbCharacters = await _context.Characters
				.Include(c => c.Weapon)
				.Include(c => c.Skills)
				.Where(u => u.User.Id == GetUserId())
				.ToListAsync();
			serviceRespone.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			return serviceRespone;
		}

		public async Task<ServiceRespone<GetCharacterDto>> GetChartacterById(int id)
		{
			var serviceRespone = new ServiceRespone<GetCharacterDto>();
			var dbCharacter = await _context.Characters
				.Include(c => c.Weapon)
				.Include(c => c.Skills)
				.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
			serviceRespone.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
			return serviceRespone;
		}

		public async Task<ServiceRespone<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
		{
			ServiceRespone<GetCharacterDto> serviceRespone = new ServiceRespone<GetCharacterDto>();
			try
			{
				var character = await _context.Characters
					.Include(c => c.User)
					.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);

				if (character.User.Id == GetUserId())
				{
					_mapper.Map(updateCharacter, character);

					await _context.SaveChangesAsync();

					serviceRespone.Data = _mapper.Map<GetCharacterDto>(character);
					//character.Name = updateCharacter.Name;
					//character.HitPoints = updateCharacter.HitPoints;
					//character.Strength = updateCharacter.Strength;
					//character.Defense = updateCharacter.Defense;
					//character.Intelligence = updateCharacter.Intelligence;
					//character.Class = updateCharacter.Class;
				}
				else
				{
					serviceRespone.Success = false;
					serviceRespone.Message = "Character not found.";
				}
			}
			catch (Exception ex)
			{
				serviceRespone.Success = false;
				serviceRespone.Message = ex.Message;
			}
			return serviceRespone;
		}

		public async Task<ServiceRespone<List<GetCharacterDto>>> DeleteCharacter(int id)
		{
			ServiceRespone<List<GetCharacterDto>> respone = new ServiceRespone<List<GetCharacterDto>>();
			try
			{
				Character character = await _context.Characters
					.Include(c => c.Weapon)
					.Include(c => c.Skills)
					.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
				if (character != null)
				{
					_context.Characters.Remove(character);

					await _context.SaveChangesAsync();
					respone.Data = _context.Characters
						.Where(u => u.User.Id == GetUserId())
						.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
				}
				else
				{
					respone.Success = false;
					respone.Message = "Character not found.";
				}
			}
			catch (Exception ex)
			{
				respone.Success = false;
				respone.Message = ex.Message;
			}
			return respone;
		}

		public async Task<ServiceRespone<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
		{
			var response = new ServiceRespone<GetCharacterDto>();
			try
			{
				var character = await _context.Characters
					.Include(c => c.Weapon)
					.Include(c => c.Skills)
					.FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
				if (character == null)
				{
					response.Success = false;
					response.Message = "Character not found.";
					return response;
				}

				var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

				if (skill == null)
				{
					response.Success = false;
					response.Message = "Skill not found.";
					return response;
				}

				character.Skills.Add(skill);
				await _context.SaveChangesAsync();
				response.Data = _mapper.Map<GetCharacterDto>(character);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
	}
}
