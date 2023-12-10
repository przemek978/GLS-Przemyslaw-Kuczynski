namespace GlsAPI.Models.Requests
{
    public class LabelsRequest
    {
        public Guid sessionId { get; set; }
        public List<int> packageIds { get; set; }
        public string mode { get; set; }
    }
}
