using GlsAPI.Models;
using GlsAPI.Models.Responses;
using GlsAPI.Models.Responses.Items;
using Microsoft.AspNetCore.Identity;

namespace GlsAPI.Interfaces
{
    public interface IPackageRepository
    {
        PackagesResponse GetPackagesIDs(Guid sessionId, int idStart);
        PackageResponse GetPackage(Guid sessionId, int packageId);
        LabelResponse GetConsignLabels(Guid sessionId, List<int> packageIds, string mode);
    }
}
