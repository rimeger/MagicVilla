using AutoMapper;
using MagicVilla_WebApi.Data;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.Dto;
using MagicVilla_WebApi.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_WebApi.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private string _secretKey;
		private readonly IMapper _mapper;

		public UserRepository(ApplicationDbContext db, IConfiguration configuration,
			UserManager<ApplicationUser> userManager, IMapper mapper,
            RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_secretKey = configuration.GetValue<string>("ApiSettings:Secret");
		}

		public bool IsUniqueUser(string username)
		{
			var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			var user = _db.ApplicationUsers
				.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

			if (user == null || !isValid)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					User = null
				};
			}

			//if user was found generate JWT token
			var roles = await _userManager.GetRolesAsync(user);
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_secretKey);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString()),
					new Claim(ClaimTypes.Role, roles.FirstOrDefault())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			LoginResponseDTO loginResponseDTO = new LoginResponseDTO
			{
				Token = tokenHandler.WriteToken(token),
				User = _mapper.Map<UserDTO>(user),
				Role = roles.FirstOrDefault()
			};

			return loginResponseDTO;
		}

		public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
		{
			ApplicationUser user = new ApplicationUser()
			{
				UserName = registrationRequestDTO.UserName,
				Email = registrationRequestDTO.UserName,
				NormalizedEmail = registrationRequestDTO.UserName.ToUpper(),
				Name = registrationRequestDTO.Name
			};

			try
			{
				var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
				if (result.Succeeded)
				{
					if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					{
						await _roleManager.CreateAsync(new IdentityRole("admin"));
						await _roleManager.CreateAsync(new IdentityRole("customer"));
					}
					await _userManager.AddToRoleAsync(user, "admin");
					var userToReturn = _db.ApplicationUsers
						.FirstOrDefault(u => u.UserName == registrationRequestDTO.UserName);
					return _mapper.Map<UserDTO>(userToReturn);
				}
			}
			catch (Exception e)
			{

				throw;
			}

			return new UserDTO();
		}
	}
}
