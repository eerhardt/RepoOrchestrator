using Newtonsoft.Json;
using System.Collections.Generic;

namespace RepoOrchestrator.Models
{
    public class PushWebHookEvent
    {
        public List<Commit> commits { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}