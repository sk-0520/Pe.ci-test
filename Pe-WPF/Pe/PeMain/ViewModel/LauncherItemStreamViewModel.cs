namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Documents;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherItemStreamViewModel : LauncherItemSimpleViewModel, IHavingView<LauncherItemStreamWindow>
	{
		#region variable

		bool _processRunning;
		string _inputConsole;

		#endregion

		public LauncherItemStreamViewModel(LauncherItemModel model, LauncherItemStreamWindow view, Process process, StreamSettingModel streamSetting, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess, IAppSender appSender)
			: base(model, launcherIconCaching, nonPorocess, appSender)
		{
			View = view;
			Process = process;
			StartInfo = Process.StartInfo;
			StreamSetting = streamSetting;

			if (HasView) {
				View.UserClosing += View_UserClosing;
				OutputStream = View.viewConsole.Document;
			} else {
				OutputStream = new FlowDocument();
			}
		}

		#region property

		StreamSettingModel StreamSetting { get; set; }

		FlowDocument OutputStream { get; set; }
		Task OutputTask;
		Task ErrorTask;

		public Process Process { get; private set; }
		public ProcessStartInfo StartInfo { get; private set; }

		public bool ProcessRunning
		{
			get { return this._processRunning; }
			set { SetVariableValue(ref this._processRunning, value); }
		}


		public bool IsEnabledInput
		{
			get { return Model.StdStream.IsEnabledInput; }
		}
		
		public string InputConsole
		{
			get { return this._inputConsole; }
			set { SetVariableValue(ref this._inputConsole, value); }
		}

		#endregion

		#region command

		public ICommand RefreshProcessCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						RefreshProcess();
					}
				);

				return result;
			}
		}

		public ICommand KillProcessCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						KillProcess();
					}
				);

				return result;
			}
		}

		public ICommand ReturnInputCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						SendInput(InputConsole);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		public void Start()
		{
			Process.EnableRaisingEvents = true;
			Process.Exited += Process_Exited;
			Process.Start();

			Debug.Assert(Model.StdStream.OutputWatch);
			OutputTask = Task.Run(() => ReceiveOutput(Process.StandardOutput, true));
			ErrorTask = Task.Run(() => ReceiveOutput(Process.StandardError, false));
			
			if (HasView) {
				View.Show();
			}

			RefreshProcess();
			//var waitTime = (int)TimeSpan.FromSeconds(3).TotalMilliseconds;
			ProcessRunning = !Process.HasExited;
		}

		void ReceiveOutput(StreamReader reader, bool isStandardOutput)
		{
			const int maxBuffer = 1024;
			int waitTime = (int)TimeSpan.FromMilliseconds(1000).TotalMilliseconds;

			//Action<string> action = line => {
			//	Debug.Assert(line.Length > 0);
			//	Debug.WriteLine(line);
			//	var textRange = new TextRange(OutputStream.ContentEnd, OutputStream.ContentEnd);
			//	textRange.Text = line;
			//	//textRange.ApplyPropertyValue(TextElement.)
			//};

			char[] buffer = new char[maxBuffer];
			var isContinue = true;
			while (isContinue) {
				var readLength = reader.ReadAsync(buffer, 0, buffer.Length);
				readLength.Wait(waitTime);

				if (readLength.Result == 0) {
					isContinue = true;
					return;
				}
				var line = string.Concat(buffer.Take(readLength.Result).ToArray());
				Application.Current.Dispatcher.Invoke(new Action(() => {
					var textRange = new TextRange(OutputStream.ContentEnd, OutputStream.ContentEnd);
					textRange.Text = line;
				}));
			}
		}

		void RefreshProcess()
		{
			Process.Refresh();
			OnPropertyChanged("Process");
		}

		void KillProcess()
		{
			try {
				if (Process.HasExited) {
					return;
				}
				Process.Kill();
			} catch (Exception ex) {
				NonProcess.Logger.Error(ex);
			}
		}

		void SendInput(string command)
		{
			Process.StandardInput.WriteLine(command);
		}

		#endregion

		#region IHavingView

		public LauncherItemStreamWindow View { get; private set; }

		public bool HasView { get { return View != null; } }

		#endregion

		void Process_Exited(object sender, EventArgs e)
		{
			ProcessRunning = false;
			RefreshProcess();
		}

		void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			View.UserClosing -= View_UserClosing;
			// not impl
		}


	}

}
