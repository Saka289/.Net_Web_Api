using DOTNET_RPG.Services;

namespace DOTNET_RPG.Data
{
    public interface IAuthRepository
	{
		Task<ServiceRespone<int>> Register(User user, string password);

		Task<ServiceRespone<string>> Login(string username, string password);

		Task<bool> UserExists(string username);
	}
}
