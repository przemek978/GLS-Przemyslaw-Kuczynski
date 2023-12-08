using Azure;
using GlsAPI.Data;
using GlsAPI.Interfaces;
using GlsAPI.Models;
using GlsAPI.Models.Responses;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GlsAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dBContext;
        public UserRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public User GetUser(string username, string password)
        {
            var hasher = new PasswordHasher<User>();
            var user = _dBContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                var passwordValid = hasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success;
                if (passwordValid)
                {
                    return user;
                }
            }
            return null;
        }
        public AuthResponse Login(User user)
        {
            AuthResponse response = new AuthResponse();
            var session = _dBContext.Sessions.ToList().Where(s => s.UserId == user.Id && s.IsActive);
            if (session.Any())
            {
                response.Error = _dBContext.Errors.FirstOrDefault(e => e.Name.Equals("err_sess_create_problem"));
                return response;
            }
            if (user.IsBlocked)
            {
                response.Error = _dBContext.Errors.FirstOrDefault(e => e.Name.Equals("err_user_blocked"));
                return response;
            }
            user.IsLogged = true;
            var newSession = CreateSession(user.Id);
            response.session = newSession;
            return response;
        }

        public AuthResponse Logout(Guid SessionId)
        {
            User user = new();
            AuthResponse response = new AuthResponse();
            var hasher = new PasswordHasher<User>();
            var session = _dBContext.Sessions.FirstOrDefault(s => s.Id == SessionId);
            if (session == null)
            {
                response.Error = _dBContext.Errors.FirstOrDefault(e => e.Name.Equals("err_sess_not_found"));
                return response;
            }
            else
            {
                session.IsActive = false;
                user = _dBContext.Users.FirstOrDefault(u => u.Id == session.UserId);
                if (user == null)
                {
                    response.Error = _dBContext.Errors.FirstOrDefault(e => e.Name.Equals("err_sess_not_found"));
                    return response;
                }
                else
                {
                    user.IsLogged = false;
                }
            }
            _dBContext.SaveChanges();
            response.session = SessionId;
            return response;
        }

        public Guid CreateSession(int userId)
        {
            var newGuid = Guid.NewGuid();
            _dBContext.Sessions.Add(new Session { Id = newGuid, UserId = userId, ExpireDateTime= DateTime.Now.AddDays(1) });
            _dBContext.SaveChanges();
            return newGuid;
        }

        public Error GetError(string name)
        {
            return _dBContext.Errors.FirstOrDefault(e => e.Name.Equals(name));
        }
    }
}
