using GlsAPI.Interfaces;
using GlsAPI.Models.Responses;

namespace GlsAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AuthResponse Login(string username, string password)
        {
            AuthResponse response = new AuthResponse();
            var user = _userRepository.GetUser(username, password);
            if (user == null)
            {
                response.Error = _userRepository.GetError("err_user_incorrect_username_password");
            }
            else
            {
                response = _userRepository.Login(user);
            }
            return response;
        }
        public AuthResponse Logout(Guid session)
        {
            var response = _userRepository.Logout(session);
            return response;
        }
    }
}
