using GlsAPI.Models.Responses;

namespace GlsAPI.Interfaces
{
    public interface IUserService
    {
        AuthResponse Login(string username, string password);
        AuthResponse Logout(Guid session);
    }
}
