using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibreriaJoseAntonio.Startup))]
namespace LibreriaJoseAntonio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
