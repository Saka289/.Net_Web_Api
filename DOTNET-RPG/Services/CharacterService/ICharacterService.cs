using DOTNET_RPG.Dtos.Character;

namespace DOTNET_RPG.Services.CharacterService
{
    public interface ICharacterService
	{
		Task<ServiceRespone<List<GetCharacterDto>>> GetAllCharacter();

		Task<ServiceRespone<GetCharacterDto>> GetChartacterById(int id);

		Task<ServiceRespone<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);

		Task<ServiceRespone<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter);

		Task<ServiceRespone<List<GetCharacterDto>>> DeleteCharacter(int id);

		Task<ServiceRespone<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
	}
}
