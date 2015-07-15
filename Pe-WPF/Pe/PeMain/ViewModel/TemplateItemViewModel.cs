﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class TemplateItemViewModel: SingleModelWrapperViewModelBase<TemplateIndexItemModel>, IHavingAppSender, IHavingClipboardWatcher, IHavingNonProcess, IHavingVariableConstants
	{
		#region variable

		TemplateBodyItemModel _bodyModel = null;
		bool _replaceViewSelected = false;
		string _replaced;

		#endregion

		public TemplateItemViewModel(TemplateIndexItemModel model, IAppSender appSender, IClipboardWatcher clipboardWatcher, INonProcess nonProcess, VariableConstants variableConstants)
			:base(model)
		{
			AppSender = appSender;
			ClipboardWatcher = clipboardWatcher;
			NonProcess = nonProcess;
			VariableConstants = variableConstants;
		}

		#region property

		ProgramTemplateProcessor Processor { get; set; }

		TemplateBodyItemModel BodyModel 
		{ 
			get
			{
				if (this._bodyModel == null) {
					var body = AppSender.SendGetIndexBody(IndexKind.Template, Model.Id);
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
			set { SetVariableValue(ref _replaced, value); }
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

		#region function

		void OnPropertyChangedItemType()
		{
			OnPropertyChanged("ItemTypeImage");
			OnPropertyChanged("ItemTypeText");
		}

		void SetReplacedValue()
		{
			if (IsReplace) {
				Processor = TemplateUtility.MakeTemplateProcessor(BodyModel.Source, Processor, NonProcess);
				Replaced = TemplateUtility.ToPlainText(Model, BodyModel, Processor, DateTime.Now, NonProcess);
			}
		}

		public void SaveBody()
		{
			if (this._bodyModel == null) {
				// 読み込んでない。
				return;
			}
			BodyModel.History.Update();
			NonProcess.Logger.Information("save body:" + Name, BodyModel);
			AppSender.SendSaveIndexBody(BodyModel, Model.Id);
			ResetChangeFlag();
		}

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				if (Processor != null) {
					Processor.Dispose();
					Processor = null;
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingVariableConstants

		public VariableConstants VariableConstants { get; private set; }

		#endregion
	}
}
