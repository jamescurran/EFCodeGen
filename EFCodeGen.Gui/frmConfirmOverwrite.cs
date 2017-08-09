using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyMeta;

namespace EFCodeGen.Gui
{



	public partial class frmConfirmOverwrite : Form
	{
		private IEnumerable<ITable> _overwrite;
		private IEnumerable<ITable> _safe;

		public frmConfirmOverwrite(IEnumerable<ITable> overwrite, IEnumerable<ITable> safe)
		{
			_overwrite = overwrite;
			_safe = safe;

			InitializeComponent();
		}

		private void frmConfirmOverwrite_Load(object sender, EventArgs e)
		{
			listBox1.DataSource = _overwrite;
			listBox2.DataSource = _safe;

		}


		private void btnBuildSome_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();

		}

		private void btnBuildNone_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();

		}

		private void btnBuildAll_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.Close();
		}
	}
}
