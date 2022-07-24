﻿namespace VRCGPUTool.Form
{
    partial class PowerHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PowerHistory));
            this.UsageGraphDay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TabRange = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DataRefreshDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DateRefresh = new System.Windows.Forms.Button();
            this.NextDayData = new System.Windows.Forms.Button();
            this.PreviousDayData = new System.Windows.Forms.Button();
            this.LogDateLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.UsageGraphMonth = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UsageGraphDay)).BeginInit();
            this.TabRange.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsageGraphMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // UsageGraphDay
            // 
            chartArea1.AxisX.MajorGrid.Interval = 1D;
            chartArea1.AxisX.MajorTickMark.Interval = 1D;
            chartArea1.Name = "ChartArea1";
            this.UsageGraphDay.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.UsageGraphDay.Legends.Add(legend1);
            this.UsageGraphDay.Location = new System.Drawing.Point(6, 40);
            this.UsageGraphDay.Name = "UsageGraphDay";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.UsageGraphDay.Series.Add(series1);
            this.UsageGraphDay.Size = new System.Drawing.Size(782, 377);
            this.UsageGraphDay.TabIndex = 0;
            this.UsageGraphDay.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "電力使用量(24時間)";
            this.UsageGraphDay.Titles.Add(title1);
            // 
            // TabRange
            // 
            this.TabRange.Controls.Add(this.tabPage1);
            this.TabRange.Controls.Add(this.tabPage2);
            this.TabRange.Location = new System.Drawing.Point(12, 12);
            this.TabRange.Name = "TabRange";
            this.TabRange.SelectedIndex = 0;
            this.TabRange.Size = new System.Drawing.Size(826, 495);
            this.TabRange.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DataRefreshDate);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.DateRefresh);
            this.tabPage1.Controls.Add(this.NextDayData);
            this.tabPage1.Controls.Add(this.PreviousDayData);
            this.tabPage1.Controls.Add(this.LogDateLabel);
            this.tabPage1.Controls.Add(this.UsageGraphDay);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(818, 469);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "電力使用量【24時間】";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DataRefreshDate
            // 
            this.DataRefreshDate.AutoSize = true;
            this.DataRefreshDate.Location = new System.Drawing.Point(582, 16);
            this.DataRefreshDate.Name = "DataRefreshDate";
            this.DataRefreshDate.Size = new System.Drawing.Size(36, 13);
            this.DataRefreshDate.TabIndex = 6;
            this.DataRefreshDate.Text = "(Date)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(490, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "データ取得日時:";
            // 
            // DateRefresh
            // 
            this.DateRefresh.Location = new System.Drawing.Point(692, 431);
            this.DateRefresh.Name = "DateRefresh";
            this.DateRefresh.Size = new System.Drawing.Size(106, 25);
            this.DateRefresh.TabIndex = 4;
            this.DateRefresh.Text = "最新データに更新";
            this.DateRefresh.UseVisualStyleBackColor = true;
            this.DateRefresh.Click += new System.EventHandler(this.DateRefresh_Click);
            // 
            // NextDayData
            // 
            this.NextDayData.Location = new System.Drawing.Point(388, 431);
            this.NextDayData.Name = "NextDayData";
            this.NextDayData.Size = new System.Drawing.Size(155, 25);
            this.NextDayData.TabIndex = 3;
            this.NextDayData.Text = "翌日＞＞";
            this.NextDayData.UseVisualStyleBackColor = true;
            this.NextDayData.Click += new System.EventHandler(this.NextDayData_Click);
            // 
            // PreviousDayData
            // 
            this.PreviousDayData.Location = new System.Drawing.Point(155, 431);
            this.PreviousDayData.Name = "PreviousDayData";
            this.PreviousDayData.Size = new System.Drawing.Size(155, 25);
            this.PreviousDayData.TabIndex = 2;
            this.PreviousDayData.Text = "＜＜前日";
            this.PreviousDayData.UseVisualStyleBackColor = true;
            this.PreviousDayData.Click += new System.EventHandler(this.PreviousDayData_Click);
            // 
            // LogDateLabel
            // 
            this.LogDateLabel.AutoSize = true;
            this.LogDateLabel.Font = new System.Drawing.Font("游ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogDateLabel.Location = new System.Drawing.Point(16, 12);
            this.LogDateLabel.Name = "LogDateLabel";
            this.LogDateLabel.Size = new System.Drawing.Size(294, 27);
            this.LogDateLabel.TabIndex = 1;
            this.LogDateLabel.Text = "2020年1月1日の電力使用履歴";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.UsageGraphMonth);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(818, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "電力使用量【1カ月】";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // UsageGraphMonth
            // 
            chartArea2.AxisX.MajorGrid.Interval = 1D;
            chartArea2.AxisX.MajorTickMark.Interval = 1D;
            chartArea2.Name = "ChartArea1";
            this.UsageGraphMonth.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.UsageGraphMonth.Legends.Add(legend2);
            this.UsageGraphMonth.Location = new System.Drawing.Point(18, 46);
            this.UsageGraphMonth.Name = "UsageGraphMonth";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.UsageGraphMonth.Series.Add(series2);
            this.UsageGraphMonth.Size = new System.Drawing.Size(782, 377);
            this.UsageGraphMonth.TabIndex = 1;
            this.UsageGraphMonth.Text = "chart1";
            title2.Name = "Title1";
            title2.Text = "電力使用量(１カ月)";
            this.UsageGraphMonth.Titles.Add(title2);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("游ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "2020年1月の電力使用履歴";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(399, 429);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "翌月＞＞";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(166, 429);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "＜＜先月";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(595, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "(Date)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(503, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "データ取得日時:";
            // 
            // PowerHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 519);
            this.Controls.Add(this.TabRange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PowerHistory";
            this.Text = "電力使用履歴";
            this.Load += new System.EventHandler(this.PowerHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UsageGraphDay)).EndInit();
            this.TabRange.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UsageGraphMonth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart UsageGraphDay;
        private System.Windows.Forms.TabControl TabRange;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button NextDayData;
        private System.Windows.Forms.Button PreviousDayData;
        private System.Windows.Forms.Label LogDateLabel;
        private System.Windows.Forms.Button DateRefresh;
        private System.Windows.Forms.Label DataRefreshDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart UsageGraphMonth;
    }
}