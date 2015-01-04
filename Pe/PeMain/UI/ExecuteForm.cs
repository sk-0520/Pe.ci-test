using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 指定して実行。
	/// </summary>
	public partial class ExecuteForm : Form, ISetCommonData
	{
		#region define
		#endregion ////////////////////////////////////

		#region variable
		#endregion ////////////////////////////////////

		public ExecuteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		CommonData CommonData { get; set; }
		LauncherItem LauncherItem { get; set; }
		IEnumerable<string> ExOptions { get; set; }

		public LauncherItem EditedLauncherItem { get; private set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			PointingUtility.AttachmentDefaultButton(this);
		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);

			var map = new Dictionary<string, string>() {
				{ AppLanguageName.itemName, LauncherItem.Name },
			};

			UIUtility.SetDefaultText(this, CommonData.Language, map);

			this.envUpdate.SetLanguage(CommonData.Language);
			this.envRemove.SetLanguage(CommonData.Language);

			/*
			this.pageBasic.Text = CommonData.Language["execute/tab/basic"];
			this.pageEnv.Text = CommonData.Language["common/tab/env"];
			this.labelOption.Text = CommonData.Language["execute/label/option"];
			this.labelWorkDirPath.Text = CommonData.Language["execute/label/work-dir"];
			this.selectStdStream.Text  = CommonData.Language["execute/check/std-stream"];
			this.selectEnvironment.Text  = CommonData.Language["execute/check/edit-env"];
			this.groupUpdate.Text = CommonData.Language["common/label/edit"];
			this.groupRemove.Text = CommonData.Language["common/label/remove"];
			this.selectAdministrator.Text = CommonData.Language["common/check/admin"];
			*/
			this.tabExecute_pageBasic.SetLanguage(CommonData.Language);
			this.tabExecute_pageEnv.SetLanguage(CommonData.Language);
			this.labelOption.SetLanguage(CommonData.Language);
			this.labelWorkDirPath.SetLanguage(CommonData.Language);
			this.selectStdStream.SetLanguage(CommonData.Language);
			this.selectEnvironment.SetLanguage(CommonData.Language);
			this.groupUpdate.SetLanguage(CommonData.Language);
			this.groupRemove.SetLanguage(CommonData.Language);
			this.selectAdministrator.SetLanguage(CommonData.Language);

		}
		#endregion ////////////////////////////////////

		#region function
		public void SetParameter(LauncherItem launcherItem, IEnumerable<string> exOptions)
		{
			LauncherItem = launcherItem;
			ExOptions = exOptions;
		}

		void ApplySetting()
		{
			Debug.Assert(LauncherItem != null);

			ApplyLanguage();

			Icon = LauncherItem.GetIcon(IconScale.Small, LauncherItem.IconItem.Index, CommonData.ApplicationSetting);

			this.viewCommand.Text = LauncherItem.Command;
			this.inputOption.Items.AddRange(LauncherItem.LauncherHistory.Options.ToArray());
			this.inputOption.Text = LauncherItem.Option;
			this.inputWorkDirPath.Items.AddRange(LauncherItem.LauncherHistory.WorkDirs.ToArray());
			this.inputWorkDirPath.Text = LauncherItem.WorkDirPath;
			this.selectStdStream.Checked = LauncherItem.StdOutputWatch;
			this.selectAdministrator.Checked = LauncherItem.Administrator;
			this.selectEnvironment.Checked = !this.selectEnvironment.Checked;
			this.selectEnvironment.Checked = LauncherItem.EnvironmentSetting.EditEnvironment;
			this.envUpdate.SetItem(LauncherItem.EnvironmentSetting.Update.ToDictionary(pair => pair.First, pair => pair.Second));
			this.envRemove.SetItem(LauncherItem.EnvironmentSetting.Remove);

			if(ExOptions != null && ExOptions.Any()) {
				var args = string.Join(" ", ExOptions.WhitespaceToQuotation());
				this.inputOption.Text = args;
			}
		}

		void SubmitInput()
		{
			var item = (LauncherItem)LauncherItem.Clone();
			item.Option = this.inputOption.Text;
			item.WorkDirPath = this.inputWorkDirPath.Text;
			item.StdOutputWatch = this.selectStdStream.Checked;
			item.Administrator = this.selectAdministrator.Checked;

			item.EnvironmentSetting.EditEnvironment = this.selectEnvironment.Checked;
			if(item.EnvironmentSetting.EditEnvironment) {
				item.EnvironmentSetting.Update = this.envUpdate.Items.ToList();
				item.EnvironmentSetting.Remove = this.envRemove.Items.ToList();
			}
			EditedLauncherItem = item;
		}
		#endregion ////////////////////////////////////

		void CommandOption_file_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogFilePath(this.inputOption);
		}
		
		void CommandOption_dir_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputOption);
		}
		
		void CommandWorkDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputWorkDirPath);
		}
		
		void SelectUserDefault_CheckedChanged(object sender, EventArgs e)
		{
			var enabled = this.selectEnvironment.Checked;
			envUpdate.Enabled = enabled;
			envRemove.Enabled = enabled;
		}
		
		void CommandSubmit_Click(object sender, EventArgs e)
		{
			SubmitInput();
			DialogResult = DialogResult.OK;
			Close();
		}
		
		void TabExecute_pageBasic_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
			}
		}
		
		void TabExecute_pageBasic_DragDrop(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				var args = string.Join(" ", dragDatas.WhitespaceToQuotation());
				this.inputOption.Text = args;
			}
		}
		
		void InputWorkDirPath_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				if(dragDatas.Count() == 1) {
					var path = dragDatas.First();
					
					e.Effect = DragDropEffects.Copy;
				}
			}
		}
		
		void InputWorkDirPath_DragDrop(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var dragDatas = (IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop);
				if(dragDatas.Count() == 1) {
					var path = dragDatas.First();
					if(FileUtility.Exists(path)) {
						var isDir = Directory.Exists(path);
						if(!isDir) {
							this.inputWorkDirPath.Text = Path.GetDirectoryName(path);
						} else {
							this.inputWorkDirPath.Text = path;
						}
					}
				}
			}
		}

		private void commandCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
