using Commons;
using DataAccess;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;

namespace Service.Admin.Interface
{
    public interface ILogService
    {
        LogsViewModel GetLogsViewModel();

        bool DeleteLogs();

        DisplayLogsViewModel GetDisplayLogsViewModel(string Pattern, int StartAt, int PageSize);

    }
}
