using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Scheduler_Project.Startup))]
namespace Scheduler_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}