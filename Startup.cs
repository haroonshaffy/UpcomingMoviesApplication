using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UpcomingMoviesApplication.Startup))]
namespace UpcomingMoviesApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
