namespace GlsAPI.Models.Responses
{
    public class LabelResponse:Response
    {
        public List<string> Labels { get; set; } = new List<string>();
    }
}
