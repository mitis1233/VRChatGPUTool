namespace VRCGPUTool.Form
{
    partial class UnitPriceSetting : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitPriceSetting));
            this.ConfigButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.HourSplitInput = new System.Windows.Forms.NumericUpDown();
            this.UnitPriceInput = new System.Windows.Forms.NumericUpDown();
            this.ProfileAddButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.HourSplitInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceInput)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(417, 382);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(121, 21);
            this.ConfigButton.TabIndex = 0;
            this.ConfigButton.Text = "應用設置";
            this.ConfigButton.UseVisualStyleBackColor = true;
            this.ConfigButton.Click += new System.EventHandler(this.ConfigButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(215, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(400, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(215, 369);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 24);
            this.label5.TabIndex = 3;
            this.label5.Text = "12";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 24);
            this.label6.TabIndex = 4;
            this.label6.Text = "18";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(544, 382);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(121, 21);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "閉じる";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(415, 259);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(221, 120);
            this.label7.TabIndex = 6;
            this.label7.Text = "設置方法\r\n・如果不是按時區\r\n在右側輸入0:00的單價，添加並點擊應用\r\n・按時區\r\n輸入0:00的單價並添加一次\r\n將左側的時間設置為單價變化的時間後\r\n在右" +
    "側，輸入將從該小時開始應用的單價\r\n添加所有內容後單擊應用\r\n*如果要刪除，請選擇要刪除的項目。\r\n您可以通過右鍵單擊刪除它";
            // 
            // HourSplitInput
            // 
            this.HourSplitInput.Location = new System.Drawing.Point(6, 168);
            this.HourSplitInput.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.HourSplitInput.Name = "HourSplitInput";
            this.HourSplitInput.Size = new System.Drawing.Size(41, 22);
            this.HourSplitInput.TabIndex = 7;
            // 
            // UnitPriceInput
            // 
            this.UnitPriceInput.DecimalPlaces = 2;
            this.UnitPriceInput.Location = new System.Drawing.Point(85, 168);
            this.UnitPriceInput.Name = "UnitPriceInput";
            this.UnitPriceInput.Size = new System.Drawing.Size(57, 22);
            this.UnitPriceInput.TabIndex = 8;
            // 
            // ProfileAddButton
            // 
            this.ProfileAddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ProfileAddButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.ProfileAddButton.Location = new System.Drawing.Point(106, 195);
            this.ProfileAddButton.Name = "ProfileAddButton";
            this.ProfileAddButton.Size = new System.Drawing.Size(51, 21);
            this.ProfileAddButton.TabIndex = 9;
            this.ProfileAddButton.Text = "追加";
            this.ProfileAddButton.UseVisualStyleBackColor = true;
            this.ProfileAddButton.Click += new System.EventHandler(this.ProfileAddButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(33, 18);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(109, 136);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 10;
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "時～";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "円";
            // 
            // groupBox1
            // 
            this.groupBox1.CausesValidation = false;
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.HourSplitInput);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.UnitPriceInput);
            this.groupBox1.Controls.Add(this.ProfileAddButton);
            this.groupBox1.Location = new System.Drawing.Point(491, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 230);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "電費設定";
            // 
            // UnitPriceSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 414);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConfigButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnitPriceSetting";
            this.Text = "電気代設定";
            this.Load += new System.EventHandler(this.UnitPriceSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Redraw_Form);
            ((System.ComponentModel.ISupportInitialize)(this.HourSplitInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnitPriceInput)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button ConfigButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown HourSplitInput;
        private System.Windows.Forms.NumericUpDown UnitPriceInput;
        private System.Windows.Forms.Button ProfileAddButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}