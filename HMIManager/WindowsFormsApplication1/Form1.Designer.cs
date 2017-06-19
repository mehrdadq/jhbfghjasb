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
            this.ListErrors = new System.Windows.Forms.ListBox();
            this.lblCorrect = new System.Windows.Forms.Label();
            this.lblWrong = new System.Windows.Forms.Label();
            this.lblAvarage = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
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
            this.btnStartAutoRead.Location = new System.Drawing.Point(209, 73);
            this.btnStartAutoRead.Name = "btnStartAutoRead";
            this.btnStartAutoRead.Size = new System.Drawing.Size(114, 78);
            this.btnStartAutoRead.TabIndex = 9;
            this.btnStartAutoRead.Text = "Start";
            this.btnStartAutoRead.UseVisualStyleBackColor = false;
            this.btnStartAutoRead.Click += new System.EventHandler(this.btnStartAutoRead_Click);
            // 
            // ListErrors
            // 
            this.ListErrors.FormattingEnabled = true;
            this.ListErrors.Location = new System.Drawing.Point(400, 9);
            this.ListErrors.Name = "ListErrors";
            this.ListErrors.Size = new System.Drawing.Size(597, 303);
            this.ListErrors.TabIndex = 10;
            // 
            // lblCorrect
            // 
            this.lblCorrect.AutoSize = true;
            this.lblCorrect.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblCorrect.Location = new System.Drawing.Point(149, 267);
            this.lblCorrect.Name = "lblCorrect";
            this.lblCorrect.Size = new System.Drawing.Size(27, 29);
            this.lblCorrect.TabIndex = 12;
            this.lblCorrect.Text = "0";
            this.lblCorrect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWrong
            // 
            this.lblWrong.AutoSize = true;
            this.lblWrong.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblWrong.Location = new System.Drawing.Point(149, 317);
            this.lblWrong.Name = "lblWrong";
            this.lblWrong.Size = new System.Drawing.Size(27, 29);
            this.lblWrong.TabIndex = 13;
            this.lblWrong.Text = "0";
            this.lblWrong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAvarage
            // 
            this.lblAvarage.AutoSize = true;
            this.lblAvarage.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.lblAvarage.Location = new System.Drawing.Point(149, 366);
            this.lblAvarage.Name = "lblAvarage";
            this.lblAvarage.Size = new System.Drawing.Size(48, 29);
            this.lblAvarage.TabIndex = 14;
            this.lblAvarage.Text = "0.0";
            this.lblAvarage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.Location = new System.Drawing.Point(36, 371);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 18);
            this.label3.TabIndex = 15;
            this.label3.Text = "AverageSpeed:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label4.Location = new System.Drawing.Point(36, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 18);
            this.label4.TabIndex = 16;
            this.label4.Text = "Fail Check:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label5.Location = new System.Drawing.Point(36, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 18);
            this.label5.TabIndex = 17;
            this.label5.Text = "Success Check:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 413);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblAvarage);
            this.Controls.Add(this.lblWrong);
            this.Controls.Add(this.lblCorrect);
            this.Controls.Add(this.ListErrors);
            this.Controls.Add(this.btnStartAutoRead);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListIP);
            this.Controls.Add(this.btnLoadDevice);
            this.Controls.Add(this.btnReadManual);
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
        private System.Windows.Forms.ListBox ListErrors;
        private System.Windows.Forms.Label lblCorrect;
        private System.Windows.Forms.Label lblWrong;
        private System.Windows.Forms.Label lblAvarage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

