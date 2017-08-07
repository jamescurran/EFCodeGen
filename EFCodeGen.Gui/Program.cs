using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCodeGen.Gui
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main()
		{
			if (AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				// RazorEngine cannot clean up from the default appdomain...
//				Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
				AppDomainSetup adSetup = new AppDomainSetup();
				adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
				var current = AppDomain.CurrentDomain;
				// You only need to add strongnames when your appdomain is not a full trust environment.
				var strongNames = new StrongName[0];

				var domain = AppDomain.CreateDomain(
					"MyMainDomain", null,
					current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
					strongNames);
				return domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var generator = new Generator();
			generator.Initialize();

			var frm = new frmCodeGen();
			frm.Generator = generator;

			Application.Run(frm);

			return 0;
		}
	}
}
