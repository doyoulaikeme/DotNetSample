namespace SocketWebServer
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.groupServerInfo = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblForPort = new System.Windows.Forms.Label();
            this.lblForIP = new System.Windows.Forms.Label();
            this.groupServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(2, 80);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(445, 275);
            this.txtStatus.TabIndex = 5;
            // 
            // groupServerInfo
            // 
            this.groupServerInfo.Controls.Add(this.btnStart);
            this.groupServerInfo.Controls.Add(this.txtPort);
            this.groupServerInfo.Controls.Add(this.txtIPAddress);
            this.groupServerInfo.Controls.Add(this.lblForPort);
            this.groupServerInfo.Controls.Add(this.lblForIP);
            this.groupServerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupServerInfo.Location = new System.Drawing.Point(0, 0);
            this.groupServerInfo.Name = "groupServerInfo";
            this.groupServerInfo.Size = new System.Drawing.Size(481, 73);
            this.groupServerInfo.TabIndex = 3;
            this.groupServerInfo.TabStop = false;
            this.groupServerInfo.Text = "Web服务设置";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(356, 31);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "开启服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(254, 32);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(64, 21);
            this.txtPort.TabIndex = 4;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(48, 33);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(150, 21);
            this.txtIPAddress.TabIndex = 3;
            // 
            // lblForPort
            // 
            this.lblForPort.AutoSize = true;
            this.lblForPort.Location = new System.Drawing.Point(204, 35);
            this.lblForPort.Name = "lblForPort";
            this.lblForPort.Size = new System.Drawing.Size(53, 12);
            this.lblForPort.TabIndex = 2;
            this.lblForPort.Text = "端口号：";
            // 
            // lblForIP
            // 
            this.lblForIP.AutoSize = true;
            this.lblForIP.Location = new System.Drawing.Point(22, 36);
            this.lblForIP.Name = "lblForIP";
            this.lblForIP.Size = new System.Drawing.Size(29, 12);
            this.lblForIP.TabIndex = 0;
            this.lblForIP.Text = "IP：";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 391);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.groupServerInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Main";
            this.Text = "模拟服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupServerInfo.ResumeLayout(false);
            this.groupServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.GroupBox groupServerInfo;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblForPort;
        private System.Windows.Forms.Label lblForIP;
    }
}

