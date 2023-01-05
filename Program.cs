using System;
using System.Threading;
using System.Windows.Forms;
using VRCGPUTool.Form;

namespace VRCGPUTool
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string mutexName = "VRChatGPUTool";
            Mutex mutex = new Mutex(true, mutexName, out bool createdNew);
            if (createdNew)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                finally
                {
                    mutex.ReleaseMutex();
                    mutex.Close();
                }
            }
            else
            {
                MessageBox.Show("檢測到重複啟動", "錯誤", MessageBoxButtons.OK,MessageBoxIcon.Error);
                mutex.Close();
            }
        }
    }
}
