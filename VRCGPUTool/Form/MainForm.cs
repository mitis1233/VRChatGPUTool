using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Threading.Tasks;
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

        private readonly NvidiaSmi nvsmi;

        private readonly AutoLimit autoLimit;
        private PowerHistory history;
        internal GPUPowerLog gpuPlog;
        private PowerLogFile powerLogFile;
        internal List<GpuStatus> gpuStatuses = [];

        internal bool limitstatus = false;

        internal string guid = "";
        private bool ignoreTimeCheck = false;
        internal int limittime = 0;
        private DateTime datetime_now = DateTime.Now;

        private bool isFetchingGpuStatus = false;

        private async void MainForm_Load(object sender, EventArgs _)
        {
            Icon appIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            Icon = appIcon;
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Text += fileVersionInfo.ProductVersion;

            NvidiaSmi.CheckNvidiaSmi();
            await nvsmi.InitGPUAsync();

            LoadConfig();
            LoadPowerLog();

            if (gpuStatuses.Count != 0)
            {
                GpuStatus firstGpu = gpuStatuses.First();
                SpecificPLValue.Value = Convert.ToDecimal(firstGpu.PLimit);
                PowerLimitValue.Value = Convert.ToDecimal(firstGpu.PLimitMin);
                GPUCorePLValue.Text = $"GPU限制:        {firstGpu.PLimit}W";
            }

            GPUreadTimer.Interval = 1000; // 1 second
            GPUreadTimer.Enabled = true;

            BeginTime.Enabled = limitime.Checked;
            EndTime.Enabled = limitime.Checked;
        }

        internal async Task Limit_Action(bool limit, bool expection)
        {
            GpuStatus g = gpuStatuses[GpuIndex.SelectedIndex];
            if (limit)
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

                if (CoreLimitEnable.Checked)
                {
                    await NvidiaSmi.NvidiaSmiCommandAsync($"-lgc 210,{CoreClockSetting.Value} --id={g.UUID}");
                }

                await NvidiaSmi.NvidiaSmiCommandAsync($"-pl {PowerLimitValue.Value} --id={g.UUID}");
                GPUCorePLValue.Text = $"GPU限制:        {PowerLimitValue.Value}W";
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

                if (CoreLimitEnable.Checked)
                {
                    await NvidiaSmi.NvidiaSmiCommandAsync($"-rgc --id={g.UUID}");
                }

                if (!expection)
                {
                    if (ResetGPUDefaultPL.Checked)
                    {
                        await NvidiaSmi.NvidiaSmiCommandAsync($"-pl {g.PLimitDefault} --id={g.UUID}");
                        GPUCorePLValue.Text = $"GPU限制:        {g.PLimitDefault}W";
                    }
                    else
                    {
                        await NvidiaSmi.NvidiaSmiCommandAsync($"-pl {SpecificPLValue.Value} --id={g.UUID}");
                        GPUCorePLValue.Text = $"GPU限制:        {SpecificPLValue.Value}W";
                    }
                }
            }
        }

        private async void ForceLimit_Click(object sender, EventArgs _)
        {
            if (datetime_now.Hour == EndTime.Value.Hour && datetime_now.Minute == EndTime.Value.Minute)
            {
                var res = MessageBox.Show("由於限制結束時間與目前時間相同，無法開始限制\n是否要透過強制變更結束時間來開始限制？", "資訊", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    ignoreTimeCheck = true;
                    EndTime.Value = DateTime.Now.AddMinutes(60);
                    await Limit_Action(true, false);
                }
            }
            else
            {
                await Limit_Action(true, false);
            }
        }

        private async void ForceUnlimit_Click(object sender, EventArgs _)
        {
            if (datetime_now.Hour == BeginTime.Value.Hour && datetime_now.Minute == BeginTime.Value.Minute)
            {
                var res = MessageBox.Show("由於限制的開始時間與目前時間相同，無法解除限制。\n是否要透過強制變更開始時間來解除限制？", "資訊", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (res == DialogResult.OK)
                {
                    ignoreTimeCheck = true;
                    BeginTime.Value = DateTime.Now.AddMinutes(60);
                    await Limit_Action(false, false);
                }
            }
            else
            {
                await Limit_Action(false, false);
            }
        }

        private void SelectGPUChanged(object sender, EventArgs _)
        {
            GpuStatus g = gpuStatuses[GpuIndex.SelectedIndex];
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimit);
            GPUCorePLValue.Text = "GPU限制:        " + g.PLimit + "W";
        }

        private async void GPUreadTimer_Tick(object sender, EventArgs e)
        {
            if (this.Visible == false || this.WindowState == FormWindowState.Minimized || isFetchingGpuStatus)
            {
                return;
            }

            isFetchingGpuStatus = true;
            try
            {
                await nvsmi.FetchGpuStatusAsync();

                if (AutoDetect.Checked && limitstatus)
                {
                    try
                    {
                        if (autoLimit.CheckAutoLimit(gpuStatuses[GpuIndex.SelectedIndex]))
                        {
                            await Limit_Action(true, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"自動限制檢查時發生錯誤: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                datetime_now = DateTime.Now;
                if (limitime.Checked && !ignoreTimeCheck)
                {
                    if (datetime_now.Hour == BeginTime.Value.Hour && datetime_now.Minute == BeginTime.Value.Minute && !limitstatus)
                    {
                        await Limit_Action(true, false);
                    }
                    if (datetime_now.Hour == EndTime.Value.Hour && datetime_now.Minute == EndTime.Value.Minute && limitstatus)
                    {
                        await Limit_Action(false, false);
                    }
                }
            }
            finally
            {
                isFetchingGpuStatus = false;
            }

            if (limitstatus)
            {
                limittime++;
            }
            else
            {
                limittime = 0;
            }
        }

        private void PowerLimitSettingChanged(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            if (PowerLimitValue.Value > g.PLimitMax)
            {
                MessageBox.Show("功率限制超出可設定範圍。\n" + g.Name + " 的最大功率限制為 " + g.PLimitMax + "W。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PowerLimitValue.Value = g.PLimitMax;
            }
            if (PowerLimitValue.Value < g.PLimitMin)
            {
                MessageBox.Show("功率限制超出可設定範圍。\n" + g.Name + " 的最小功率限制為 " + g.PLimitMin + "W。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PowerLimitValue.Value = g.PLimitMin;
            }
        }

        private void SpecificPLValue_ValueChanged(object sender, EventArgs e)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            if (SpecificPLValue.Value > g.PLimitMax)
            {
                MessageBox.Show("功率限制超出可設定範圍。\n" + g.Name + " 的最大功率限制為 " + g.PLimitMax + "W。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpecificPLValue.Value = g.PLimitMax;
            }
            if (SpecificPLValue.Value < g.PLimitMin)
            {
                MessageBox.Show("功率限制超出可設定範圍。\n" + g.Name + " 的最小功率限制為 " + g.PLimitMin + "W。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpecificPLValue.Value = g.PLimitMin;
            }
        }


        private void SetGPUPLSpecific_CheckedChanged(object sender, EventArgs _)
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

        private void LoadMinimumLimit_Click(object sender, EventArgs _)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitMin);
        }

        private void LoadDefaultLimit_Click(object sender, EventArgs _)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitDefault);
        }

        private void LoadMaximumLimit_Click(object sender, EventArgs _)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            PowerLimitValue.Value = Convert.ToDecimal(g.PLimitMax);
        }

        private async void AppClosing(object sender, FormClosingEventArgs _)
        {
            if (limitstatus == true)
            {
                await Limit_Action(false, false);
            }

            ConfigFile config = new(this);
            await config.SaveConfigAsync();

            await powerLogFile.SavePowerLogAsync();

            notifyIcon.Dispose();
        }

        private async void ResetClockSetting_Click(object sender, EventArgs _)
        {
            GpuStatus g = gpuStatuses.ElementAt(GpuIndex.SelectedIndex);
            await NvidiaSmi.NvidiaSmiCommandAsync($"-rgc --id={g.UUID}");
            MessageBox.Show("已將時脈限制重設為預設值", "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowHowToUse(object sender, EventArgs _)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://github.com/njm2360/VRChatGPUTool#readme", UseShellExecute = true });
        }

        private async void SettingTimeChange(object sender, EventArgs _)
        {
            if ((BeginTime.Value.Hour == EndTime.Value.Hour) && (BeginTime.Value.Minute == EndTime.Value.Minute))
            {
                if (!ignoreTimeCheck)
                {
                    if (limitstatus)
                    {
                        await Limit_Action(false, false);
                    }
                    BeginTime.Value = BeginTime.Value.AddMinutes(15);
                    MessageBox.Show("開始時間和結束時間不能設定為同一時間", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ignoreTimeCheck = false;
                }
            }
        }



        private void PowerLogShow_Click(object sender, EventArgs _)
        {
            if ((history == null) || history.IsDisposed)
            {
                history = new PowerHistory(this);
                history.Show();
            }
        }

        private async void LoadConfig()
        {
            ConfigFile config = new(this);
            await config.LoadConfigAsync();
        }

        private async void LoadPowerLog()
        {
            powerLogFile = new PowerLogFile(gpuPlog);
            await powerLogFile.LoadPowerLogAsync(DateTime.Now, false);
        }

        private void SettingButton_Click(object sender, EventArgs _)
        {
            SettingForm fm = new(this);
            fm.ShowDialog();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs _)
        {
            if (this.Visible == false || this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;

                notifyIcon.Visible = false;
            }
        }

        private void MainWindowOpenStrip_Click(object sender, EventArgs _)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;

            notifyIcon.Visible = false;
        }

        private void ShowVersionInfoStrip_Click(object sender, EventArgs _)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionInfo = fileVersionInfo.ProductVersion;    

            MessageBox.Show("Version v" + versionInfo + "\n\nCopyright njm2360 Allrights reserved 2022" , "版本資訊");
        }

        private void ApplicationExitStrip_Click(object sender, EventArgs _)
        {
            Application.Exit();
        }

        private void Button135_Click(object sender, EventArgs _)
        {
            PowerLimitValue.Value = 450;
        }

        private void Button150_Click(object sender, EventArgs _)
        {
            PowerLimitValue.Value = 500;
        }

        private void Button200_Click(object sender, EventArgs _)
        {
            PowerLimitValue.Value = 550;
        }

        private void Limitime_CheckedChanged(object sender, EventArgs _)
        {
            BeginTime.Enabled = limitime.Checked;
            EndTime.Enabled = limitime.Checked;
        }

        internal void UpdateGpuInfoUI(GpuStatus g)
        {
            GPUCoreTemp.Text = $"GPU核心:        {g.CoreTemp}°C";
            GPUTotalPower.Text = $"GPU功耗:        {g.PowerDraw}W";
            GPUCorePLValue.Text = $"GPU限制:        {g.PLimit}W";
            GPUCoreClockValue.Text = $"GPU頻率:        {g.CoreClock}MHz";
            GPUMemoryClockValue.Text = $"VRAM頻率:     {g.MemoryClock}MHz";
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GPUreadTimer.Enabled)
            {
                GPUreadTimer.Enabled = false;
                await Task.Delay(500);
            }
            notifyIcon.Visible = false;
            
            // Save power log when application is closing
            PowerLogFile logfile = new PowerLogFile(gpuPlog);
            await logfile.SavePowerLogAsync();
            
            Application.Exit();
        }
    }
}
