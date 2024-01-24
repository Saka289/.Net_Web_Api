using AutoMapper;
using DOTNET_RPG.Data;
using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Weapon;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DOTNET_RPG.Services.WeaponService
{
	public class WeaponService : IWeaponService
	{
		private readonly DataContext _context;

		private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly IMapper _mapper;

		public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
		}

		private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

		public async Task<ServiceRespone<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
		{
			ServiceRespone<GetCharacterDto> response = new ServiceRespone<GetCharacterDto>();
			try
			{
				Character character = await _context.Characters
					.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == GetUserId());
				if (character == null)
				{
					response.Success = false;
					response.Message = "Character not found";
					return response;
				}
				Weapon weapon = new Weapon
				{
					Name = newWeapon.Name,
					Damage = newWeapon.Damage,
					Character = character,
				};
				_context.Weapons.Add(weapon);
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
