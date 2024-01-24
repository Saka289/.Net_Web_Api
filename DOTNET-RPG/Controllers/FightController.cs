using DOTNET_RPG.Dtos.Fight;
using DOTNET_RPG.Services.FightService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FightController : ControllerBase
	{
		private readonly IFightService _fightService;

		public FightController(IFightService fightService)
		{
			_fightService = fightService;
		}

		[HttpPost("Weapon")]
		public async Task<ActionResult<ServiceRespone<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
		{
			return Ok(await _fightService.WeaponAttack(request));	
		}

		[HttpPost("Skill")]
		public async Task<ActionResult<ServiceRespone<AttackResultDto>>> SkillAttack(SkillAttackDto request)
		{
			return Ok(await _fightService.SkillAttack(request));
		}

		[HttpPost]
		public async Task<ActionResult<ServiceRespone<FightResultDto>>> Fight(FightRequestDto request)
		{
			return Ok(await _fightService.Fight(request));
		}

		[HttpGet]
		public async Task<ActionResult<ServiceRespone<List<HighScoreDto>>>> GetHighScore()
		{
			return Ok(await _fightService.GetHighScore());
		}
	}
}
