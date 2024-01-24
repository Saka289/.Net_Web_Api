using AutoMapper;
using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Fight;
using DOTNET_RPG.Dtos.Skill;
using DOTNET_RPG.Dtos.Weapon;

namespace DOTNET_RPG
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Character, GetCharacterDto>();

			CreateMap<AddCharacterDto, Character>();

			CreateMap<UpdateCharacterDto, Character>();

			CreateMap<Weapon, GetWeaponDto>();

			CreateMap<Skill, GetSkillDto>();

			CreateMap<Character, HighScoreDto>();
		}
	}
}
