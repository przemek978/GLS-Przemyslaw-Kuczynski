using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Triggers.Services
{
    public interface IDataService
    {
        Task<Guid> Login();
        Task<List<string>> GetPackageIds(Guid session, int id_start);
        Task<List<string>> GetLabels(Guid sessionId, List<int> packageIds, string mode);
        void InsertLabels(List<int> ids, List<string> labels);
        bool LabelCheck(int labeledId);
        string Print();
        Task Logout();
    }
}
