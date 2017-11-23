using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.Class
{
    public class DisplayTasksViewModel : BaseModelPager
    {
        public List<TaskLog> TaskList { get; set; }

        public string Pattern { get; set; }


        public DisplayTasksViewModel()
        {
            TaskList = new List<TaskLog>();
        }
    }
}
