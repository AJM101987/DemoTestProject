using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace DemoTestProject.UI
{
    public class Driver : IDisposable
    {
        private readonly Task<IPage> page;

        #nullable enable
        private IBrowser? browser;

        public Driver()
        {
            this.page = this.InitialisePlaywright();
        }

        public IPage Page => this.page.Result;

        public async Task<IPage> InitialisePlaywright()
        {
            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = true,
                Args = new List<string> { "--start-maximized" },
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                SlowMo = 500
            };

            var playwright = await Playwright.CreateAsync();

            this.browser = await playwright.Chromium.LaunchAsync(launchOptions);

            var context = await this.browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    ViewportSize = ViewportSize.NoViewport
                });

            return await context.NewPageAsync();
        }

        public void Dispose()
        {
            this.browser?.CloseAsync();
        }
    }
}