using RepoOrchestrator.Models;
using RepoOrchestrator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace RepoOrchestrator.Controllers
{
    public class CommitPushedController : ApiController
    {
        private EventRegistrationTable _eventService = new EventRegistrationTable();
        private VsoService _vsoService = new VsoService();

        public async Task Post(PushWebHookEvent e)
        {
            Trace.TraceInformation($"CommitPushed Started: {e}");

            try
            {
                if (e.commits != null)
                {
                    foreach (Commit c in e.commits)
                    {
                        if (c.modified != null)
                        {
                            foreach (string modified in c.modified)
                            {
                                IndexModel indexModel;
                                if (!IndexModel.TryParse(modified, out indexModel))
                                {
                                    Trace.TraceWarning($"Skipping file '{modified}'");
                                    continue;
                                }

                                List<Task> queueBuildTasks = new List<Task>();
                                foreach (EventRegistration registration in _eventService.GetRegistrations(indexModel))
                                {
                                    queueBuildTasks.Add(
                                        _vsoService.QueueBuildAsync(
                                            registration.VsoInstance,
                                            registration.VsoProject,
                                            registration.BuildDefinitionId));
                                }

                                await Task.WhenAll(queueBuildTasks);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"An unhandled exception occurred handling PushWebHookEvent {e} : Exception: {ex}");

                throw;
            }

            Trace.TraceInformation("CommitPushed Complete");
        }
    }
}
