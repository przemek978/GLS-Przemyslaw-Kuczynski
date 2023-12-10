using Google.Protobuf.Compiler;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json.Nodes;
using Triggers.Data;
using Triggers.Dto;

namespace Triggers.Services
{
    public class DataService : IDataService
    {
        private IRestClient _restClient;
        private readonly ILogger _logger;
        private readonly DBContext _dbContext;
        private string apiUrl = "https://localhost:44339/api/";
        private string printerUrl = "https://localhost:44304/print";

        public DataService(ILoggerFactory loggerFactory, DBContext dBContext)
        {
            _restClient = new RestClient(apiUrl);
            _logger = loggerFactory.CreateLogger<DataService>();
            _dbContext = dBContext;
        }

        public async Task<Guid> Login()
        {
            try
            {
                var data = new
                {
                    username = "User1",
                    password = "User123#"
                };
                string jsonData = JsonConvert.SerializeObject(data);
                var request = new RestRequest("User/adeLogin", Method.Post);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(data);

                var response = await _restClient.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Zalogowano");
                    SessionDto sessionId = JsonConvert.DeserializeObject<SessionDto>(response.Content);
                    return Guid.TryParse(sessionId.session, out Guid id) == true ? id : Guid.Empty;
                }
                else
                {
                    _logger.LogInformation($"Błąd w żądaniu. Kod odpowiedzi: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Błąd w trakcie wysyłania żądania: " + ex.Message);
            }
            return Guid.Empty;
        }

        public async Task<List<string>> GetPackageIds(Guid session, int id_start)
        {
            try
            {
                var request = new RestRequest($"Package/adePreparingBox_GetConsignIDs?session={session}&id_start={id_start}", Method.Get);

                var response = await _restClient.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("");
                    var data = JsonConvert.DeserializeObject<PackageDto>(response.Content);
                    return data.package_id;

                }
                else
                {
                    _logger.LogInformation($"Błąd w żądaniu. Kod odpowiedzi: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Błąd w trakcie wysyłania żądania: " + ex.Message);
            }
            return null;
        }

        public async Task<List<string>> GetLabels(Guid sessionId, List<int> packageIds, string mode)
        {
            try
            {
                var data = new
                {
                    sessionId = sessionId,
                    packageIds = packageIds,
                    mode = mode
                };
                string jsonData = JsonConvert.SerializeObject(data);
                var request = new RestRequest($"Package/adePreparingBox_GetConsignsLabels", Method.Post);
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(data);
                var response = await _restClient.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("");
                    var deserializeData = JsonConvert.DeserializeObject<PackageDto>(response.Content);
                    return deserializeData.labels;

                }
                else
                {
                    _logger.LogInformation($"Błąd w żądaniu. Kod odpowiedzi: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Błąd w trakcie wysyłania żądania: " + ex.Message);
            }
            return null;
        }

        public void InsertLabels(List<int> ids, List<string> labels)
        {
            var labelsFromDb = _dbContext.Labels.ToList();
            foreach (var label in labels)
            {
                bool labelExists = labelsFromDb.Any(dbLabel => dbLabel.LabelFile == label);
                if (!labelExists)
                {
                    _dbContext.Labels.Add(new Models.Label { LabelFile = label });
                }
            }
            _dbContext.SaveChanges();
        }

        public bool LabelCheck(int labeledId)
        {
            var label = _dbContext.Labels.FirstOrDefault(l => l.Id == labeledId);
            if (label != null)
            {
                label.IsPrinted = true;
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            var request = new RestRequest("User/adeLogout", Method.Post);

            var response = await _restClient.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                _logger.LogInformation("Wylogowano");
            }
            else
            {
                _logger.LogInformation($"Błąd w żądaniu. Kod odpowiedzi: {response.StatusCode}");
            }
        }
    }
}
