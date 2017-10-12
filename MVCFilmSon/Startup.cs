using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCFilmSon.Startup))]
namespace MVCFilmSon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
