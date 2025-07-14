using System.ComponentModel;

namespace DemoTestProject.Context
{
    public enum Controllers
    {
        [Description("application/ping")]
        Ping,

        [Description("application/submit")]
        Submit
    }
}
