namespace GlsAPI.Models.Responses
{
    public class AuthResponse
    {
        public Guid? session {  get; set; }
        public Error? Error { get; set; }
    }
}
