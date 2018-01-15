using Microsoft.Owin;
using Owin;
using System.Transactions;
using System;
using Commons;
using System.Threading.Tasks;
using Service;

[assembly: OwinStartupAttribute(typeof(Template.Startup))]
namespace Template
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            Task.Factory.StartNew(() => ScheduledTaskService.SetTasks());


        }


    }
}


