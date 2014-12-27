using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class HashWindow: Window
	{
		public HashWindow()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		HashViewModel ViewModel { get; set; }

		#endregion /////////////////////////////

		#region Initialize

		void InitializeAssembly()
		{
			var assemblyList = new string[] { "PlatformInvoke", "Utility", };
			var basePath = "Pe/bin/dir";
			
			var assembly = Assembly.GetExecutingAssembly();
			
			var dirPath = assembly.Location;
			foreach(var n in Enumerable.Range(0, basePath.Split('/').Count())) {
				dirPath = System.IO.Path.GetDirectoryName(dirPath);
			}
			var libDir = System.IO.Path.Combine(dirPath, "lib");
			foreach(var assemblyName in assemblyList) {
				var assemblyPath = System.IO.Path.Combine(libDir, assemblyName + ".dll");
				var loadAssembly = Assembly.LoadFrom(assemblyPath);
			}
			assembly.GetReferencedAssemblies();
		}

		void InitializeCommandLine()
		{
			var commandLine = new CommandLine();

			var eventNameArg = "event-name";
			if(commandLine.HasValue(eventNameArg)) {
				ViewModel.EventName = commandLine.GetValue(eventNameArg);
			}
		}

		void Initialize()
		{
			InitializeAssembly();

			ViewModel = new HashViewModel(new HashModel());

			InitializeCommandLine();

			DataContext = ViewModel;
		}

		#endregion //////////////////////////////////////

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if(dialog.ShowDialog() == true) {
				var path = dialog.FileName;
				ViewModel.FilePath = path;
			}
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			var files = e.Data.GetData(DataFormats.FileDrop, true) as string[];
			if(files != null && files.Length == 1) {
				ViewModel.FilePath = files[0];
			}
		}
	}
}
