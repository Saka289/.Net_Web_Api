using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Dtos.Fight;
using DOTNET_RPG.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DOTNET_RPG.Data
{
    public class AuthRepository : IAuthRepository
	{
		private readonly DataContext _context;
		private readonly IConfiguration _configuration;

		public AuthRepository(DataContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public async Task<ServiceRespone<string>> Login(string username, string password)
		{
			var respone = new ServiceRespone<string>();
			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username.ToLower()));
			if (user == null)
			{
				respone.Success = false;
				respone.Message = "User not found.";
			}
			else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
			{
				respone.Success = false;
				respone.Message = "Wrong password.";
			}
			else
			{
				respone.Data = CreateToken(user);
			}
			return respone;
		}

		public async Task<ServiceRespone<int>> Register(User user, string password)
		{
			ServiceRespone<int> respone = new ServiceRespone<int>();
			if (await UserExists(user.UserName))
			{
				respone.Success = false;
				respone.Message = "User already exists.";
				return respone;
			}
			CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			respone.Data = user.Id;
			return respone;
		}

		public async Task<bool> UserExists(string username)
		{
			if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
			{
				return true;
			}
			return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computeHash.SequenceEqual(passwordHash);
			}
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName)
			};

			SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
				.GetBytes(_configuration.GetSection("Jwt:Token").Value));

			SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
