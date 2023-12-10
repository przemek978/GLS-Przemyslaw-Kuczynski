using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;
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
        private string printerUrl = "https://localhost:44304/";

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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public void InsertLabels(List<int> ids, List<string> labels)
        {
            var labelsFromDb = _dbContext.Labels.ToList();
            for (int i =0; i<labels.Count; i++)
            {
                bool labelExists = labelsFromDb.Any(dbLabel => dbLabel.PackageId == ids[i]);
                if (!labelExists)
                {
                    _dbContext.Labels.Add(new Models.Label {PackageId = ids[i], LabelFile = labels[i] });
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

        public string Print()
        {
            try
            {
                int sendCount = 0;
                var labels = _dbContext.Labels.Where(l => !l.IsPrinted).Take(10).ToList();
                int labelsCount = labels.Count();
                if (labels.Any())
                {
                    using (var printerClient = new RestClient(printerUrl))
                    {
                        foreach (var label in labels)
                        {
                            using (var labelStream = new MemoryStream(Encoding.UTF8.GetBytes(label.LabelFile)))
                            {
                                var labelFile = new FormFile(labelStream, 0, labelStream.Length, "file", $"label {label.PackageId}.pdf");

                                var request = new RestRequest("print", Method.Post);
                                request.AddFile("file", labelStream.ToArray(), "label.pdf");

                                var response = printerClient.Execute(request);
                                if (response.IsSuccessful)
                                {
                                    label.IsPrinted = true;
                                    sendCount++;
                                }
                                else
                                {
                                    _logger.LogInformation($"Błąd w żądaniu. Kod odpowiedzi: {response.StatusCode}");
                                }
                            }
                        }
                        _logger.LogInformation($"Wydrukowano {sendCount} z {labelsCount}");
                        _dbContext.SaveChanges();
                        return $"Wydrukowano {sendCount} z {labelsCount}";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return "Nie pobrano żadnej etykiety";

        }
    }
}
