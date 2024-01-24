using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Weapon;
using DOTNET_RPG.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class WeaponController : ControllerBase
	{
		private readonly IWeaponService _weaponService;

		public WeaponController(IWeaponService weaponService)
		{
			_weaponService = weaponService;
		}

		[HttpPost]
		public async Task<ActionResult<ServiceRespone<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
		{
			return Ok(await _weaponService.AddWeapon(newWeapon));
		}
	}
}
