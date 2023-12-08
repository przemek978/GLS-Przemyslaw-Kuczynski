namespace GlsAPI.Models.Responses.Items
{
    public class PackageItem
    {
        public string Package_Id { get; set; }
        public RecipientInPackageItem? Recipient { get; set; }
    }
}
