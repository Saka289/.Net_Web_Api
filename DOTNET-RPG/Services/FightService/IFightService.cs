using DOTNET_RPG.Dtos.Fight;

namespace DOTNET_RPG.Services.FightService
{
	public interface IFightService
	{
		Task<ServiceRespone<AttackResultDto>> WeaponAttack(WeaponAttackDto request);

		Task<ServiceRespone<AttackResultDto>> SkillAttack(SkillAttackDto request);

		Task<ServiceRespone<FightResultDto>> Fight(FightRequestDto request);

		Task<ServiceRespone<List<HighScoreDto>>> GetHighScore();
	}
}
