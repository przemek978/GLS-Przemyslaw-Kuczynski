using GlsAPI.Models.Responses.Items;

namespace GlsAPI.Models.Responses
{
    public class PackagesResponse: Response
    {
        public List<string> package_id { get; set; } = new List<string>();
    }
}
