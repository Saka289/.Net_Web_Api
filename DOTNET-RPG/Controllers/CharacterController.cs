using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}


		//[AllowAnonymous]
		[HttpGet("GetAll")]
		public async Task<ActionResult<ServiceRespone<List<GetCharacterDto>>>> Get()
		{
			return Ok(await _characterService.GetAllCharacter());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceRespone<GetCharacterDto>>> GetSingle(int id)
		{
			return Ok(await _characterService.GetChartacterById(id));
		}

		[HttpPost]
		public async Task<ActionResult<ServiceRespone<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
		{
			return Ok(await _characterService.AddCharacter(newCharacter));
		}

		[HttpPut]
		public async Task<ActionResult<ServiceRespone<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
		{
			var respone = await _characterService.UpdateCharacter(updateCharacter);
			if (respone == null)
			{
				return NotFound(respone);

			}
			return Ok(respone);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<ServiceRespone<List<GetCharacterDto>>>> DeleteCharacter(int id)
		{
			var respone = await _characterService.DeleteCharacter(id);
			if (respone == null)
			{
				return NotFound(respone);
			}
			return Ok(respone);
		}

		[HttpPost("Skill")]
		public async Task<ActionResult<ServiceRespone<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
		{
			return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
		}
	}
}
