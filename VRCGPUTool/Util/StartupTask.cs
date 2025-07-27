using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace VRCGPUTool.Util
{
    internal class StartupTask
    {
        private const string TASK_NAME = "VRChatGPUTool";
        private const string AUTHOR = "njm2360";
        private const string DESCRIPTION = "";

        public static void RegisterTask()
        {
            try
            {
                using TaskService ts = new();
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Author = AUTHOR;
                td.RegistrationInfo.Description = DESCRIPTION;

                td.Principal.UserId = $@"{Environment.UserDomainName}\{Environment.UserName}";
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Principal.RunLevel = TaskRunLevel.Highest;

                LogonTrigger lt = new()
                {
                    UserId = $@"{Environment.UserDomainName}\{Environment.UserName}"
                };
                td.Triggers.Add(lt);

                td.Settings.ExecutionTimeLimit = TimeSpan.Zero;
                td.Settings.DisallowStartIfOnBatteries = true;
                td.Settings.Priority = System.Diagnostics.ProcessPriorityClass.BelowNormal;

                td.Actions.Add(new ExecAction(Application.ExecutablePath, null, Path.GetDirectoryName(Application.ExecutablePath)));

                ts.RootFolder.RegisterTaskDefinition(TASK_NAME, td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"錯誤\n{ex.Message}");
            }
        }

        public static void RemoveTask()
        {
            try
            {
                using TaskService ts = new();
                ts.RootFolder.DeleteTask(TASK_NAME);
            }
            catch
            {
                //未登録
            }
        }
    }
}
