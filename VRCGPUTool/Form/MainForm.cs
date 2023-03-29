using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using VRCGPUTool.Util;

namespace VRCGPUTool.Form
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();

            nvsmi = new NvidiaSmi(this);
            gpuPlog = new GPUPowerLog();
            autoLimit = new AutoLimit(this);
            notifyIcon.Visible = false;
        }

        private NvidiaSmi nvsmi;

        private AutoLimit autoLimit;
        private PowerHistory history;
        internal GPUPowerLog gpuPlog;
        internal List<GpuStatus> gpuStatuses = new List<GpuStatus>();

        internal bool limitstatus = false;
        internal bool allowDataProvide = false;
        internal string guid = "";
        private bool ignoreTimeCheck = false;
        internal int limittime = 0;
        private DateTime datetime_now = DateTime.Now;

        private void MainForm_Load(object sender, EventArgs e)
        {
            Icon appIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Icon = appIcon;
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Text += fileVersionInfo.ProductVersion;

            nvsmi.CheckNvidiaSmi();
            nvsmi.InitGPU();



            ConfigFile config = new ConfigFile(this);
            config.LoadConfig();

            PowerLogFile plog = new PowerLogFile(gpuPlog);
            Directory.CreateDirectory("powerlog");
            plog.LoadPowerLog(DateTime.Now, false);

            SpecificPLValue.Value = Convert.ToDecimal(gpuStatuses.First().PLimit);
            PowerLimitValue.Value = Convert.ToDecimal(150);
            GPUCorePLValue.Text = "GPUコア電力制限: " + gpuStatuses.First().PLimit.ToString() + "W";


            GPUreadTimer.Enabled = true;
        }

        internal void Limit_Action(bool limit, bool expection)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            if (limit == true)
            {
                limitstatus = true;
                LimitStatusText.Visible = true;
                PowerLimitValue.Enabled = false;
                BeginTime.Enabled = false;
                EndTime.Enabled = false;
                LoadDefaultLimit.Enabled = false;
                LoadMaximumLimit.Enabled = false;
                LoadMinimumLimit.Enabled = false;
                ForceLimit.Enabled = false;
                CoreLimitEnable.Enabled = false;
                CoreClockSetting.Enabled = false;
                GpuIndex.Enabled = false;
                SettingButton.Enabled = false;
                button135.Enabled = false;
                button150.Enabled = false;
                button200.Enabled = false;

                if (CoreLimitEnable.Checked == true)
                {
                    nvsmi.nvidia_smi("-lgc 210," + CoreClockSetting.Value + "--id=" + g.UUID);
                }

                nvsmi.nvidia_smi("-pl " + PowerLimitValue.Value.ToString() + " --id=" + g.UUID);
                GPUCorePLValue.Text = "GPUコア電力制限: " + PowerLimitValue.Value.ToString() + "W";
            }
            else
            {
                limitstatus = false;
                LimitStatusText.Visible = false;
                PowerLimitValue.Enabled = true;
                BeginTime.Enabled = true;
                EndTime.Enabled = true;
                LoadDefaultLimit.Enabled = true;
                LoadMaximumLimit.Enabled = true;
                LoadMinimumLimit.Enabled = true;
                ForceLimit.Enabled = true;
                CoreLimitEnable.Enabled = true;
                CoreClockSetting.Enabled = true;
                GpuIndex.Enabled = true;
                SettingButton.Enabled = true;
                button135.Enabled = true;
                button150.Enabled = true;
                button200.Enabled = true;

                if (CoreLimitEnable.Checked == true)
                {
                    nvsmi.nvidia_smi("-rgc --id=" + g.UUID);
                }

                if (expection == false)
                {
                    if (ResetGPUDefaultPL.Checked == true)
                    {
                        nvsmi.nvidia_smi("-pl " + g.PLimitDefault.ToString() + " --id=" + g.UUID);
                        GPUCorePLValue.Text = "GPUコア電力制限: " + g.PLimitDefault.ToString() + "W";
                    }
                    else
                    {
                        nvsmi.nvidia_smi("-pl " + SpecificPLValue.Value.ToString() + " --id=" + g.UUID);
                        GPUCorePLValue.Text = "GPUコア電力制限: " + SpecificPLValue.Value.ToString() + "W";
                    }
                }
            }
        }

        private void ForceLimit_Click(object sender, EventArgs e)
        {
            if (datetime_now.Hour == EndTime.Value.Hour && datetime_now.Minute == EndTime.Value.Minute)
            {
                var res = MessageBox.Show("由於限制結束時間與當前時間相同，無法開始限制\n是否要通過強制更改結束時間來開始限制？", "情報", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    ignoreTimeCheck = true;
                    EndTime.Value = DateTime.Now.AddMinutes(60);
                    Limit_Action(true, false);
                }
            }
            Limit_Action(true, false);
        }

        private void ForceUnlimit_Click(object sender, EventArgs e)
        {
            if (datetime_now.Hour == BeginTime.Value.Hour && datetime_now.Minute == BeginTime.Value.Minute)
            {
                var res = MessageBox.Show("由於限制的開始時間與當前時間相同，無法取消限制。\n是否要通過強制更改開始時間來取消限制？", "情報", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    ignoreTimeCheck = true;
                    BeginTime.Value = DateTime.Now.AddMinutes(60);
                    Limit_Action(false, false);
                }
            }
            Limit_Action(false, false);
        }

        private void SelectGPUChanged(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimit);
            GPUCorePLValue.Text = "GPUコア電力制限: " + g.PLimit + "W";
        }

        private void GPUreadTimer_Tick(object sender, EventArgs e)
        {
            if(nvsmi.NvsmiWorker.IsBusy == false)
            {
                nvsmi.NvsmiWorker.RunWorkerAsync();
            }

            if (limitstatus)
            {
                limittime++;
            }
            else
            {
                limittime = 0;
            }

            if (AutoDetect.Checked == true && limitstatus == true)
            {
                if (autoLimit.CheckAutoLimit(gpuStatuses.ElementAt(GpuIndex.SelectedIndex)))
                {
                    Limit_Action(true, false);
                }
            }

            datetime_now = DateTime.Now;
            if (datetime_now.Hour == BeginTime.Value.Hour && datetime_now.Minute == BeginTime.Value.Minute && !limitstatus)
            {
                Limit_Action(true, false);
            }
            if (datetime_now.Hour == EndTime.Value.Hour && datetime_now.Minute == EndTime.Value.Minute && limitstatus)
            {
                AutoDetect.Checked = false;

                Limit_Action(false, false);
            }
        }

        private void PowerLimitSettingChanged(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            if (PowerLimitValue.Value > g.PLimitMax)
            {
                MessageBox.Show("功率限制超出可配置範圍。\n" + g.Name + "の最大功率限制" + g.PLimitMax + "Wです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PowerLimitValue.Value = g.PLimitMax;
            }
            if (PowerLimitValue.Value < g.PLimitMin)
            {
                MessageBox.Show("功率限制超出可配置範圍。\n" + g.Name + "の最小功率限制" + g.PLimitMin + "Wです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PowerLimitValue.Value = g.PLimitMin;
            }
        }

        private void SpecificPLValue_ValueChanged(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            if (SpecificPLValue.Value > g.PLimitMax)
            {
                MessageBox.Show("功率限制超出可配置範圍。\n" + g.Name + "の最大功率限制" + g.PLimitMax + "Wです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpecificPLValue.Value = g.PLimitMax;
            }
            if (SpecificPLValue.Value < g.PLimitMin)
            {
                MessageBox.Show("功率限制超出可配置範圍。\n" + g.Name + "の最小功率限制" + g.PLimitMin + "Wです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpecificPLValue.Value = g.PLimitMin;
            }
        }


        private void SetGPUPLSpecific_CheckedChanged(object sender, EventArgs e)
        {
            if (SetGPUPLSpecific.Checked == true)
            {
                SpecificPLValue.Enabled = true;
            }
            else
            {
                SpecificPLValue.Enabled = false;
            }
        }

        private void LoadMinimumLimit_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitMin);
        }

        private void LoadDefaultLimit_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitDefault);
        }

        private void LoadMaximumLimit_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitMax);
        }

        private void AppClosing(object sender, FormClosingEventArgs e)
        {
            if (limitstatus == true)
            {
                Limit_Action(false, false);
            }

            ConfigFile config = new ConfigFile(this);
            config.SaveConfig();

            PowerLogFile plog = new PowerLogFile(gpuPlog);
            plog.SavePowerLog(false);
        }

        private void ResetClockSetting_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            nvsmi.nvidia_smi("-rgc --id=" + g.UUID);
            MessageBox.Show("將時鐘限制設置為默認值", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowHowToUse(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/njm2360/VRChatGPUTool#readme", UseShellExecute = true });
        }

        private void SettingTimeChange(object sender, EventArgs e)
        {
            if ((BeginTime.Value.Hour == EndTime.Value.Hour) && (BeginTime.Value.Minute == EndTime.Value.Minute))
            {
                if (!ignoreTimeCheck)
                {
                    if (limitstatus)
                    {
                        Limit_Action(false, false);
                    }
                    BeginTime.Value = BeginTime.Value.AddMinutes(15);
                    MessageBox.Show("開始時間和結束時間不能設置為同一時間", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ignoreTimeCheck = false;
                }
            }
        }

        private void Reporter(object sender, EventArgs e)
        {
            BugReport report = new BugReport(this);
            report.ShowDialog();
        }

        private void PowerLogShow_Click(object sender, EventArgs e)
        {
            if ((history == null) || history.IsDisposed)
            {
                history = new PowerHistory(this);
                history.Show();
            }
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            SettingForm fm = new SettingForm(this);
            fm.ShowDialog();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;

                notifyIcon.Visible = false;
            }
        }

        private void MainWindowOpenStrip_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;

            notifyIcon.Visible = false;
        }

        private void ShowVersionInfoStrip_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionInfo = fileVersionInfo.ProductVersion;    

            MessageBox.Show("Version v" + versionInfo + "\n\nCopyright© njm2360 Allrights reserved 2022" , "版本信息");
        }

        private void ApplicationExitStrip_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
                notifyIcon.Visible = true;
            }
        }

        private void PercentLabel_Click(object sender, EventArgs e)
        {

        }

        private void PLwattLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button135_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(135);
        }

        private void button150_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(150);
        }

        private void button200_Click(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(200);
        }
    }
}
