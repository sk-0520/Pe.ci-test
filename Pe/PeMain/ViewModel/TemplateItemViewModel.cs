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

	public class TemplateItemViewModel: SingleModelWrapperViewModelBase<TemplateIndexItemModel>, IHavingAppSender, IHavingAppNonProcess
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
					body.Disposing += Body_Disposing;
					this._bodyModel = (TemplateBodyItemModel)body;
					if(IsDisposed) {
						// 再度読み込まれた
						IsDisposed = false;
					}
				}

				return this._bodyModel;
			}
		}

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		public TemplateReplaceMode TemplateReplaceMode
		{
			get { return Model.TemplateReplaceMode; }
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

		//public ImageSource ItemTypeImage
		//{
		//	get
		//	{
		//		switch(TemplateReplaceMode) {
		//			case Library.PeData.Define.TemplateReplaceMode.None:
		//				return AppResource.TemplatePlainImage;
		//			case Library.PeData.Define.TemplateReplaceMode.Text:
		//				return AppResource.TemplateReplaceImage;
		//			case Library.PeData.Define.TemplateReplaceMode.Program:
		//				return AppResource.TemplateProgrammableImage;
		//			default:
		//				throw new NotImplementedException();
		//		}
		//	}
		//}

		public bool IsReplace
		{
			get { return TemplateReplaceMode != TemplateReplaceMode.None; }
		}

		public string ItemTypeText
		{
			get
			{
				switch(TemplateReplaceMode) {
					case TemplateReplaceMode.None:
						return AppNonProcess.Language["template/replace/plain"];
					case TemplateReplaceMode.Text:
						return AppNonProcess.Language["template/replace/text"];
					case TemplateReplaceMode.Program:
						return AppNonProcess.Language["template/replace/program"];
					default:
						throw new NotImplementedException();
				}
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
			var propertyNames = new[] {
				"IsReplace",
//				"ItemTypeImage",
//				"ItemTypeText",
			};
			CallOnPropertyChange(propertyNames);
		}

		public void SetReplacedValue()
		{
			if(TemplateReplaceMode == TemplateReplaceMode.None) {
				Replaced = Source ?? string.Empty;
			} else {
				if(TemplateReplaceMode == TemplateReplaceMode.Program) {
					Processor = TemplateUtility.MakeTemplateProcessor(BodyModel.Source, Processor, AppNonProcess);
				}
				Replaced = TemplateUtility.ToPlainText(Model, BodyModel, Processor, DateTime.Now, AppNonProcess);
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

		#endregion

		#region SingleModelWrapperViewModelBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				if(Processor != null) {
					Processor.Dispose();
					Processor = null;
				}
				if(this._bodyModel != null) {
					this._bodyModel.Disposing -= Body_Disposing;
					this._bodyModel = null;
				}
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

		void Body_Disposing(object sender, EventArgs e)
		{
			Dispose();
		}
	}
}
