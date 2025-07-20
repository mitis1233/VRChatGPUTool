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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingForm));
            RegisterStartup = new Button();
            DeleteStartup = new Button();
            GeneralGroup = new GroupBox();
            ConfigFileRecreate = new Button();
            PriceSettingRecreate = new Button();
            UsageLogDelete = new Button();
            DataGroup = new GroupBox();
            GeneralGroup.SuspendLayout();
            DataGroup.SuspendLayout();
            SuspendLayout();
            // 
            // RegisterStartup
            // 
            RegisterStartup.Location = new Point(29, 24);
            RegisterStartup.Margin = new Padding(4);
            RegisterStartup.Name = "RegisterStartup";
            RegisterStartup.Size = new Size(175, 22);
            RegisterStartup.TabIndex = 1;
            RegisterStartup.Text = "開機自動開啟登録";
            RegisterStartup.Click += RegisterStartup_Click;
            // 
            // DeleteStartup
            // 
            DeleteStartup.Location = new Point(29, 64);
            DeleteStartup.Margin = new Padding(4);
            DeleteStartup.Name = "DeleteStartup";
            DeleteStartup.Size = new Size(175, 22);
            DeleteStartup.TabIndex = 2;
            DeleteStartup.Text = "開機自動開啟解除";
            DeleteStartup.Click += DeleteStartup_Click;
            // 
            // GeneralGroup
            // 
            GeneralGroup.Controls.Add(DeleteStartup);
            GeneralGroup.Controls.Add(RegisterStartup);
            GeneralGroup.Location = new Point(12, 11);
            GeneralGroup.Margin = new Padding(4);
            GeneralGroup.Name = "GeneralGroup";
            GeneralGroup.Padding = new Padding(4);
            GeneralGroup.Size = new Size(233, 104);
            GeneralGroup.TabIndex = 1;
            GeneralGroup.TabStop = false;
            GeneralGroup.Text = "一般";
            // 
            // ConfigFileRecreate
            // 
            ConfigFileRecreate.Location = new Point(29, 24);
            ConfigFileRecreate.Margin = new Padding(4);
            ConfigFileRecreate.Name = "ConfigFileRecreate";
            ConfigFileRecreate.Size = new Size(175, 22);
            ConfigFileRecreate.TabIndex = 3;
            ConfigFileRecreate.Text = "設置文件削除";
            ConfigFileRecreate.Click += ConfigFileRecreate_Click;
            // 
            // PriceSettingRecreate
            // 
            PriceSettingRecreate.Location = new Point(29, 69);
            PriceSettingRecreate.Margin = new Padding(4);
            PriceSettingRecreate.Name = "PriceSettingRecreate";
            PriceSettingRecreate.Size = new Size(175, 22);
            PriceSettingRecreate.TabIndex = 4;
            PriceSettingRecreate.Text = "電費設定削除";
            PriceSettingRecreate.Click += PriceSettingRecreate_Click;
            // 
            // UsageLogDelete
            // 
            UsageLogDelete.Location = new Point(29, 111);
            UsageLogDelete.Margin = new Padding(4);
            UsageLogDelete.Name = "UsageLogDelete";
            UsageLogDelete.Size = new Size(175, 22);
            UsageLogDelete.TabIndex = 5;
            UsageLogDelete.Text = "用電量歷史削除";
            UsageLogDelete.Click += UsageLogDelete_Click;
            // 
            // DataGroup
            // 
            DataGroup.Controls.Add(UsageLogDelete);
            DataGroup.Controls.Add(PriceSettingRecreate);
            DataGroup.Controls.Add(ConfigFileRecreate);
            DataGroup.Location = new Point(12, 123);
            DataGroup.Margin = new Padding(4);
            DataGroup.Name = "DataGroup";
            DataGroup.Padding = new Padding(4);
            DataGroup.Size = new Size(233, 155);
            DataGroup.TabIndex = 0;
            DataGroup.TabStop = false;
            DataGroup.Text = "數據";
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(257, 289);
            Controls.Add(DataGroup);
            Controls.Add(GeneralGroup);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingForm";
            Text = "設定";
            GeneralGroup.ResumeLayout(false);
            DataGroup.ResumeLayout(false);
            ResumeLayout(false);

        }

        private GroupBox GeneralGroup;
        private GroupBox DataGroup;
        private Button UsageLogDelete;
        private Button PriceSettingRecreate;
        private Button ConfigFileRecreate;
        private Button RegisterStartup;
        private Button DeleteStartup;
    }
}