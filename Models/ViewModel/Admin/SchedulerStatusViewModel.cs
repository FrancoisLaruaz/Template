using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Revalee;

namespace Models.Class
{
    public class SchedulerStatusViewModel : IAdminViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Is Scheduler Active")]
        public bool IsSchedulerActive { get; set; }

        [Display(Name = "Scheduled Task Number")]
        public int ScheduledTaskNumber { get; set; }

        public List<Revalee.Client.RecurringTasks.IRecurringTask> RevaleeTasksScheduled { get; set; }

        public SchedulerStatusViewModel()
        {
            RevaleeTasksScheduled = new List<Revalee.Client.RecurringTasks.IRecurringTask>();
        }
    }
}
