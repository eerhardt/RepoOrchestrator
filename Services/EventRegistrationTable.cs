using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RepoOrchestrator.Models;

namespace RepoOrchestrator.Services
{
    public class EventRegistrationTable
    {
        private const string SubscriptionPartitionKey = "Subscriptions";

        private CloudTable _subscriptionsTable;

        public EventRegistrationTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["EventSubscriptionsConnectionString"]);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            _subscriptionsTable = tableClient.GetTableReference("RepoSubscriptions");
        }

        public IEnumerable<EventRegistration> GetRegistrations(IndexModel indexModel)
        {
            return _subscriptionsTable.CreateQuery<RepoSubscription>()
                .Where(s => s.PartitionKey == SubscriptionPartitionKey &&
                    s.RepoPath == indexModel.RepoPath &&
                    s.BranchPath == indexModel.BranchPath &&
                    s.IndexType == indexModel.IndexType)
                .Select(s => new EventRegistration()
                {
                    Index = indexModel,
                    VsoInstance = s.VsoInstance,
                    VsoProject = s.VsoProject,
                    BuildDefinitionId = s.BuildDefinitionId,
                });
        }

        private class RepoSubscription : TableEntity
        {
            public RepoSubscription()
            {
            }

            public string RepoPath { get; set; }
            public string BranchPath { get; set; }
            public string IndexType { get; set; }

            public string VsoInstance { get; set; }
            public string VsoProject { get; set; }
            public string BuildDefinitionId { get; set; }
        }
    }
}