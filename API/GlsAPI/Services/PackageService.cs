using GlsAPI.Interfaces;
using GlsAPI.Models.Responses;

namespace GlsAPI.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public PackagesResponse GetPackagesIDs(Guid sessionId, int idStart)
        {
            var response = _packageRepository.GetPackagesIDs(sessionId, idStart);
            return response;
        }
        public PackageResponse GetPackage(Guid sessionId, int packageId)
        {
            var response = _packageRepository.GetPackage(sessionId, packageId);
            return response;
        }
        public LabelResponse GetConsignLabels(Guid sessionId, List<int> packageIds, string mode)
        {
            var response = _packageRepository.GetConsignLabels(sessionId, packageIds, mode);
            return response;
        }
    }
}
