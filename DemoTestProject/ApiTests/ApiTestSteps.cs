using DemoTestProject.Context;
using DemoTestProject.DTO;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace DemoTestProject.ApiTests
{
    [Binding]
    public sealed class ApiTestSteps
    {
        public DataSubmission DataSubmission { get; private set; }

        private readonly ApiContext apiContext;

        public ApiTestSteps(ApiContext apiContext)
        {
            this.apiContext = apiContext;
        }

        [Given(@"we have data to submit")]
        public void GivenWeHaveDataToSubmit()
        {
            this.DataSubmission = new DataSubmission
                { Address = "Test Address", Name = "Aaron", Age = 37, IsCustomer = false };
        }

        [When(@"the Test Application Server is pinged")]
        public void WhenTheTestApplicationServerIsPinged()
        {
            this.apiContext.Ping();
        }

        [When(@"submit the data")]
        public void WhenSubmitTheData()
        {
            this.apiContext.SubmitData(this.DataSubmission);
        }

        [Then(@"the api call is successful")]
        public void ThenTheApiCallIsSuccessful()
        {
            this.apiContext.AssertLastApiCallWasSuccessful();
        }

        [Then(@"the system info is returned")]
        public void ThenTheSystemInfoIsReturned()
        {
            var expectedSystemInfo = new SystemInfoDto
            {
                ApplicationName = "TestApplication",
                EnvironmentName = "Test",
                IsProd = false
            };

            var actualSystemInfo = this.apiContext.LastApiResponse.Content;

            var deserializeObject = JsonConvert.DeserializeObject(actualSystemInfo.ToString());

            deserializeObject.Should().BeEquivalentTo(expectedSystemInfo,
                x => x.Excluding(b => b.BuildName));
        }

        [Then(@"the data is returned")]
        public void ThenTheDataIsReturned()
        {
            var expectedDataSubmission = new DataSubmission
            {
                Name = this.DataSubmission.Name,
                Address = this.DataSubmission.Address,
                Age = this.DataSubmission.Age,
                IsCustomer = this.DataSubmission.IsCustomer
            };

            var actualDataSubmission = this.apiContext.LastApiResponse.Content;

            var deserializeObject = JsonConvert.DeserializeObject(actualDataSubmission.ToString());

            deserializeObject.Should().BeEquivalentTo(expectedDataSubmission);
        }
    }
}