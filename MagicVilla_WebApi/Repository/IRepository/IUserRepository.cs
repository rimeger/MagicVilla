using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.Dto;

namespace MagicVilla_WebApi.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
		Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
	}
}
