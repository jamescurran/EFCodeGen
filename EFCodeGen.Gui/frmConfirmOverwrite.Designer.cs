namespace EFCodeGen.Gui
{
	partial class frmConfirmOverwrite
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBuildAll = new System.Windows.Forms.Button();
			this.btnBuildSome = new System.Windows.Forms.Button();
			this.btnBuildNone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.DisplayMember = "Name";
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(24, 44);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listBox1.Size = new System.Drawing.Size(176, 147);
			this.listBox1.TabIndex = 0;
			// 
			// listBox2
			// 
			this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox2.DisplayMember = "Name";
			this.listBox2.FormattingEnabled = true;
			this.listBox2.Location = new System.Drawing.Point(224, 44);
			this.listBox2.Name = "listBox2";
			this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listBox2.Size = new System.Drawing.Size(176, 147);
			this.listBox2.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(196, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "These files exist, and will be overwritten:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(209, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(209, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "These can be created without overwritting:";
			// 
			// btnBuildAll
			// 
			this.btnBuildAll.Location = new System.Drawing.Point(19, 209);
			this.btnBuildAll.Name = "btnBuildAll";
			this.btnBuildAll.Size = new System.Drawing.Size(106, 37);
			this.btnBuildAll.TabIndex = 4;
			this.btnBuildAll.Text = "Build all tables";
			this.btnBuildAll.UseVisualStyleBackColor = true;
			this.btnBuildAll.Click += new System.EventHandler(this.btnBuildAll_Click);
			// 
			// btnBuildSome
			// 
			this.btnBuildSome.Location = new System.Drawing.Point(173, 209);
			this.btnBuildSome.Name = "btnBuildSome";
			this.btnBuildSome.Size = new System.Drawing.Size(109, 37);
			this.btnBuildSome.TabIndex = 5;
			this.btnBuildSome.Text = "Build only non-existing files";
			this.btnBuildSome.UseVisualStyleBackColor = true;
			this.btnBuildSome.Click += new System.EventHandler(this.btnBuildSome_Click);
			// 
			// btnBuildNone
			// 
			this.btnBuildNone.Location = new System.Drawing.Point(330, 209);
			this.btnBuildNone.Name = "btnBuildNone";
			this.btnBuildNone.Size = new System.Drawing.Size(75, 37);
			this.btnBuildNone.TabIndex = 6;
			this.btnBuildNone.Text = "Cancel";
			this.btnBuildNone.UseVisualStyleBackColor = true;
			this.btnBuildNone.Click += new System.EventHandler(this.btnBuildNone_Click);
			// 
			// frmConfirmOverwrite
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(424, 271);
			this.Controls.Add(this.btnBuildNone);
			this.Controls.Add(this.btnBuildSome);
			this.Controls.Add(this.btnBuildAll);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Name = "frmConfirmOverwrite";
			this.Text = "Confirm Overwriting Files";
			this.Load += new System.EventHandler(this.frmConfirmOverwrite_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBuildAll;
		private System.Windows.Forms.Button btnBuildSome;
		private System.Windows.Forms.Button btnBuildNone;
	}
}