namespace RepoOrchestrator.Models
{
    public class EventRegistration
    {
        public IndexModel Index { get; set; }

        public string VsoInstance { get; set; }
        public string VsoProject { get; set; }
        public string BuildDefinitionId { get; set; }
    }
}