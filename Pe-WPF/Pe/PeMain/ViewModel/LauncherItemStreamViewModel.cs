namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		#endregion

		public LauncherItemStreamViewModel(LauncherItemModel model, LauncherItemStreamWindow view, Process process, StreamSettingModel streamSetting, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess, IAppSender appSender)
			: base(model, launcherIconCaching, nonPorocess, appSender)
		{
			View = view;
			Process = process;
			StartInfo = Process.StartInfo;
			StreamSetting = streamSetting;
		}

		#region property

		StreamSettingModel StreamSetting { get; set; }

		public Process Process { get; private set; }
		public ProcessStartInfo StartInfo { get; private set; }

		public bool ProcessRunning
		{
			get { return this._processRunning; }
			set { SetVariableValue(ref this._processRunning, value); }
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

		#endregion

		#region function

		public void Start()
		{
			Process.EnableRaisingEvents = true;
			Process.Exited += Process_Exited;
			Process.Start();
			
			if (HasView) {
				View.Show();
			}

			RefreshProcess();
			//var waitTime = (int)TimeSpan.FromSeconds(3).TotalMilliseconds;
			ProcessRunning = !Process.HasExited;
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


	}

}
