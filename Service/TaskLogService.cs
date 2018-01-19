﻿using Commons;
using DataAccess;
using Models.BDDObject;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Models.ViewModels;

namespace Service
{
    public static class TaskLogService
    {


        public static DisplayTasksViewModel GetDisplayTasksViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayTasksViewModel model = new DisplayTasksViewModel();
            try
            {
                model.TaskList = TaskLogDAL.GetTaskLogsList(null,Pattern, StartAt, PageSize);
                model.Pattern = Pattern;
                model.Count = TaskLogDAL.GetTaskLogsCount(Pattern);
                model.StartAt = StartAt;
                model.PageSize = PageSize;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }


        /// <summary>
        ///  Get the logs of the scheduled tasks    
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static List<TaskLog> GetTaskLogsListByGroupName(string GroupName)
        {
            List<TaskLog> Result = new List<TaskLog>();
            try
            {
                Result = TaskLogDAL.GetTaskLogsList(GroupName);
            }
            catch(Exception e)
            {

                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "GroupName = " + GroupName);
            }
            return Result;
        }

        /// <summary>
        /// Insert a task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static int InsertTaskLog(TaskLog Task)
        {
            int Result = -1;
            try
            {
                Dictionary<string, Object> Columns = new Dictionary<string, Object>();
                Columns.Add("GroupName", Task.GroupName);
                Columns.Add("CallbackId", Task.CallbackId);
                Columns.Add("StartDate", DateTime.UtcNow);
                Result = GenericDAL.InsertRow("tasklog", Columns);
            }
            catch (Exception e)
            {
                Result = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Result;
        }


        /// <summary>
        /// Update a specified task
        /// </summary>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static bool UpdateTaskLog(TaskLog Task)
        {
            bool Result = false;
            try
            {
                Result = TaskLogDAL.UpdateTaskLog(Task);
            }
            catch (Exception e)
            {
                Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Task.Id);
            }
            return Result;
        }

    }
}