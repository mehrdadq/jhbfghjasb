namespace testpanel
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer timer1;
            this.btnReadManual = new System.Windows.Forms.Button();
            this.btnLoadDevice = new System.Windows.Forms.Button();
            this.ListIP = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStartAutoRead = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lblCorrect = new System.Windows.Forms.Label();
            this.lblRead = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAvarage = new System.Windows.Forms.Label();
            this.ListErrors = new System.Windows.Forms.ListBox();
            this.ListRegion = new System.Windows.Forms.ListBox();
            this.ListProductionLine = new System.Windows.Forms.ListBox();
            timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnReadManual
            // 
            this.btnReadManual.Location = new System.Drawing.Point(34, 41);
            this.btnReadManual.Margin = new System.Windows.Forms.Padding(2);
            this.btnReadManual.Name = "btnReadManual";
            this.btnReadManual.Size = new System.Drawing.Size(108, 27);
            this.btnReadManual.TabIndex = 0;
            this.btnReadManual.Text = "Raed DataManual";
            this.btnReadManual.UseVisualStyleBackColor = true;
            this.btnReadManual.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLoadDevice
            // 
            this.btnLoadDevice.Location = new System.Drawing.Point(34, 73);
            this.btnLoadDevice.Name = "btnLoadDevice";
            this.btnLoadDevice.Size = new System.Drawing.Size(108, 23);
            this.btnLoadDevice.TabIndex = 2;
            this.btnLoadDevice.Text = "Load Device";
            this.btnLoadDevice.UseVisualStyleBackColor = true;
            this.btnLoadDevice.Click += new System.EventHandler(this.button2_Click);
            // 
            // ListIP
            // 
            this.ListIP.FormattingEnabled = true;
            this.ListIP.Location = new System.Drawing.Point(34, 102);
            this.ListIP.Name = "ListIP";
            this.ListIP.Size = new System.Drawing.Size(108, 134);
            this.ListIP.TabIndex = 4;
            this.ListIP.SelectedIndexChanged += new System.EventHandler(this.ListIP_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP:";
            // 
            // txtIP
            // 
            this.txtIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtIP.Location = new System.Drawing.Point(172, 45);
            this.txtIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(206, 19);
            this.txtIP.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblStatus.Location = new System.Drawing.Point(120, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 8;
            // 
            // btnStartAutoRead
            // 
            this.btnStartAutoRead.BackColor = System.Drawing.SystemColors.Window;
            this.btnStartAutoRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnStartAutoRead.Location = new System.Drawing.Point(183, 129);
            this.btnStartAutoRead.Name = "btnStartAutoRead";
            this.btnStartAutoRead.Size = new System.Drawing.Size(114, 78);
            this.btnStartAutoRead.TabIndex = 9;
            this.btnStartAutoRead.Text = "Start";
            this.btnStartAutoRead.UseVisualStyleBackColor = false;
            this.btnStartAutoRead.Click += new System.EventHandler(this.btnStartAutoRead_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.Location = new System.Drawing.Point(31, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "Succesed Read :";
            // 
            // lblWrong
            // 
            this.lblWrong.AutoSize = true;
            this.lblWrong.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblWrong.Location = new System.Drawing.Point(169, 355);
            this.lblWrong.Name = "lblWrong";
            this.lblWrong.Size = new System.Drawing.Size(17, 18);
            this.lblWrong.TabIndex = 11;
            this.lblWrong.Text = "0";
            // 
            // lblCorrect
            // 
            this.lblCorrect.AutoSize = true;
            this.lblCorrect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblCorrect.Location = new System.Drawing.Point(169, 305);
            this.lblCorrect.Name = "lblCorrect";
            this.lblCorrect.Size = new System.Drawing.Size(17, 18);
            this.lblCorrect.TabIndex = 12;
            this.lblCorrect.Text = "0";
            // 
            // lblRead
            // 
            this.lblRead.AutoSize = true;
            this.lblRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblRead.Location = new System.Drawing.Point(169, 251);
            this.lblRead.Name = "lblRead";
            this.lblRead.Size = new System.Drawing.Size(17, 18);
            this.lblRead.TabIndex = 13;
            this.lblRead.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label7.Location = new System.Drawing.Point(22, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 18);
            this.label7.TabIndex = 14;
            this.label7.Text = "Succesed Check :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label8.Location = new System.Drawing.Point(47, 352);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 18);
            this.label8.TabIndex = 15;
            this.label8.Text = "Checked faild :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label4.Location = new System.Drawing.Point(39, 401);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 18);
            this.label4.TabIndex = 17;
            this.label4.Text = "Average Speed :";
            // 
            // lblAvarage
            // 
            this.lblAvarage.AutoSize = true;
            this.lblAvarage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblAvarage.Location = new System.Drawing.Point(169, 404);
            this.lblAvarage.Name = "lblAvarage";
            this.lblAvarage.Size = new System.Drawing.Size(17, 18);
            this.lblAvarage.TabIndex = 16;
            this.lblAvarage.Text = "0";
            // 
            // ListErrors
            // 
            this.ListErrors.FormattingEnabled = true;
            this.ListErrors.HorizontalScrollbar = true;
            this.ListErrors.Location = new System.Drawing.Point(337, 89);
            this.ListErrors.Name = "ListErrors";
            this.ListErrors.ScrollAlwaysVisible = true;
            this.ListErrors.Size = new System.Drawing.Size(398, 329);
            this.ListErrors.TabIndex = 18;
            // 
            // ListRegion
            // 
            this.ListRegion.FormattingEnabled = true;
            this.ListRegion.Location = new System.Drawing.Point(34, 102);
            this.ListRegion.Name = "ListRegion";
            this.ListRegion.Size = new System.Drawing.Size(108, 134);
            this.ListRegion.TabIndex = 19;
            this.ListRegion.Visible = false;
            // 
            // ListProductionLine
            // 
            this.ListProductionLine.FormattingEnabled = true;
            this.ListProductionLine.Location = new System.Drawing.Point(34, 102);
            this.ListProductionLine.Name = "ListProductionLine";
            this.ListProductionLine.Size = new System.Drawing.Size(108, 134);
            this.ListProductionLine.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.ClientSize = new System.Drawing.Size(747, 428);
            this.Controls.Add(this.ListErrors);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAvarage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblRead);
            this.Controls.Add(this.lblCorrect);
            this.Controls.Add(this.lblWrong);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStartAutoRead);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadDevice);
            this.Controls.Add(this.btnReadManual);
            this.Controls.Add(this.ListIP);
            this.Controls.Add(this.ListRegion);
            this.Controls.Add(this.ListProductionLine);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Software Towzin";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadManual;
        private System.Windows.Forms.Button btnLoadDevice;
        private System.Windows.Forms.ListBox ListIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnStartAutoRead;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblWrong;
        private System.Windows.Forms.Label lblCorrect;
        private System.Windows.Forms.Label lblRead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAvarage;
        private System.Windows.Forms.ListBox ListErrors;
        private System.Windows.Forms.ListBox ListRegion;
        private System.Windows.Forms.ListBox ListProductionLine;
    }
}

