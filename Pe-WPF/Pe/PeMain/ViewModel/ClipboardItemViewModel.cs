namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
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
		public ClipboardItemViewModel(ClipboardIndexItemModel model, IAppSender appSender, IClipboardWatcher clipboardWatcher, INonProcess nonProcess, VariableConstants variableConstants)
			:base(model)
		{
			AppSender = appSender;
			ClipboardWatcher = clipboardWatcher;
			NonProcess = nonProcess;
			VariableConstants = variableConstants;
		}

		#region property

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
