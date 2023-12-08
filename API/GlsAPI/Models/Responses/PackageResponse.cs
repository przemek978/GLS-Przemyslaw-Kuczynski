using GlsAPI.Models.Responses.Items;

namespace GlsAPI.Models.Responses
{
    public class PackageResponse: Response
    {
        public PackageItem Package { get; set; } = new PackageItem();
    }
}
