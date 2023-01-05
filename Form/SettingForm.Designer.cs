using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace VRCGPUTool.Form
{
    partial class SettingForm
    {
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.DataProvideAllow = new System.Windows.Forms.CheckBox();
            this.RegisterStartup = new System.Windows.Forms.Button();
            this.DeleteStartup = new System.Windows.Forms.Button();
            this.GeneralGroup = new System.Windows.Forms.GroupBox();
            this.ConfigFileRecreate = new System.Windows.Forms.Button();
            this.PriceSettingRecreate = new System.Windows.Forms.Button();
            this.UsageLogDelete = new System.Windows.Forms.Button();
            this.DataGroup = new System.Windows.Forms.GroupBox();
            this.GeneralGroup.SuspendLayout();
            this.DataGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataProvideAllow
            // 
            this.DataProvideAllow.Location = new System.Drawing.Point(25, 18);
            this.DataProvideAllow.Name = "DataProvideAllow";
            this.DataProvideAllow.Size = new System.Drawing.Size(150, 18);
            this.DataProvideAllow.TabIndex = 0;
            this.DataProvideAllow.Text = "提供使用數據";
            this.DataProvideAllow.CheckedChanged += new System.EventHandler(this.DataProvideAllow_CheckedChanged);
            // 
            // RegisterStartup
            // 
            this.RegisterStartup.Location = new System.Drawing.Point(25, 42);
            this.RegisterStartup.Name = "RegisterStartup";
            this.RegisterStartup.Size = new System.Drawing.Size(150, 18);
            this.RegisterStartup.TabIndex = 1;
            this.RegisterStartup.Text = "開機自動開啟登録";
            this.RegisterStartup.Click += new System.EventHandler(this.RegisterStartup_Click);
            // 
            // DeleteStartup
            // 
            this.DeleteStartup.Location = new System.Drawing.Point(25, 65);
            this.DeleteStartup.Name = "DeleteStartup";
            this.DeleteStartup.Size = new System.Drawing.Size(150, 18);
            this.DeleteStartup.TabIndex = 2;
            this.DeleteStartup.Text = "開機自動開啟解除";
            this.DeleteStartup.Click += new System.EventHandler(this.DeleteStartup_Click);
            // 
            // GeneralGroup
            // 
            this.GeneralGroup.Controls.Add(this.DeleteStartup);
            this.GeneralGroup.Controls.Add(this.RegisterStartup);
            this.GeneralGroup.Controls.Add(this.DataProvideAllow);
            this.GeneralGroup.Location = new System.Drawing.Point(10, 9);
            this.GeneralGroup.Name = "GeneralGroup";
            this.GeneralGroup.Size = new System.Drawing.Size(200, 102);
            this.GeneralGroup.TabIndex = 1;
            this.GeneralGroup.TabStop = false;
            this.GeneralGroup.Text = "一般";
            // 
            // ConfigFileRecreate
            // 
            this.ConfigFileRecreate.Location = new System.Drawing.Point(25, 18);
            this.ConfigFileRecreate.Name = "ConfigFileRecreate";
            this.ConfigFileRecreate.Size = new System.Drawing.Size(150, 18);
            this.ConfigFileRecreate.TabIndex = 3;
            this.ConfigFileRecreate.Text = "設置文件削除";
            this.ConfigFileRecreate.Click += new System.EventHandler(this.ConfigFileRecreate_Click);
            // 
            // PriceSettingRecreate
            // 
            this.PriceSettingRecreate.Location = new System.Drawing.Point(25, 42);
            this.PriceSettingRecreate.Name = "PriceSettingRecreate";
            this.PriceSettingRecreate.Size = new System.Drawing.Size(150, 18);
            this.PriceSettingRecreate.TabIndex = 4;
            this.PriceSettingRecreate.Text = "電費設定削除";
            this.PriceSettingRecreate.Click += new System.EventHandler(this.PriceSettingRecreate_Click);
            // 
            // UsageLogDelete
            // 
            this.UsageLogDelete.Location = new System.Drawing.Point(25, 65);
            this.UsageLogDelete.Name = "UsageLogDelete";
            this.UsageLogDelete.Size = new System.Drawing.Size(150, 18);
            this.UsageLogDelete.TabIndex = 5;
            this.UsageLogDelete.Text = "用電量歷史削除";
            this.UsageLogDelete.Click += new System.EventHandler(this.UsageLogDelete_Click);
            // 
            // DataGroup
            // 
            this.DataGroup.Controls.Add(this.UsageLogDelete);
            this.DataGroup.Controls.Add(this.PriceSettingRecreate);
            this.DataGroup.Controls.Add(this.ConfigFileRecreate);
            this.DataGroup.Location = new System.Drawing.Point(10, 120);
            this.DataGroup.Name = "DataGroup";
            this.DataGroup.Size = new System.Drawing.Size(200, 102);
            this.DataGroup.TabIndex = 0;
            this.DataGroup.TabStop = false;
            this.DataGroup.Text = "數據";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 231);
            this.Controls.Add(this.DataGroup);
            this.Controls.Add(this.GeneralGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.Text = "設定";
            this.GeneralGroup.ResumeLayout(false);
            this.DataGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private GroupBox GeneralGroup;
        private CheckBox DataProvideAllow;
        private GroupBox DataGroup;
        private Button UsageLogDelete;
        private Button PriceSettingRecreate;
        private Button ConfigFileRecreate;
        private Button RegisterStartup;
        private Button DeleteStartup;
    }
}