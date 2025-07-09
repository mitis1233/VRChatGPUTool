using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace VRCGPUTool.Form
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.BeginTime = new System.Windows.Forms.DateTimePicker();
            this.EndTime = new System.Windows.Forms.DateTimePicker();
            this.BeginTimeLabel = new System.Windows.Forms.Label();
            this.EndTimeLabel = new System.Windows.Forms.Label();
            this.TimeSetGroupBox = new System.Windows.Forms.GroupBox();
            this.PowerLogShow = new System.Windows.Forms.Button();
            this.SettingButton = new System.Windows.Forms.Button();
            this.GPUCoreTemp = new System.Windows.Forms.Label();
            this.GPUTotalPower = new System.Windows.Forms.Label();
            this.GPUCorePLValue = new System.Windows.Forms.Label();
            this.GPUCoreClockValue = new System.Windows.Forms.Label();
            this.GPUMemoryClockValue = new System.Windows.Forms.Label();
            this.GPUStatusGroup = new System.Windows.Forms.GroupBox();
            this.PLimitLabel = new System.Windows.Forms.Label();
            this.PowerLimitValue = new System.Windows.Forms.NumericUpDown();
            this.PLwattLabel1 = new System.Windows.Forms.Label();
            this.LoadMinimumLimit = new System.Windows.Forms.Button();
            this.LoadDefaultLimit = new System.Windows.Forms.Button();
            this.LoadMaximumLimit = new System.Windows.Forms.Button();
            this.ResetBehavior = new System.Windows.Forms.Label();
            this.ResetGPUDefaultPL = new System.Windows.Forms.RadioButton();
            this.SetGPUPLSpecific = new System.Windows.Forms.RadioButton();
            this.SpecificPLValue = new System.Windows.Forms.NumericUpDown();
            this.PLwattLabel2 = new System.Windows.Forms.Label();
            this.PLimitGroup = new System.Windows.Forms.GroupBox();
            this.button200 = new System.Windows.Forms.Button();
            this.button150 = new System.Windows.Forms.Button();
            this.button135 = new System.Windows.Forms.Button();
            this.LimitStatusText = new System.Windows.Forms.Label();
            this.ForceUnlimit = new System.Windows.Forms.Button();
            this.ForceLimit = new System.Windows.Forms.Button();
            this.AutoDetect = new System.Windows.Forms.CheckBox();
            this.ThresholdLabel = new System.Windows.Forms.Label();
            this.GPUusageThreshold = new System.Windows.Forms.NumericUpDown();
            this.PercentLabel = new System.Windows.Forms.Label();
            this.CoreLimitEnable = new System.Windows.Forms.CheckBox();
            this.CoreClockSetting = new System.Windows.Forms.NumericUpDown();
            this.FreqLabel = new System.Windows.Forms.Label();
            this.BetaGroup = new System.Windows.Forms.GroupBox();
            this.limitime = new System.Windows.Forms.CheckBox();
            this.GpuIndex = new System.Windows.Forms.ComboBox();
            this.GPUreadTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MainWindowOpenStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowVersionInfoStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ApplicationExitStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.TimeSetGroupBox.SuspendLayout();
            this.GPUStatusGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerLimitValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecificPLValue)).BeginInit();
            this.PLimitGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GPUusageThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoreClockSetting)).BeginInit();
            this.BetaGroup.SuspendLayout();
            this.notifyIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // BeginTime
            // 
            this.BeginTime.CustomFormat = "H:mm";
            this.BeginTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.BeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BeginTime.Location = new System.Drawing.Point(223, 9);
            this.BeginTime.Name = "BeginTime";
            this.BeginTime.ShowUpDown = true;
            this.BeginTime.Size = new System.Drawing.Size(96, 38);
            this.BeginTime.TabIndex = 0;
            this.BeginTime.ValueChanged += new System.EventHandler(this.SettingTimeChange);
            // 
            // EndTime
            // 
            this.EndTime.CustomFormat = "H:mm";
            this.EndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndTime.Location = new System.Drawing.Point(223, 48);
            this.EndTime.Name = "EndTime";
            this.EndTime.ShowUpDown = true;
            this.EndTime.Size = new System.Drawing.Size(96, 38);
            this.EndTime.TabIndex = 1;
            this.EndTime.ValueChanged += new System.EventHandler(this.SettingTimeChange);
            // 
            // BeginTimeLabel
            // 
            this.BeginTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.BeginTimeLabel.Location = new System.Drawing.Point(6, 15);
            this.BeginTimeLabel.Name = "BeginTimeLabel";
            this.BeginTimeLabel.Size = new System.Drawing.Size(210, 30);
            this.BeginTimeLabel.TabIndex = 2;
            this.BeginTimeLabel.Text = "限制開始時間";
            // 
            // EndTimeLabel
            // 
            this.EndTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.EndTimeLabel.Location = new System.Drawing.Point(6, 54);
            this.EndTimeLabel.Name = "EndTimeLabel";
            this.EndTimeLabel.Size = new System.Drawing.Size(211, 30);
            this.EndTimeLabel.TabIndex = 3;
            this.EndTimeLabel.Text = "限制結束時間";
            // 
            // TimeSetGroupBox
            // 
            this.TimeSetGroupBox.Controls.Add(this.EndTime);
            this.TimeSetGroupBox.Controls.Add(this.BeginTimeLabel);
            this.TimeSetGroupBox.Controls.Add(this.BeginTime);
            this.TimeSetGroupBox.Controls.Add(this.EndTimeLabel);
            this.TimeSetGroupBox.Location = new System.Drawing.Point(10, 2);
            this.TimeSetGroupBox.Name = "TimeSetGroupBox";
            this.TimeSetGroupBox.Size = new System.Drawing.Size(330, 94);
            this.TimeSetGroupBox.TabIndex = 0;
            this.TimeSetGroupBox.TabStop = false;
            this.TimeSetGroupBox.Text = "時間設定";
            // 
            // PowerLogShow
            // 
            this.PowerLogShow.Location = new System.Drawing.Point(427, 307);
            this.PowerLogShow.Name = "PowerLogShow";
            this.PowerLogShow.Size = new System.Drawing.Size(100, 25);
            this.PowerLogShow.TabIndex = 4;
            this.PowerLogShow.Text = "用電量歷史";
            this.PowerLogShow.Click += new System.EventHandler(this.PowerLogShow_Click);
            // 
            // SettingButton
            // 
            this.SettingButton.Location = new System.Drawing.Point(533, 307);
            this.SettingButton.Name = "SettingButton";
            this.SettingButton.Size = new System.Drawing.Size(100, 25);
            this.SettingButton.TabIndex = 5;
            this.SettingButton.Text = "設定";
            this.SettingButton.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // GPUCoreTemp
            // 
            this.GPUCoreTemp.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.GPUCoreTemp.Location = new System.Drawing.Point(10, 25);
            this.GPUCoreTemp.Name = "GPUCoreTemp";
            this.GPUCoreTemp.Size = new System.Drawing.Size(300, 35);
            this.GPUCoreTemp.TabIndex = 4;
            this.GPUCoreTemp.Text = "GPUコア温度: 0℃";
            // 
            // GPUTotalPower
            // 
            this.GPUTotalPower.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.GPUTotalPower.Location = new System.Drawing.Point(10, 60);
            this.GPUTotalPower.Name = "GPUTotalPower";
            this.GPUTotalPower.Size = new System.Drawing.Size(300, 35);
            this.GPUTotalPower.TabIndex = 3;
            this.GPUTotalPower.Text = "GPU全体電力: 0W";
            // 
            // GPUCorePLValue
            // 
            this.GPUCorePLValue.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.GPUCorePLValue.Location = new System.Drawing.Point(10, 95);
            this.GPUCorePLValue.Name = "GPUCorePLValue";
            this.GPUCorePLValue.Size = new System.Drawing.Size(300, 35);
            this.GPUCorePLValue.TabIndex = 2;
            this.GPUCorePLValue.Text = "GPUコア電力制限: 0W";
            // 
            // GPUCoreClockValue
            // 
            this.GPUCoreClockValue.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.GPUCoreClockValue.Location = new System.Drawing.Point(10, 130);
            this.GPUCoreClockValue.Name = "GPUCoreClockValue";
            this.GPUCoreClockValue.Size = new System.Drawing.Size(300, 35);
            this.GPUCoreClockValue.TabIndex = 1;
            this.GPUCoreClockValue.Text = "GPUコアクロック: 0MHz";
            // 
            // GPUMemoryClockValue
            // 
            this.GPUMemoryClockValue.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold);
            this.GPUMemoryClockValue.Location = new System.Drawing.Point(10, 165);
            this.GPUMemoryClockValue.Name = "GPUMemoryClockValue";
            this.GPUMemoryClockValue.Size = new System.Drawing.Size(350, 35);
            this.GPUMemoryClockValue.TabIndex = 0;
            this.GPUMemoryClockValue.Text = "GPUメモリクロック: 0MHz";
            // 
            // GPUStatusGroup
            // 
            this.GPUStatusGroup.Controls.Add(this.GPUMemoryClockValue);
            this.GPUStatusGroup.Controls.Add(this.GPUCoreClockValue);
            this.GPUStatusGroup.Controls.Add(this.GPUCorePLValue);
            this.GPUStatusGroup.Controls.Add(this.GPUTotalPower);
            this.GPUStatusGroup.Controls.Add(this.GPUCoreTemp);
            this.GPUStatusGroup.Location = new System.Drawing.Point(10, 94);
            this.GPUStatusGroup.Name = "GPUStatusGroup";
            this.GPUStatusGroup.Size = new System.Drawing.Size(330, 200);
            this.GPUStatusGroup.TabIndex = 6;
            this.GPUStatusGroup.TabStop = false;
            this.GPUStatusGroup.Text = "GPU 狀態";
            // 
            // PLimitLabel
            // 
            this.PLimitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.PLimitLabel.Location = new System.Drawing.Point(10, 20);
            this.PLimitLabel.Name = "PLimitLabel";
            this.PLimitLabel.Size = new System.Drawing.Size(172, 30);
            this.PLimitLabel.TabIndex = 0;
            this.PLimitLabel.Text = "限制瓦數";
            // 
            // PowerLimitValue
            // 
            this.PowerLimitValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.PowerLimitValue.Location = new System.Drawing.Point(188, 18);
            this.PowerLimitValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PowerLimitValue.Name = "PowerLimitValue";
            this.PowerLimitValue.Size = new System.Drawing.Size(90, 35);
            this.PowerLimitValue.TabIndex = 6;
            this.PowerLimitValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PowerLimitValue.ValueChanged += new System.EventHandler(this.PowerLimitSettingChanged);
            // 
            // PLwattLabel1
            // 
            this.PLwattLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.PLwattLabel1.Location = new System.Drawing.Point(282, 28);
            this.PLwattLabel1.Name = "PLwattLabel1";
            this.PLwattLabel1.Size = new System.Drawing.Size(35, 22);
            this.PLwattLabel1.TabIndex = 13;
            this.PLwattLabel1.Text = "W";
            //this.PLwattLabel1.Click += new System.EventHandler(this.PLwattLabel1_Click);
            // 
            // LoadMinimumLimit
            // 
            this.LoadMinimumLimit.Location = new System.Drawing.Point(15, 101);
            this.LoadMinimumLimit.Name = "LoadMinimumLimit";
            this.LoadMinimumLimit.Size = new System.Drawing.Size(70, 25);
            this.LoadMinimumLimit.TabIndex = 7;
            this.LoadMinimumLimit.Text = "最小値";
            this.LoadMinimumLimit.Click += new System.EventHandler(this.LoadMinimumLimit_Click);
            // 
            // LoadDefaultLimit
            // 
            this.LoadDefaultLimit.Location = new System.Drawing.Point(15, 130);
            this.LoadDefaultLimit.Name = "LoadDefaultLimit";
            this.LoadDefaultLimit.Size = new System.Drawing.Size(70, 25);
            this.LoadDefaultLimit.TabIndex = 8;
            this.LoadDefaultLimit.Text = "默認";
            this.LoadDefaultLimit.Click += new System.EventHandler(this.LoadDefaultLimit_Click);
            // 
            // LoadMaximumLimit
            // 
            this.LoadMaximumLimit.Location = new System.Drawing.Point(15, 161);
            this.LoadMaximumLimit.Name = "LoadMaximumLimit";
            this.LoadMaximumLimit.Size = new System.Drawing.Size(70, 25);
            this.LoadMaximumLimit.TabIndex = 9;
            this.LoadMaximumLimit.Text = "最大値";
            this.LoadMaximumLimit.Click += new System.EventHandler(this.LoadMaximumLimit_Click);
            // 
            // ResetBehavior
            // 
            this.ResetBehavior.Location = new System.Drawing.Point(12, 60);
            this.ResetBehavior.Name = "ResetBehavior";
            this.ResetBehavior.Size = new System.Drawing.Size(49, 17);
            this.ResetBehavior.TabIndex = 14;
            this.ResetBehavior.Text = "解除時";
            // 
            // ResetGPUDefaultPL
            // 
            this.ResetGPUDefaultPL.Checked = true;
            this.ResetGPUDefaultPL.Location = new System.Drawing.Point(62, 58);
            this.ResetGPUDefaultPL.Name = "ResetGPUDefaultPL";
            this.ResetGPUDefaultPL.Size = new System.Drawing.Size(140, 20);
            this.ResetGPUDefaultPL.TabIndex = 11;
            this.ResetGPUDefaultPL.TabStop = true;
            this.ResetGPUDefaultPL.Text = "恢復 GPU 默認值";
            // 
            // SetGPUPLSpecific
            // 
            this.SetGPUPLSpecific.Location = new System.Drawing.Point(62, 79);
            this.SetGPUPLSpecific.Name = "SetGPUPLSpecific";
            this.SetGPUPLSpecific.Size = new System.Drawing.Size(140, 20);
            this.SetGPUPLSpecific.TabIndex = 10;
            this.SetGPUPLSpecific.Text = "設置為指定值";
            this.SetGPUPLSpecific.CheckedChanged += new System.EventHandler(this.SetGPUPLSpecific_CheckedChanged);
            // 
            // SpecificPLValue
            // 
            this.SpecificPLValue.Enabled = false;
            this.SpecificPLValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.SpecificPLValue.Location = new System.Drawing.Point(188, 78);
            this.SpecificPLValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SpecificPLValue.Name = "SpecificPLValue";
            this.SpecificPLValue.Size = new System.Drawing.Size(55, 21);
            this.SpecificPLValue.TabIndex = 12;
            this.SpecificPLValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SpecificPLValue.ValueChanged += new System.EventHandler(this.SpecificPLValue_ValueChanged);
            // 
            // PLwattLabel2
            // 
            this.PLwattLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.PLwattLabel2.Location = new System.Drawing.Point(249, 84);
            this.PLwattLabel2.Name = "PLwattLabel2";
            this.PLwattLabel2.Size = new System.Drawing.Size(20, 15);
            this.PLwattLabel2.TabIndex = 15;
            this.PLwattLabel2.Text = "W";
            // 
            // PLimitGroup
            // 
            this.PLimitGroup.Controls.Add(this.button200);
            this.PLimitGroup.Controls.Add(this.button150);
            this.PLimitGroup.Controls.Add(this.button135);
            this.PLimitGroup.Controls.Add(this.PLimitLabel);
            this.PLimitGroup.Controls.Add(this.SpecificPLValue);
            this.PLimitGroup.Controls.Add(this.PLwattLabel1);
            this.PLimitGroup.Controls.Add(this.LoadMinimumLimit);
            this.PLimitGroup.Controls.Add(this.LoadDefaultLimit);
            this.PLimitGroup.Controls.Add(this.LoadMaximumLimit);
            this.PLimitGroup.Controls.Add(this.ResetBehavior);
            this.PLimitGroup.Controls.Add(this.LimitStatusText);
            this.PLimitGroup.Controls.Add(this.ForceUnlimit);
            this.PLimitGroup.Controls.Add(this.ForceLimit);
            this.PLimitGroup.Controls.Add(this.ResetGPUDefaultPL);
            this.PLimitGroup.Controls.Add(this.SetGPUPLSpecific);
            this.PLimitGroup.Controls.Add(this.PowerLimitValue);
            this.PLimitGroup.Controls.Add(this.PLwattLabel2);
            this.PLimitGroup.Location = new System.Drawing.Point(346, 2);
            this.PLimitGroup.Name = "PLimitGroup";
            this.PLimitGroup.Size = new System.Drawing.Size(317, 299);
            this.PLimitGroup.TabIndex = 7;
            this.PLimitGroup.TabStop = false;
            this.PLimitGroup.Text = "核心功耗限制";
            // 
            // button200
            // 
            this.button200.Location = new System.Drawing.Point(15, 254);
            this.button200.Name = "button200";
            this.button200.Size = new System.Drawing.Size(70, 25);
            this.button200.TabIndex = 22;
            this.button200.Text = "550";
            this.button200.Click += new System.EventHandler(this.button200_Click);
            // 
            // button150
            // 
            this.button150.Location = new System.Drawing.Point(15, 223);
            this.button150.Name = "button150";
            this.button150.Size = new System.Drawing.Size(70, 25);
            this.button150.TabIndex = 21;
            this.button150.Text = "500";
            this.button150.Click += new System.EventHandler(this.button150_Click);
            // 
            // button135
            // 
            this.button135.Location = new System.Drawing.Point(15, 192);
            this.button135.Name = "button135";
            this.button135.Size = new System.Drawing.Size(70, 25);
            this.button135.TabIndex = 20;
            this.button135.Text = "450";
            this.button135.Click += new System.EventHandler(this.button135_Click);
            // 
            // LimitStatusText
            // 
            this.LimitStatusText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LimitStatusText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LimitStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold);
            this.LimitStatusText.ForeColor = System.Drawing.Color.Red;
            this.LimitStatusText.Location = new System.Drawing.Point(166, 240);
            this.LimitStatusText.Name = "LimitStatusText";
            this.LimitStatusText.Size = new System.Drawing.Size(127, 41);
            this.LimitStatusText.TabIndex = 19;
            this.LimitStatusText.Text = "限制中";
            this.LimitStatusText.Visible = false;
            // 
            // ForceUnlimit
            // 
            this.ForceUnlimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.ForceUnlimit.Location = new System.Drawing.Point(93, 172);
            this.ForceUnlimit.Name = "ForceUnlimit";
            this.ForceUnlimit.Size = new System.Drawing.Size(200, 65);
            this.ForceUnlimit.TabIndex = 18;
            this.ForceUnlimit.Text = "解除限制";
            this.ForceUnlimit.Click += new System.EventHandler(this.ForceUnlimit_Click);
            // 
            // ForceLimit
            // 
            this.ForceLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.ForceLimit.Location = new System.Drawing.Point(93, 101);
            this.ForceLimit.Name = "ForceLimit";
            this.ForceLimit.Size = new System.Drawing.Size(200, 65);
            this.ForceLimit.TabIndex = 17;
            this.ForceLimit.Text = "開始限制";
            this.ForceLimit.Click += new System.EventHandler(this.ForceLimit_Click);
            // 
            // AutoDetect
            // 
            this.AutoDetect.Font = new System.Drawing.Font("Gadugi", 9F);
            this.AutoDetect.Location = new System.Drawing.Point(10, 15);
            this.AutoDetect.Name = "AutoDetect";
            this.AutoDetect.Size = new System.Drawing.Size(75, 20);
            this.AutoDetect.TabIndex = 13;
            this.AutoDetect.Text = "自動偵測";
            // 
            // ThresholdLabel
            // 
            this.ThresholdLabel.Font = new System.Drawing.Font("Gadugi", 9F);
            this.ThresholdLabel.Location = new System.Drawing.Point(28, 58);
            this.ThresholdLabel.Name = "ThresholdLabel";
            this.ThresholdLabel.Size = new System.Drawing.Size(43, 19);
            this.ThresholdLabel.TabIndex = 17;
            this.ThresholdLabel.Text = "臨界點";
            // 
            // GPUusageThreshold
            // 
            this.GPUusageThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.GPUusageThreshold.Location = new System.Drawing.Point(105, 56);
            this.GPUusageThreshold.Name = "GPUusageThreshold";
            this.GPUusageThreshold.Size = new System.Drawing.Size(40, 21);
            this.GPUusageThreshold.TabIndex = 14;
            this.GPUusageThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.GPUusageThreshold.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // PercentLabel
            // 
            this.PercentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.PercentLabel.Location = new System.Drawing.Point(145, 58);
            this.PercentLabel.Name = "PercentLabel";
            this.PercentLabel.Size = new System.Drawing.Size(20, 15);
            this.PercentLabel.TabIndex = 18;
            this.PercentLabel.Text = "%";
            //this.PercentLabel.Click += new System.EventHandler(this.PercentLabel_Click);
            // 
            // CoreLimitEnable
            // 
            this.CoreLimitEnable.Font = new System.Drawing.Font("Gadugi", 9F);
            this.CoreLimitEnable.Location = new System.Drawing.Point(10, 35);
            this.CoreLimitEnable.Name = "CoreLimitEnable";
            this.CoreLimitEnable.Size = new System.Drawing.Size(100, 20);
            this.CoreLimitEnable.TabIndex = 15;
            this.CoreLimitEnable.Text = "核心頻率限制";
            // 
            // CoreClockSetting
            // 
            this.CoreClockSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.CoreClockSetting.Location = new System.Drawing.Point(105, 34);
            this.CoreClockSetting.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.CoreClockSetting.Minimum = new decimal(new int[] {
            210,
            0,
            0,
            0});
            this.CoreClockSetting.Name = "CoreClockSetting";
            this.CoreClockSetting.Size = new System.Drawing.Size(60, 21);
            this.CoreClockSetting.TabIndex = 16;
            this.CoreClockSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CoreClockSetting.Value = new decimal(new int[] {
            210,
            0,
            0,
            0});
            // 
            // FreqLabel
            // 
            this.FreqLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FreqLabel.Location = new System.Drawing.Point(171, 36);
            this.FreqLabel.Name = "FreqLabel";
            this.FreqLabel.Size = new System.Drawing.Size(37, 19);
            this.FreqLabel.TabIndex = 1;
            this.FreqLabel.Text = "MHz";
            // 
            // BetaGroup
            // 
            this.BetaGroup.Controls.Add(this.limitime);
            this.BetaGroup.Controls.Add(this.FreqLabel);
            this.BetaGroup.Controls.Add(this.CoreClockSetting);
            this.BetaGroup.Controls.Add(this.CoreLimitEnable);
            this.BetaGroup.Controls.Add(this.ThresholdLabel);
            this.BetaGroup.Controls.Add(this.PercentLabel);
            this.BetaGroup.Controls.Add(this.GPUusageThreshold);
            this.BetaGroup.Controls.Add(this.AutoDetect);
            this.BetaGroup.Location = new System.Drawing.Point(10, 297);
            this.BetaGroup.Name = "BetaGroup";
            this.BetaGroup.Size = new System.Drawing.Size(216, 82);
            this.BetaGroup.TabIndex = 8;
            this.BetaGroup.TabStop = false;
            this.BetaGroup.Text = "額外功能";
            // 
            // limitime
            // 
            this.limitime.Font = new System.Drawing.Font("Gadugi", 9F);
            this.limitime.Location = new System.Drawing.Point(105, 15);
            this.limitime.Name = "limitime";
            this.limitime.Size = new System.Drawing.Size(91, 20);
            this.limitime.TabIndex = 19;
            this.limitime.Text = "Time limit";
            this.limitime.CheckedChanged += new System.EventHandler(this.limitime_CheckedChanged);
            // 
            // GpuIndex
            // 
            this.GpuIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GpuIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.GpuIndex.FormattingEnabled = true;
            this.GpuIndex.Location = new System.Drawing.Point(353, 346);
            this.GpuIndex.Name = "GpuIndex";
            this.GpuIndex.Size = new System.Drawing.Size(280, 28);
            this.GpuIndex.TabIndex = 19;
            this.GpuIndex.SelectedIndexChanged += new System.EventHandler(this.SelectGPUChanged);
            // 
            // GPUreadTimer
            // 
            this.GPUreadTimer.Interval = 1000;
            this.GPUreadTimer.Tick += new System.EventHandler(this.GPUreadTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "VRChatGPUTool";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainWindowOpenStrip,
            this.ShowVersionInfoStrip,
            this.ApplicationExitStrip});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(171, 70);
            // 
            // MainWindowOpenStrip
            // 
            this.MainWindowOpenStrip.Name = "MainWindowOpenStrip";
            this.MainWindowOpenStrip.Size = new System.Drawing.Size(170, 22);
            this.MainWindowOpenStrip.Text = "メインウィンドウ";
            this.MainWindowOpenStrip.Click += new System.EventHandler(this.MainWindowOpenStrip_Click);
            // 
            // ShowVersionInfoStrip
            // 
            this.ShowVersionInfoStrip.Name = "ShowVersionInfoStrip";
            this.ShowVersionInfoStrip.Size = new System.Drawing.Size(170, 22);
            this.ShowVersionInfoStrip.Text = "バージョン情報";
            this.ShowVersionInfoStrip.Click += new System.EventHandler(this.ShowVersionInfoStrip_Click);
            // 
            // ApplicationExitStrip
            // 
            this.ApplicationExitStrip.Name = "ApplicationExitStrip";
            this.ApplicationExitStrip.Size = new System.Drawing.Size(170, 22);
            this.ApplicationExitStrip.Text = "終了";
            this.ApplicationExitStrip.Click += new System.EventHandler(this.ApplicationExitStrip_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 377);
            this.Controls.Add(this.TimeSetGroupBox);
            this.Controls.Add(this.PowerLogShow);
            this.Controls.Add(this.SettingButton);
            this.Controls.Add(this.GPUStatusGroup);
            this.Controls.Add(this.PLimitGroup);
            this.Controls.Add(this.BetaGroup);
            this.Controls.Add(this.GpuIndex);
            this.Font = new System.Drawing.Font("Gadugi", 8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VRChat向け GPU電力制限ツール Ver ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            //this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.TimeSetGroupBox.ResumeLayout(false);
            this.GPUStatusGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PowerLimitValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpecificPLValue)).EndInit();
            this.PLimitGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GPUusageThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CoreClockSetting)).EndInit();
            this.BetaGroup.ResumeLayout(false);
            this.notifyIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Label BeginTimeLabel;
        private Label PLimitLabel;
        private Label PLwattLabel1;
        private Button ForceLimit;
        private Button ForceUnlimit;
        private Timer GPUreadTimer;
        private Button LoadDefaultLimit;
        private CheckBox AutoDetect;
        private Label LimitStatusText;
        private Label PercentLabel;
        private Label ThresholdLabel;
        private GroupBox BetaGroup;
        private Label EndTimeLabel;
        private GroupBox TimeSetGroupBox;
        private Button LoadMinimumLimit;
        private Button LoadMaximumLimit;
        private GroupBox GPUStatusGroup;
        private NumericUpDown CoreClockSetting;
        private CheckBox CoreLimitEnable;
        private GroupBox PLimitGroup;
        private Label FreqLabel;
        internal ComboBox GpuIndex;
        internal Label GPUCoreTemp;
        internal Label GPUTotalPower;
        internal Label GPUCorePLValue;
        internal Label GPUMemoryClockValue;
        internal Label GPUCoreClockValue;
        private Label PLwattLabel2;
        private Label ResetBehavior;
        internal NumericUpDown PowerLimitValue;
        internal DateTimePicker BeginTime;
        internal DateTimePicker EndTime;
        internal NumericUpDown SpecificPLValue;
        internal RadioButton SetGPUPLSpecific;
        internal RadioButton ResetGPUDefaultPL;
        private Button PowerLogShow;
        internal NumericUpDown GPUusageThreshold;
        private Button SettingButton;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip notifyIconMenu;
        private ToolStripMenuItem MainWindowOpenStrip;
        private ToolStripMenuItem ShowVersionInfoStrip;
        private ToolStripMenuItem ApplicationExitStrip;
        private Button button135;
        private Button button150;
        private Button button200;
        private CheckBox limitime;
    }
}
