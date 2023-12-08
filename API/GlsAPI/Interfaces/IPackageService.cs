using GlsAPI.Models.Responses;

namespace GlsAPI.Interfaces
{
    public interface IPackageService
    {
        PackagesResponse GetPackagesIDs(Guid sessionId, int idStart);
        PackageResponse GetPackage(Guid sessionId, int packageId);
        LabelResponse GetConsignLabels(Guid sessionId, List<int> packageIds, string mode);
    }
}
