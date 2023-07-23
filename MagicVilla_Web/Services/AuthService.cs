using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_Web.Services
{
	public class AuthService : BaseService, IAuthService
	{
		private readonly IHttpClientFactory _httpClient;
		private string _villaUrl;

		public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
		{
			_httpClient = httpClient;
			_villaUrl = configuration.GetValue<string>("ServicesUrls:VillaAPI");
		}

		public Task<T> LoginAsync<T>(LoginRequestDTO obj)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = SD.ApiType.POST,
				Data = obj,
				Url = _villaUrl + "/api/UsersAuth/login"
			});
		}

		public Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate)
		{
			return SendAsync<T>(new APIRequest
			{
				ApiType = SD.ApiType.POST,
				Data = objToCreate,
				Url = _villaUrl + "/api/UsersAuth/register"
			});
		}
	}
}
