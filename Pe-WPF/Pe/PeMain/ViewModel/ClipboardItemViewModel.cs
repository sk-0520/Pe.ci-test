namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Documents;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ClipboardItemViewModel : SingleModelWrapperViewModelBase<ClipboardIndexItemModel>, IHavingAppSender, IHavingClipboardWatcher, IHavingNonProcess, IHavingVariableConstants
	{
		#region define

		const string defineEnabled = "Type";

		#endregion

		#region
		
		ClipboardBodyItemModel _bodyModel = null;

		#endregion

		public ClipboardItemViewModel(ClipboardIndexItemModel model, IAppSender appSender, IClipboardWatcher clipboardWatcher, INonProcess nonProcess, VariableConstants variableConstants)
			:base(model)
		{
			AppSender = appSender;
			ClipboardWatcher = clipboardWatcher;
			NonProcess = nonProcess;
			VariableConstants = variableConstants;
		}

		#region property

		ClipboardBodyItemModel BodyModel
		{
			get
			{
				if(this._bodyModel == null) {
					var body = AppSender.SendGetIndexBody(IndexKind.Clipboard, Model.Id);
					this._bodyModel = (ClipboardBodyItemModel)body;
				}

				return this._bodyModel;
			}
		}

		public ImageSource ItemTypeImage
		{
			get
			{
				var type = ClipboardUtility.GetSingleClipboardType(Model.Type);
				var map = new Dictionary<ClipboardType, ImageSource>() {
					{ ClipboardType.Text, AppResource.ClipboardTextFormatImage },
					{ ClipboardType.Rtf, AppResource.ClipboardRichTextFormatImage },
					{ ClipboardType.Html, AppResource.ClipboardHtmlImage },
					{ ClipboardType.Image, AppResource.ClipboardImageImage },
					{ ClipboardType.File, AppResource.ClipboardFileImage },
				};

				return map[type];
			}
		}

		public DateTime CreateTimestamp { get { return Model.History.CreateTimestamp; } }

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		#region Type

		public bool EnabledClipboardTypesText
		{
			get { return Model.Type.HasFlag(ClipboardType.Text); }
			set { SetClipboardType(Model, Model.Type, ClipboardType.Text, defineEnabled); }
		}
		public bool EnabledClipboardTypesRtf
		{
			get { return Model.Type.HasFlag(ClipboardType.Rtf); }
			set { SetClipboardType(Model, Model.Type, ClipboardType.Rtf, defineEnabled); }
		}
		public bool EnabledClipboardTypesHtml
		{
			get { return Model.Type.HasFlag(ClipboardType.Html); }
			set { SetClipboardType(Model, Model.Type, ClipboardType.Html, defineEnabled); }
		}
		public bool EnabledClipboardTypesImage
		{
			get { return Model.Type.HasFlag(ClipboardType.Image); }
			set { SetClipboardType(Model, Model.Type, ClipboardType.Image, defineEnabled); }
		}
		public bool EnabledClipboardTypesFile
		{
			get { return Model.Type.HasFlag(ClipboardType.File); }
			set { SetClipboardType(Model, Model.Type, ClipboardType.File, defineEnabled); }
		}
	
		#endregion

		public string Text
		{
			get { return BodyModel.Text ?? string.Empty; }
		}

		//public FlowDocument Rtf
		//{
		//	get
		//	{
		//		//return BodyModel.Rtf;
		//		var result = new FlowDocument();
		//		return result;
		//	}
		//}

		public string Rtf
		{
			get { return BodyModel.Rtf; }
			set { /* dummy Mode=OneWay */}
		}

		#endregion

		#region function

		void SetClipboardType(object obj, ClipboardType nowValue, ClipboardType clipboardType, string memberName, [CallerMemberName]string propertyName = "")
		{
			SetPropertyValue(obj, nowValue ^ clipboardType, memberName, propertyName);
		}

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

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
