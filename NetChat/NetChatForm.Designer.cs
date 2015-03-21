namespace CriChat
{
    partial class NetChatForm
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
            this.btnSettings = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMessageBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtMessageBoard = new System.Windows.Forms.TextBox();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.grpDisconnect = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.grpSettings.SuspendLayout();
            this.grpMessageBox.SuspendLayout();
            this.grpDisconnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(530, 19);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(63, 23);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.btnConnect);
            this.grpSettings.Controls.Add(this.txtUserName);
            this.grpSettings.Controls.Add(this.label1);
            this.grpSettings.Controls.Add(this.btnSettings);
            this.grpSettings.Location = new System.Drawing.Point(13, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(599, 52);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Connect";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(462, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(62, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(75, 21);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(381, 20);
            this.txtUserName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name:";
            // 
            // grpMessageBox
            // 
            this.grpMessageBox.Controls.Add(this.label4);
            this.grpMessageBox.Controls.Add(this.label3);
            this.grpMessageBox.Controls.Add(this.btnSend);
            this.grpMessageBox.Controls.Add(this.txtMessage);
            this.grpMessageBox.Controls.Add(this.txtMessageBoard);
            this.grpMessageBox.Controls.Add(this.lstClients);
            this.grpMessageBox.Location = new System.Drawing.Point(13, 70);
            this.grpMessageBox.Name = "grpMessageBox";
            this.grpMessageBox.Size = new System.Drawing.Size(599, 254);
            this.grpMessageBox.TabIndex = 2;
            this.grpMessageBox.TabStop = false;
            this.grpMessageBox.Text = "Message Board";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(459, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Connected Clients:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Messages:";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(462, 220);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(130, 24);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(9, 220);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(447, 24);
            this.txtMessage.TabIndex = 2;
            // 
            // txtMessageBoard
            // 
            this.txtMessageBoard.Location = new System.Drawing.Point(9, 45);
            this.txtMessageBoard.Multiline = true;
            this.txtMessageBoard.Name = "txtMessageBoard";
            this.txtMessageBoard.ReadOnly = true;
            this.txtMessageBoard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessageBoard.Size = new System.Drawing.Size(447, 169);
            this.txtMessageBoard.TabIndex = 1;
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.Location = new System.Drawing.Point(462, 45);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(131, 173);
            this.lstClients.TabIndex = 0;
            this.lstClients.SelectedIndexChanged += new System.EventHandler(this.lstClients_SelectedIndexChanged);
            // 
            // grpDisconnect
            // 
            this.grpDisconnect.Controls.Add(this.checkBox1);
            this.grpDisconnect.Controls.Add(this.txtStatus);
            this.grpDisconnect.Controls.Add(this.label2);
            this.grpDisconnect.Controls.Add(this.btnDisconnect);
            this.grpDisconnect.Location = new System.Drawing.Point(13, 330);
            this.grpDisconnect.Name = "grpDisconnect";
            this.grpDisconnect.Size = new System.Drawing.Size(599, 115);
            this.grpDisconnect.TabIndex = 3;
            this.grpDisconnect.TabStop = false;
            this.grpDisconnect.Text = "Disconnect";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(463, 64);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(134, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Show status messages";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(9, 34);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(447, 75);
            this.txtStatus.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status:";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(462, 34);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(130, 23);
            this.btnDisconnect.TabIndex = 0;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // NetChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 450);
            this.Controls.Add(this.grpDisconnect);
            this.Controls.Add(this.grpMessageBox);
            this.Controls.Add(this.grpSettings);
            this.Name = "NetChatForm";
            this.Text = ".NetChat";
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpMessageBox.ResumeLayout(false);
            this.grpMessageBox.PerformLayout();
            this.grpDisconnect.ResumeLayout(false);
            this.grpDisconnect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpMessageBox;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtMessageBoard;
        private System.Windows.Forms.GroupBox grpDisconnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

