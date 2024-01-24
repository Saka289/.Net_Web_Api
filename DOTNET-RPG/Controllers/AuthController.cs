using DOTNET_RPG.Data;
using DOTNET_RPG.Dtos.User;
using DOTNET_RPG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _authRepo;

		public AuthController(IAuthRepository authRepo)
		{
			_authRepo = authRepo;
		}

		[HttpPost("register")]
		public async Task<ActionResult<ServiceRespone<int>>> Register(UserRegisterDto request)
		{
			var respone = await _authRepo.Register(
				new User { UserName = request.Username }, request.Password
				);
			if(respone.Success)
			{
				return BadRequest(respone);
			}
			return Ok(respone);
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceRespone<string>>> Login(UserLoginDto request)
		{
			var respone = await _authRepo.Login(request.Username, request.Password);
			if (respone.Success)
			{
				return BadRequest(respone);
			}
			return Ok(respone);
		}
	}
}
