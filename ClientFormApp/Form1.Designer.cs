namespace ClientFormApp
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
            this.Disconnect_Button = new System.Windows.Forms.Button();
            this.Send_Button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMsgRecividoServidor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tbStatus = new System.Windows.Forms.Label();
            this.Connet_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Disconnect_Button
            // 
            this.Disconnect_Button.Location = new System.Drawing.Point(30, 313);
            this.Disconnect_Button.Name = "Disconnect_Button";
            this.Disconnect_Button.Size = new System.Drawing.Size(110, 23);
            this.Disconnect_Button.TabIndex = 17;
            this.Disconnect_Button.Text = "Close Connection";
            this.Disconnect_Button.UseVisualStyleBackColor = true;
            this.Disconnect_Button.Click += new System.EventHandler(this.Disconnect_Button_Click);
            // 
            // Send_Button
            // 
            this.Send_Button.Location = new System.Drawing.Point(30, 155);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(93, 23);
            this.Send_Button.TabIndex = 16;
            this.Send_Button.Text = "Send message";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Mensaje recibido del Server";
            // 
            // txtMsgRecividoServidor
            // 
            this.txtMsgRecividoServidor.Location = new System.Drawing.Point(30, 225);
            this.txtMsgRecividoServidor.Multiline = true;
            this.txtMsgRecividoServidor.Name = "txtMsgRecividoServidor";
            this.txtMsgRecividoServidor.Size = new System.Drawing.Size(249, 74);
            this.txtMsgRecividoServidor.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Mensaje para enviar al Servidor";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(30, 66);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(249, 83);
            this.txtMessage.TabIndex = 12;
            // 
            // tbStatus
            // 
            this.tbStatus.AutoSize = true;
            this.tbStatus.Location = new System.Drawing.Point(151, 17);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Size = new System.Drawing.Size(89, 13);
            this.tbStatus.TabIndex = 11;
            this.tbStatus.Text = "No connection !!!";
            // 
            // Connet_Button
            // 
            this.Connet_Button.Location = new System.Drawing.Point(30, 12);
            this.Connet_Button.Name = "Connet_Button";
            this.Connet_Button.Size = new System.Drawing.Size(93, 23);
            this.Connet_Button.TabIndex = 9;
            this.Connet_Button.Text = "Connet Server";
            this.Connet_Button.UseVisualStyleBackColor = true;
            this.Connet_Button.Click += new System.EventHandler(this.Connet_Button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 348);
            this.Controls.Add(this.Disconnect_Button);
            this.Controls.Add(this.Send_Button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMsgRecividoServidor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.Connet_Button);
            this.Name = "Form1";
            this.Text = "Cliente";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Disconnect_Button;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMsgRecividoServidor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label tbStatus;
        private System.Windows.Forms.Button Connet_Button;
    }
}

