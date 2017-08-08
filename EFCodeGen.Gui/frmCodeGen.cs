using MyMeta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Configuration;
using System.Runtime.InteropServices;

namespace EFCodeGen.Gui
{
	public partial class frmCodeGen : Form
	{

		public Generator Generator { get; internal set; }

		private Configuration _config;


		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool MessageBeep(BeepType uType);

		public frmCodeGen()
		{
			InitializeComponent();

		}

		private void frmCodeGen_Load(object sender, EventArgs e)
		{
			_config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

			txtOutput.Text = _config.AppSettings.Settings["OutputFolder"]?.Value;
			txtAppConfig.Text = _config.AppSettings.Settings["AppConfigLoc"]?.Value;
			txtNamespace.Text = _config.AppSettings.Settings["Namespace"]?.Value;
			toolStripStatusLabel1.Text = "";


		}



		private void button1_Click(object sender, EventArgs e)
		{

			openFileDialog1.CheckFileExists = true;
			openFileDialog1.ReadOnlyChecked = true;
			openFileDialog1.DefaultExt = ".config";
			openFileDialog1.FileName = txtAppConfig.Text;
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				txtAppConfig.Text = openFileDialog1.FileName;
				_config.AppSettings.Settings["AppConfigLoc"].Value = openFileDialog1.FileName;
				_config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");

			}
		}

		private void txtAppConfig_TextChanged(object sender, EventArgs e)
		{
			btnGo.Enabled = false;

			cbConnectionStrings.Items.Clear();
			var appConfig = txtAppConfig.Text;
			if (File.Exists(appConfig))
			{
				using (var stream = File.OpenRead(appConfig))
				{
					var xml = XDocument.Load(stream);
					cbConnectionStrings.Items.AddRange(
						((IEnumerable<object>) xml.XPathEvaluate("//connectionStrings/add/@name")).Cast<XAttribute>().Select(a => a.Value)
						.ToArray()
					);

					cbConnectionStrings.Items.AddRange(
						((IEnumerable<object>) xml.XPathEvaluate("//appSettings/add[contains(@value, \"Data Source=\")]/@key"))
						.Cast<XAttribute>().Select(a => a.Value).ToArray()
					);

					cbConnectionStrings.Items.AddRange(
						((IEnumerable<object>)xml.XPathEvaluate("//appSettings/add[contains(@value, \"server=\")]/@key"))
						.Cast<XAttribute>().Select(a => a.Value).ToArray()
					);
				}
				//if (cbConnectionStrings.Items.Count > 0)
				//{
				//	cbConnectionStrings.SelectedIndex = 0;
				//}
			}
		}


		private string _connectionstring;

		private void cbConnectionStrings_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnGo.Enabled = false;

			var appConfig = txtAppConfig.Text;
			if (File.Exists(appConfig))
			{
				var xpath1 = String.Format("//connectionStrings/add[@name='{0}']/@connectionString",
					cbConnectionStrings.SelectedItem);
				var xpath2 = String.Format("//appSettings/add[@key='{0}']/@value",
					cbConnectionStrings.SelectedItem);

				using (var stream = File.OpenRead(appConfig))
				{
					var xml = XDocument.Load(stream);
					var connectionstring = ((IEnumerable<object>) xml.XPathEvaluate(xpath1)).Cast<XAttribute>().Select(a => a.Value)
						.FirstOrDefault();
					if (connectionstring == null)
						connectionstring =
							((IEnumerable<object>) xml.XPathEvaluate(xpath2)).Cast<XAttribute>().Select(a => a.Value).FirstOrDefault();
					if (connectionstring != null)
					{
						txtConnectionstring.Text = connectionstring;
					}
				}
			}
		}

		private dbRoot _dbroot;

		void FillTablesListbox()
		{
			lbTables.Items.Clear();

			_dbroot = new dbRoot();
			_dbroot.Connect("SQL", "Provider=SQLOLEDB;" + _connectionstring);
			var saveColor = toolStripStatusLabel1.BackColor;

			if (_dbroot.Databases == null)
			{
				//	MessageBox.Show(this, "Cannot connect to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				toolStripStatusLabel1.BackColor = Color.Red;
				toolStripStatusLabel1.Text = "Cannot connect to database: " + cbConnectionStrings.SelectedItem;
				MessageBeep(BeepType.Exclamation);
			}
			else
			{
				Cursor.Current = Cursors.WaitCursor;
				toolStripStatusLabel1.Text = "";
				toolStripStatusLabel1.BackColor = saveColor;

				lbTables.Items.AddRange(_dbroot.DefaultDatabase.Tables.ToArray());
				Cursor.Current = Cursors.Default;
			}

		}

		private void lbTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnGo.Enabled = Directory.Exists(txtOutput.Text);
//			var prod = dbroot.DefaultDatabase.Tables["Productions"];

		}

		private void txtConnectionstring_TextChanged(object sender, EventArgs e)
		{
			btnGo.Enabled = false;
			_connectionstring = txtConnectionstring.Text;
			FillTablesListbox();
		}

		private async void btnGo_Click(object sender, EventArgs e)
		{
			var table = lbTables.SelectedItem as ITable;
			var filename = table.Name + ".cs";
			var pathname = Path.Combine(txtOutput.Text, filename);
			var result = DialogResult.Yes;

			if (File.Exists(pathname))
				result = MessageBox.Show(String.Format("File '{0}' exist.  Overwrite?", filename), "Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				toolStripStatusLabel1.Text = "Building "+ table.FullName;

				Cursor.Current = Cursors.WaitCursor;

				using (var sw = File.CreateText(pathname))
				{
					await Task.Run(()=>Generator.WriteEntityClass(table, sw));
				}
				Cursor.Current = Cursors.Default;
				toolStripStatusLabel1.Text = table.FullName +" Complete.";
			}
		}

		private void btnOutput_Click(object sender, EventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
			//dialog.InitialDirectory = "C:\\Users";
			dialog.IsFolderPicker = true;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				txtOutput.Text = dialog.FileName;
				_config.AppSettings.Settings["OutputFolder"].Value = dialog.FileName;
				_config.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection("appSettings");
			}
		}

		private void txtNamespace_TextChanged(object sender, EventArgs e)
		{
			_config.AppSettings.Settings.Remove("Namespace");
			_config.AppSettings.Settings.Add("Namespace", txtNamespace.Text);
			_config.Save(ConfigurationSaveMode.Modified);
			Generator.Namespace = txtNamespace.Text;

		}

	}
}
