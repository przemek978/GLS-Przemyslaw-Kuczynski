using GlsAPI.Models;
using GlsAPI.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace GlsAPI.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
        AuthResponse Login(User user);
        AuthResponse Logout(Guid SessionId);
        Guid CreateSession(int userId);
        Error GetError(string name);
    }
}
