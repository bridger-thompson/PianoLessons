using PianoLessons.Shared.Data;

namespace PianoLessons.Services
{
	public interface IAuthService
	{
		IdentityModel.OidcClient.Browser.IBrowser Browser { set; }
		PianoLessonsUser User { get; }

		Task<LoginResult> LoginAsync();
		Task Logout();
		Task RegisterUser(bool isTeacher, string name);
		Task<LoginResult> SilentLogin();
	}
}