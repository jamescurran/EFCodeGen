namespace EFCodeGen.Gui
{
	partial class frmError
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
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCLose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtMessage
			// 
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMessage.Location = new System.Drawing.Point(12, 43);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtMessage.Size = new System.Drawing.Size(1043, 509);
			this.txtMessage.TabIndex = 0;
			this.txtMessage.WordWrap = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Exception Message:";
			// 
			// btnCLose
			// 
			this.btnCLose.Location = new System.Drawing.Point(979, 14);
			this.btnCLose.Name = "btnCLose";
			this.btnCLose.Size = new System.Drawing.Size(75, 23);
			this.btnCLose.TabIndex = 2;
			this.btnCLose.Text = "Close";
			this.btnCLose.UseVisualStyleBackColor = true;
			this.btnCLose.Click += new System.EventHandler(this.btnCLose_Click);
			// 
			// frmError
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 564);
			this.Controls.Add(this.btnCLose);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMessage);
			this.Name = "frmError";
			this.Text = "frmError";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCLose;
	}
}