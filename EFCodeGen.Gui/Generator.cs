using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyMeta;
using RazorEngine;
using RazorEngine.Compilation;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace EFCodeGen.Gui
{
	public class Generator
	{
		public string Namespace { get; set; }

		public Generator()
		{
			
		}


		public async void Initialize()
		{
			var taskService = Task.Run(() =>
			{
				var config = new TemplateServiceConfiguration
				{
					Language = Language.CSharp,
					EncodedStringFactory = new RawStringFactory(),
					Debug = true,
					ReferenceResolver = new MyIReferenceResolver()
				};

				var service = RazorEngineService.Create(config);
				Engine.Razor = service;
			});

			var taskData = Task.Run(() => LoadDataTypes());

			await taskService;

			var taskCompile1 = Task.Run(() => LoadAndCompile("EntityClass.cshtml"));

			await taskData;
			await taskCompile1;
		}


		private void LoadAndCompile(string filename)
		{
			var t1 = File.ReadAllText(filename);
			Engine.Razor.Compile(t1, filename, typeof(TableModel));
		}


		public class TableModel
		{
			public ITable Table { get; set; }

			public StringDictionary DataTypeMap { get; set; }
			public string Namespace { get; set; }

			public Func<string, string> DePluralize { get; set; }
			public Func<string, string> GetDataType { get; set; }
			public Func<string, string> ToPascalCase { get; set; }


			public TextInfo TextInfo { get; set; }

		}


		public void WriteEntityClass(ITable table, TextWriter tw)
		{
			try
			{

				Engine.Razor.Run("EntityClass.cshtml", tw, typeof(TableModel), 
						new TableModel
						{
							Table = table,
							DePluralize = DePluralize,
							ToPascalCase = ToPascalCase,
							GetDataType = GetDataType,
							Namespace = Namespace,
							TextInfo = CultureInfo.CurrentCulture.TextInfo
						});

			}
			catch (Exception e)
			{
				var frm = new frmError();
				frm.Message = e.Message;
				frm.Show();
			}
		}

		static string DePluralize(string plural)
		{
			if (plural.EndsWith("ies", StringComparison.InvariantCultureIgnoreCase))
				return plural.Substring(0, plural.Length - 3) + "y";

			if (plural.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
				return plural.Substring(0, plural.Length - 1);

			return plural;
		}

		static readonly char[] seps = new char[] { '_', ' ', '/', '-' };
		public static string ToPascalCase(string name)
		{
			var sb = new StringBuilder();
			bool lastSep = true;
			bool lastUpper = false;
			foreach (char cc in name)
			{
				var c = cc;
				if (seps.Contains(c))
					lastSep = true;
				else
				{
					var isUpper	 = Char.IsUpper(c);
					if (lastSep)
						c = Char.ToUpperInvariant(c);
					else if (isUpper && lastUpper)
						c = Char.ToLowerInvariant(c);

					sb.Append(c);
					lastSep = false;
					lastUpper = isUpper;
				}
			}
			if (Char.IsDigit(sb[0]))
				sb.Insert(0, '_');

			return sb.ToString();
		}



		public static string GetDataType(string dbtype)
		{
			return _dataTypeMap[dbtype] ?? dbtype;

		}





		static StringDictionary _dataTypeMap;

		void LoadDataTypes()
		{


			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "EFCodeGen.Gui.DataTypes.txt";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				string result = reader.ReadToEnd();
				_dataTypeMap = new StringDictionary();
				foreach (var t in result.Split('\n'))
				{
					var parts = t.Split('=');
					if (parts.Length == 2)
						_dataTypeMap.Add(parts[0], parts[1].Trim());
				}
			}
		}

	}


	// this is taken directly from the RazorEngine docs (plus adding the MyMeta line)

	class MyIReferenceResolver : IReferenceResolver
	{
		public string FindLoaded(IEnumerable<string> refs, string find)
		{
			return refs.First(r => r.EndsWith(System.IO.Path.DirectorySeparatorChar + find));
		}
		public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies)
		{
			// TypeContext gives you some context for the compilation (which templates, which namespaces and types)

			// You must make sure to include all libraries that are required!
			// Mono compiler does add more standard references than csc! 
			// If you want mono compatibility include ALL references here, including mscorlib!
			// If you include mscorlib here the compiler is called with /nostdlib.
			IEnumerable<string> loadedAssemblies = (new UseCurrentAssembliesReferenceResolver())
				.GetReferences(context, includeAssemblies)
				.Select(r => r.GetFile())
				.ToArray();

			yield return CompilerReference.From(FindLoaded(loadedAssemblies, "mscorlib.dll"));
			yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.dll"));
			yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Core.dll"));
			yield return CompilerReference.From(FindLoaded(loadedAssemblies, "RazorEngine.dll"));
			yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.Razor.dll"));
			yield return CompilerReference.From(typeof(MyIReferenceResolver).Assembly); // Assembly
			yield return CompilerReference.From(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MyMeta.dll"));

			// There are several ways to load an assembly:
			//yield return CompilerReference.From("Path-to-my-custom-assembly"); // file path (string)
			//byte[] assemblyInByteArray = --- Load your assembly ---;
			//yield return CompilerReference.From(assemblyInByteArray); // byte array (roslyn only)
			//string assemblyFile = --- Get the path to the assembly ---;
			//yield return CompilerReference.From(File.OpenRead(assemblyFile)); // stream (roslyn only)
		}
	}

}
