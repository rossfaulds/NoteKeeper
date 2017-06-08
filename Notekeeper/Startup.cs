using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Notekeeper.Startup))]
namespace Notekeeper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
