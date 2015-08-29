namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.View.Window;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class TemplateItemViewModel : SingleModelWrapperViewModelBase<TemplateIndexItemModel>, IHavingAppSender, IHavingAppNonProcess, IUnload
	{
		#region variable

		TemplateBodyItemModel _bodyModel = null;
		bool _replaceViewSelected = false;
		string _replaced;

		#endregion

		public TemplateItemViewModel(TemplateIndexItemModel model, IAppSender appSender, IAppNonProcess appNonProcess)
			:base(model)
		{
			AppSender = appSender;
			AppNonProcess = appNonProcess;
		}

		#region property

		ProgramTemplateProcessor Processor { get; set; }

		TemplateBodyItemModel BodyModel 
		{ 
			get
			{
				if (this._bodyModel == null) {
					var body = AppSender.SendLoadIndexBody(IndexKind.Template, Model.Id);
					this._bodyModel = (TemplateBodyItemModel)body;
				}

				return this._bodyModel;
			}
		}

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		public bool IsReplace
		{
			get { return Model.IsReplace; }
			set
			{
				if(SetModelValue(value)) {
					OnPropertyChangedItemType();
				}
			}
		}

		public bool IsProgrammableReplace
		{
			get { return Model.IsProgrammableReplace; }
			set
			{
				if(SetModelValue(value)) {
					OnPropertyChangedItemType();
				}
			}
		}

		public bool ReplaceViewSelected
		{
			get { return this._replaceViewSelected; }
			set
			{
				if (SetVariableValue(ref this._replaceViewSelected, value)) {
					if (value) {
						SetReplacedValue();
					}
				}
			}
		}

		public string Source
		{
			get { return BodyModel.Source; }
			set { SetPropertyValue(BodyModel, value); }
		}

		public string Replaced
		{
			get { return this._replaced; }
			set { SetVariableValue(ref this._replaced, value); }
		}

		public ImageSource ItemTypeImage
		{
			get
			{
				if(IsReplace) {
					if(IsProgrammableReplace) {
						return AppResource.TemplateProgrammableImage;
					} else {
						return AppResource.TemplateReplaceImage;
					}
				}
				return AppResource.TemplatePlainImage;
			}
		}

		public string ItemTypeText
		{
			get
			{
				if(IsReplace) {
					if(IsProgrammableReplace) {
						return "TODO:Programmable";
					} else {
						return "TODO:Replace";
					}
				}
				return "TODO:Plain";
			}
		}

		#endregion

		#region command

		public ICommand SendItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var apiWindow = (WindowsAPIWindowBase)o;
						var hWnd = apiWindow.Handle;
						// TODO: なんだかなぁ。
						SetReplacedValue();
						ClipboardUtility.OutputTextForNextWindow(hWnd, Replaced, AppNonProcess, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						SetReplacedValue();
						ClipboardUtility.CopyText(Replaced, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void OnPropertyChangedItemType()
		{
			OnPropertyChanged("ItemTypeImage");
			OnPropertyChanged("ItemTypeText");
		}

		public void SetReplacedValue()
		{
			if (IsReplace) {
				if(IsProgrammableReplace) {
					Processor = TemplateUtility.MakeTemplateProcessor(BodyModel.Source, Processor, AppNonProcess);
				}
				Replaced = TemplateUtility.ToPlainText(Model, BodyModel, Processor, DateTime.Now, AppNonProcess);
			} else {
				Replaced = Source ?? string.Empty;
			}
		}

		public void SaveBody()
		{
			if (this._bodyModel == null) {
				// 読み込んでない。
				return;
			}
			BodyModel.History.Update();
			AppNonProcess.Logger.Information("save body:" + Name, BodyModel);
			AppSender.SendSaveIndexBody(BodyModel, Model.Id, Timing.Delay);
			ResetChangeFlag();
		}

		void ClearProcessor()
		{
			if(Processor != null) {
				Processor.Dispose();
				Processor = null;
			}
		}

		#endregion

		#region SingleModelWrapperViewModelBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				ClearProcessor();
			}
			base.Dispose(disposing);
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region IUnload

		public bool IsUnloaded { get; private set; }

		public void Unload()
		{
			if (!IsUnloaded) {
				ClearProcessor();
				this._bodyModel = null;
				IsUnloaded = true;
			}
		}

		#endregion
	}
}
