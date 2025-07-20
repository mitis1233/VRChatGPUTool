using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRCGPUTool.Util;

namespace VRCGPUTool.Form
{
    public partial class SettingForm : System.Windows.Forms.Form
    {
        MainForm fm;
        StartupTask startupTask;

        public SettingForm(MainForm　fm)
        {
            InitializeComponent();
            this.fm = fm;

            startupTask = new StartupTask();
        }

        private async void ConfigFileRecreate_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                "設定ファイルを削除してよろしいですか\n" +
                "削除すると保存している情報が失われます\n" +
                "※削除するとアプリが終了します",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if(res == DialogResult.Yes)
            {
                await Task.Run(() => File.Delete("config.json"));
                Environment.Exit(0);
            }
        }

        private async void PriceSettingRecreate_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                "電気代設定ファイルを削除してよろしいですか\n" +
                "削除すると保存している情報が失われます\n" +
                "※削除するとアプリが終了します",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (res == DialogResult.Yes)
            {
                await Task.Run(() => File.Delete("profile.json"));
                Environment.Exit(0);
            }
        }

        private async void UsageLogDelete_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(
                "電力使用履歴ファイルを削除してよろしいですか\n" +
                "削除すると保存している情報が失われます\n" +
                "※削除するとアプリが終了します",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (res == DialogResult.Yes)
            {
                await Task.Run(() => 
                {
                    if (Directory.Exists(PathUtil.LogDirectory))
                    {
                        Directory.Delete(PathUtil.LogDirectory, true);
                    }
                    Directory.CreateDirectory(PathUtil.LogDirectory);
                });
                Environment.Exit(0);
            }
        }


        private void RegisterStartup_Click(object sender, EventArgs e)
        {
            StartupTask.RegisterTask();
        }

        private void DeleteStartup_Click(object sender, EventArgs e)
        {
            StartupTask.RemoveTask();
        }
    }
}
