using Microsoft.Playwright;
using System.Threading.Tasks;
using System;
using DemoTestProject.UI.PageObjects;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DemoTestProject.UI
{
    [Binding]
    public sealed class UISteps
    {
        public string WebUrl { get; private set; }

        private readonly Driver driver;

        private readonly AvailabilitiesPage availabilitiesPage;

        private readonly WithinDayPage withinDayPage;

        private readonly Navigation navigation;

        public UISteps(
            Driver driver,
            AvailabilitiesPage availabilitiesPage,
            WithinDayPage withinDayPage,
            Navigation navigation)
        {
            this.driver = driver;
            this.availabilitiesPage = availabilitiesPage;
            this.withinDayPage = withinDayPage;
            this.navigation = navigation;
        }

        [Given(@"we open UI")]
        public void GivenWeOpenUI()
        {
            this.WebUrl = new Uri("this.config.WebUrl").AbsoluteUri;
        }

        [Given(@"we navigate to the Asset Availabilities page")]
        public async Task GivenWeNavigateToTheAssetAvailabilitiesPage()
        {
            await this.driver.Page.GotoAsync(this.WebUrl + this.navigation.AvailabilitiesUrl);
            await this.driver.Page.GetByText("TestContract").ClickAsync();
        }

        [Given(@"we update all the day ahead Asset Availability")]
        public async Task GivenWeUpdateAllTheDayAheadAssetAvailability()
        {
            await this.InputAvailabilityDataForAllRows(this.availabilitiesPage.AvailabilityValue, "99", 0);

            await this.driver.Page.GetByRole(AriaRole.Alert).ClickAsync();

            await this.InputAvailabilityDataForAllRows(this.availabilitiesPage.EmbeddedBenefits, "2.5", 1);
        }

        [Given(@"we update the Asset Availability for a single row")]
        public async Task GivenWeUpdateTheAssetAvailabilityForASingleRow()
        {
            await this.InputAvailabilityDataForSingleRow(this.availabilitiesPage.AvailabilityValue, "88");
            await this.InputAvailabilityDataForSingleRow(this.availabilitiesPage.EmbeddedBenefits, "2.5");
        }

        [When(@"we save the changes")]
        public async Task WhenWeSaveTheChanges()
        {
            await this.driver.Page.GetByText(this.availabilitiesPage.SaveButton).ClickAsync();
        }

        [Then(@"a success message will be displayed")]
        public async Task ThenASuccessMessageWillBeDisplayed()
        {
            var successMessage = await this.driver.Page.GetByRole(AriaRole.Alert).TextContentAsync();
            successMessage.Should().Be("Asset Availabilities successfully received");

            await this.driver.Page.GetByRole(AriaRole.Alert).ClickAsync();
        }

        [Then(@"we navigate to the Within Day page")]
        public async Task ThenWeNavigateToTheWithinDayPage()
        {
            await this.driver.Page.GetByRole(AriaRole.Link, this.navigation.WithinDayPage).ClickAsync();
            await this.driver.Page.GetByText("TestContract").ClickAsync();
            await this.driver.Page.GetByText(this.withinDayPage.EditButton).ClickAsync();
        }

        [Then(@"Refresh the data")]
        public async Task ThenRefreshTheData()
        {
            await this.driver.Page.GetByTitle("Confirm Refresh").GetByText("Yes").ClickAsync();
            await this.driver.Page.GetByText(this.withinDayPage.RefreshButton).ClickAsync();

            var refreshMessage = await this.driver.Page.GetByRole(AriaRole.Alert).First.TextContentAsync();
            refreshMessage.Should().Be("Data refreshed successfully.");
        }

        [Then(@"the Within Day Availability will be updated")]
        public async Task ThenTheWithinDayAvailabilityWillBeUpdated()
        {
            await this.driver.Page.GetByText(this.availabilitiesPage.RefreshButton).ClickAsync();
            await this.RefreshTableIfNoData(" 99 ");

            for (int i = 0; i < 48; i++)
            {
                var value = await this.driver.Page.Locator(this.withinDayPage.Availability).Nth(i).TextContentAsync();

                value.Should().Be(" 99 ");
            }
        }

        [Then(@"the Within Day Availability for a single row will be updated")]
        public async Task ThenTheWithinDayAvailabilityForASingleRowWillBeUpdated()
        {
            await this.driver.Page.GetByText(this.availabilitiesPage.RefreshButton).ClickAsync();
            await this.RefreshTableIfNoData(" 88 ");

            var value = await this.driver.Page.Locator(this.withinDayPage.Availability).Nth(46).TextContentAsync();
            value.Should().Be(" 88 ");
        }

        private async Task InputAvailabilityDataForAllRows(string field, string value, int index)
        {
            await this.driver.Page.Locator(field).First.ClickAsync();
            await this.driver.Page.Locator(field).First.TypeAsync(value);

            await this.driver.Page.HoverAsync(field);
            await this.driver.Page.GetByTitle(this.availabilitiesPage.CopyDownArrow).Nth(index).ClickAsync();
        }

        private async Task InputAvailabilityDataForSingleRow(string field, string value)
        {
            var count = await this.driver.Page.Locator(field).CountAsync();
            var tenPmRow = count - 50;

            await this.driver.Page.Locator(field).Nth(tenPmRow).ClickAsync();
            await this.driver.Page.Locator(field).Nth(tenPmRow).TypeAsync(value);
        }

        private async Task RefreshTableIfNoData(string value)
        {
            for (int i = 0; i < 10; i++)
            {
                var residualValue = await this.driver.Page.Locator(this.withinDayPage.Residual).Nth(46).TextContentAsync();

                if (residualValue == "  ")
                {
                    await this.driver.Page.GetByText(this.availabilitiesPage.RefreshButton).ClickAsync();
                }
                if (residualValue == value)
                {
                    break;
                }
            }
        }
    }
}