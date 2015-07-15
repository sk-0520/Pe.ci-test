namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			set { SetModelValue(value); }
		}

		public bool IsProgrammableReplace
		{
			get { return Model.IsProgrammableReplace; }
			set { SetModelValue(value); }
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

		#endregion

		#region function

		void SetProcessor()
		{
			//TODO:Processor.CultureCode = 
			if (Processor == null) {
				var processor = new ProgramTemplateProcessor() {
					TemplateSource = Source ?? string.Empty,
				};
				Processor = processor;
			} else {
				Processor.TemplateSource = Source ?? string.Empty;
			}
		}

		string ExecuteProcessor(ProgramTemplateProcessor processor)
		{
			if (processor.Compiled) {
				return processor.TransformText();
			}
			processor.AllProcess();
			if (processor.Error != null || processor.GeneratedErrorList.Any() || processor.CompileErrorList.Any()) {
				// エラーあり
				if (processor.Error != null) {
					return processor.Error.ToString() + Environment.NewLine + string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => e.ToString()));
				} else {
					return string.Join(Environment.NewLine, processor.GeneratedErrorList.Concat(processor.CompileErrorList).Select(e => string.Format("[{0},{1}] {2}: {3}", e.Line - processor.FirstLineNumber, e.Column, e.ErrorNumber, e.ErrorText)));
				}
			}
			return processor.TransformText();
		}

		void SetReplacedValue()
		{
			if (IsProgrammableReplace) {
				SetProcessor();
				var result = ExecuteProcessor(Processor);
				Replaced = result;
			}
		}

		public void SaveBody()
		{
			if (this._bodyModel == null) {
				// 読み込んでない。
				return;
			}
			NonProcess.Logger.Information("save body:" + Name, BodyModel);
			AppSender.SendSaveIndexBody(BodyModel, Model.Id);
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
