using System.Runtime.Serialization;

namespace RepoOrchestrator.Models
{
    public class Repository
    {
        [DataMember(Name = "full_name")]
        public string Full_Name { get; set; }
    }
}