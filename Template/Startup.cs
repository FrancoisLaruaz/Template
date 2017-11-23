using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Template.Startup))]
namespace Template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}