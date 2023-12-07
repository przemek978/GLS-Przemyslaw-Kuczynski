using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GlsAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [JsonIgnore]
        [SwaggerSchema(Format = "password")]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [JsonIgnore]
        [SwaggerSchema(Format = "password")]
        public string ConfirmPassword { get; set; }
        public bool IsLogged {  get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsBlocked { get; set; } = false;

        public string PasswordHash()
        {
            var hasher = new PasswordHasher<User>();
            Password = hasher.HashPassword(this, Password);
            return Password;
        }
    }
}
