namespace DOTNET_RPG.Models
{
	public class Skill
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int Damage { get; set; }	

		public List<Character> Characters { get; set; }
	}
}
