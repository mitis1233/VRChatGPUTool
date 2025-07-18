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

        public void registerTask()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Author = AUTHOR;
                    td.RegistrationInfo.Description = DESCRIPTION;

                    td.Principal.UserId = $@"{Environment.UserDomainName}\{Environment.UserName}";
                    td.Principal.LogonType = TaskLogonType.InteractiveToken;
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                    LogonTrigger lt = new LogonTrigger();
                    lt.UserId = $@"{Environment.UserDomainName}\{Environment.UserName}";
                    td.Triggers.Add(lt);

                    td.Settings.ExecutionTimeLimit = TimeSpan.Zero;
                    td.Settings.DisallowStartIfOnBatteries = true;
                    td.Settings.Priority = System.Diagnostics.ProcessPriorityClass.BelowNormal;

                    td.Actions.Add(new ExecAction(Application.ExecutablePath, null, Path.GetDirectoryName(Application.ExecutablePath)));

                    ts.RootFolder.RegisterTaskDefinition(TASK_NAME, td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"錯誤\n{ex.Message}");
            }
        }

        public void removeTask()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    ts.RootFolder.DeleteTask(TASK_NAME);
                }
            }
            catch
            {
                //未登録
            }
        }
    }
}
