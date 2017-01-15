namespace ChatProject
{
    partial class ConfigurationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.Port2TextBox = new System.Windows.Forms.TextBox();
            this.IpTextBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.IpforTcpTextBox = new System.Windows.Forms.TextBox();
            this.RemoteTcpPortTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SendDataPort";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ReceiveDataPort";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ip For Udp";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(113, 6);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 20);
            this.PortTextBox.TabIndex = 3;
            this.PortTextBox.TextChanged += new System.EventHandler(this.portTextBox_TextChanged);
            // 
            // Port2TextBox
            // 
            this.Port2TextBox.Location = new System.Drawing.Point(113, 53);
            this.Port2TextBox.Name = "Port2TextBox";
            this.Port2TextBox.Size = new System.Drawing.Size(100, 20);
            this.Port2TextBox.TabIndex = 4;
            this.Port2TextBox.TextChanged += new System.EventHandler(this.port2TextBox_TextChanged);
            // 
            // IpTextBox
            // 
            this.IpTextBox.Location = new System.Drawing.Point(113, 95);
            this.IpTextBox.Name = "IpTextBox";
            this.IpTextBox.Size = new System.Drawing.Size(100, 20);
            this.IpTextBox.TabIndex = 5;
            this.IpTextBox.TextChanged += new System.EventHandler(this.ipTextBox_TextChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(189, 195);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 8;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ip For Tcp";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Remote Tcp Port";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // IpforTcpTextBox
            // 
            this.IpforTcpTextBox.Location = new System.Drawing.Point(113, 134);
            this.IpforTcpTextBox.Name = "IpforTcpTextBox";
            this.IpforTcpTextBox.Size = new System.Drawing.Size(100, 20);
            this.IpforTcpTextBox.TabIndex = 6;
            this.IpforTcpTextBox.TextChanged += new System.EventHandler(this.ipforTcpTextBox_TextChanged);
            // 
            // RemoteTcpPortTextBox
            // 
            this.RemoteTcpPortTextBox.Location = new System.Drawing.Point(113, 167);
            this.RemoteTcpPortTextBox.Name = "RemoteTcpPortTextBox";
            this.RemoteTcpPortTextBox.Size = new System.Drawing.Size(100, 20);
            this.RemoteTcpPortTextBox.TabIndex = 7;
            this.RemoteTcpPortTextBox.TextChanged += new System.EventHandler(this.remoteTcpPortTextBox_TextChanged);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 230);
            this.Controls.Add(this.RemoteTcpPortTextBox);
            this.Controls.Add(this.IpforTcpTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.IpTextBox);
            this.Controls.Add(this.Port2TextBox);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ConfigurationForm";
            this.Text = "ConfigurationForm";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.TextBox Port2TextBox;
        private System.Windows.Forms.TextBox IpTextBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox IpforTcpTextBox;
        private System.Windows.Forms.TextBox RemoteTcpPortTextBox;
    }
}