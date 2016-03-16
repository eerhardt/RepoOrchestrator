using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RepoOrchestrator.Services
{
    public class VsoService
    {
        private static HttpClient client = new HttpClient();
        private const string apiVersion = "2.0";

        static VsoService()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
               Convert.ToBase64String(
                   Encoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", ConfigurationManager.AppSettings["VsoUser"], ConfigurationManager.AppSettings["VsoPassword"]))));
        }

        public async Task QueueBuildAsync(string instance, string project, string buildDefinitionId)
        {
            string queueBuildUrl = $"https://{instance}/defaultcollection/{project}/_apis/build/builds?api-version={apiVersion}";

            string queueBuildBody = @"{
    ""definition"": {
        ""id"": " + buildDefinitionId + @"
    }
}";

            StringContent queueBuildContent = new StringContent(queueBuildBody);
            queueBuildContent.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = await client.PostAsync(queueBuildUrl, queueBuildContent);
            if (!response.IsSuccessStatusCode)
            {
                Trace.TraceError($"Error queuing VSO build to '{queueBuildUrl}'\nBody: {queueBuildBody}\n\nResponse StatusCode: {response.StatusCode}\nResponse Body: {await response.Content.ReadAsStringAsync()}");
            }
            else
            {
                Trace.TraceInformation($"Successfully queued VSO build.{Environment.NewLine}Response Body: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}