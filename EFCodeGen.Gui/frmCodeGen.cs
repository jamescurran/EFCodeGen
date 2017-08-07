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
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool MessageBeep(beepType uType);

		/// <summary>
		/// Enum type that enables intellisense on the private <see cref="Beep"/> method.
		/// </summary>
		/// <remarks>
		/// Used by the public Beep <see cref="Beep"/></remarks>
		public enum beepType : uint
		{
			/// <summary>
			/// A simple windows beep
			/// </summary>            
			SimpleBeep = 0xFFFFFFFF,
			/// <summary>
			/// A standard windows OK beep
			/// </summary>
			OK = 0x00,
			/// <summary>
			/// A standard windows Question beep
			/// </summary>
			Question = 0x20,
			/// <summary>
			/// A standard windows Exclamation beep
			/// </summary>
			Exclamation = 0x30,
			/// <summary>
			/// A standard windows Asterisk beep
			/// </summary>
			Asterisk = 0x40,
		}

		public frmCodeGen()
		{
			InitializeComponent();

			_config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

			txtOutput.Text = _config.AppSettings.Settings["OutputFolder"]?.Value;
			txtAppConfig.Text = _config.AppSettings.Settings["AppConfigLoc"]?.Value;
			toolStripStatusLabel1.Text = "";

		}

		public Generator Generator { get; internal set; }

		private Configuration _config;

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
						((IEnumerable<object>) xml.XPathEvaluate("//appSettings/add[contains(@value, \"Data Source\")]/@key"))
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

			if (_dbroot.Databases == null)
			{
				//	MessageBox.Show(this, "Cannot connect to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				toolStripStatusLabel1.Text = "Cannot connect to database";
				MessageBeep(beepType.Exclamation);
			}
			else
			{
				Cursor.Current = Cursors.WaitCursor;
				toolStripStatusLabel1.Text = "";
				lbTables.Items.AddRange(_dbroot.DefaultDatabase.Tables.Select(t => t.Name).ToArray());
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

		private void btnGo_Click(object sender, EventArgs e)
		{
			var table = lbTables.SelectedItem;
			var filename = table + ".cs";
			var pathname = Path.Combine(txtOutput.Text, filename);
			var result = DialogResult.Yes;

			if (File.Exists(pathname))
				result = MessageBox.Show(String.Format("File '{0}' exist.  Overwrite?", filename), "Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				toolStripStatusLabel1.Text = "Building "+ table;

				Cursor.Current = Cursors.WaitCursor;

				using (var sw = File.CreateText(pathname))
				{
					Generator.WriteEntityClass(_dbroot.DefaultDatabase.Tables[table], sw);
				}




				Cursor.Current = Cursors.Default;
				toolStripStatusLabel1.Text = table +" Complete.";


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
	}
}
