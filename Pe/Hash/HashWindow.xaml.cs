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
		AssemblyLoader AssemblyLoader { get; set; }
		EventWaitHandle Event { get; set; }

		#endregion /////////////////////////////

		#region Initialize

		void InitializeCommandLine()
		{
			var commandLine = new CommandLine();

			var keyTitle = "title";
			if(commandLine.HasValue(keyTitle)) {
				var valTitle = commandLine.GetValue(keyTitle);
				Title = valTitle;
			}
		}
		void InitializeEvent()
		{
			var ev = new EVDictionary();
			var eventName = ev[EVLiteral.communicationEventName];
			if(!string.IsNullOrWhiteSpace(eventName)) {
				EventWaitHandle eventHandle;
				if(EventWaitHandle.TryOpenExisting(eventName, out eventHandle)) {
					Event = eventHandle;
					Task.Run(() =>{
						return Event.WaitOne(Timeout.InfiniteTimeSpan);
					}).ContinueWith(t => {
						if(t.Result) {
							Application.Current.Shutdown();
						}
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}
		void Initialize()
		{
			AssemblyLoader = new AssemblyLoader();
			AssemblyLoader.AttachmentEvent();

			ViewModel = new HashViewModel(new HashModel());

			InitializeCommandLine();
			InitializeEvent();

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
