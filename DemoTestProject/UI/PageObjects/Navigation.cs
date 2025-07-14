using Microsoft.Playwright;

namespace DemoTestProject.UI.PageObjects
{
    public class Navigation
    {
        public string AvailabilitiesUrl => "/assets/availabilities";

        public PageGetByRoleOptions WithinDayPage => new() { Name = "Within Day" };
    }
}