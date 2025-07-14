using System.Net.Http;
using System.Text;
using DemoTestProject.DTO;
using FluentAssertions;
using Newtonsoft.Json;

namespace DemoTestProject.Context
{
    public class ApiContext
    {
        public HttpResponseMessage LastApiResponse { get; private set; }

        public HttpClient CreateHttpConnector()
        {
            return new HttpClient();
        }

        public ApiContext Ping()
        {
            this.Get(this.BuildApiUrl(Controllers.Ping.ToString()));

            return this;
        }

        public ApiContext SubmitData(DataSubmission data)
        {
            var json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = this.CreateHttpConnector())
            {
                this.LastApiResponse =
                    httpClient.PostAsync(this.BuildApiUrl(Controllers.Submit.ToString()), httpContent).Result;
            }

            return this;
        }

        public void AssertLastApiCallWasSuccessful()
        {
            this.LastApiResponse.Should().Be("200");
        }

        private void Get(string route)
        {
            using (var client = this.CreateHttpConnector())
            {
                this.LastApiResponse = client.GetAsync(route).Result;
            }
        }

        private string BuildApiUrl(string controller)
        {
            return $"www.test/{controller}.com";
        }
    }
}
