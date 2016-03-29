namespace RepoOrchestrator.Models
{
    public class EventRegistration
    {
        public string VsoInstance { get; set; }
        public string VsoProject { get; set; }
        public int BuildDefinitionId { get; set; }
        public string VsoParameters { get; set; }
    }
}