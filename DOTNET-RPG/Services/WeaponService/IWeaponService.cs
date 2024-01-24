using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Weapon;

namespace DOTNET_RPG.Services.WeaponService
{
	public interface IWeaponService
	{
		Task<ServiceRespone<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
	}
}
