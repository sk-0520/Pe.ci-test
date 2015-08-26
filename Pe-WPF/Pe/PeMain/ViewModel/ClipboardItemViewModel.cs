namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.View.Window;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ClipboardItemViewModel : SingleModelWrapperViewModelBase<ClipboardIndexItemModel>, IHavingAppSender, IHavingAppNonProcess, IUnload
	{
		#region define

		const string defineEnabled = "Type";

		#endregion

		#region varable
		
		ClipboardBodyItemModel _bodyModel = null;

		#endregion

		public ClipboardItemViewModel(ClipboardIndexItemModel model, IAppSender appSender, IAppNonProcess appNonProcess)
			:base(model)
		{
			AppSender = appSender;
			AppNonProcess = appNonProcess;
		}

		#region property

		ClipboardBodyItemModel BodyModel
		{
			get
			{
				lock(Model)
				if(this._bodyModel == null) {
					var body = AppSender.SendLoadIndexBody(IndexKind.Clipboard, Model.Id);
					this._bodyModel = (ClipboardBodyItemModel)body;
					if(Model.Type.HasFlag(ClipboardType.Html)) {
						HtmlModel = ClipboardUtility.ConvertClipboardHtmlFromFromRawHtml(this._bodyModel.Html, AppNonProcess);
					} else {
						HtmlModel = null;
					}
					IsUnloaded = false;
				}

				return this._bodyModel;
			}
		}

		ClipboardHtmlData HtmlModel { get; set; }


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
		public bool EnabledClipboardTypesFiles
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
			get { return BodyModel.Rtf ?? string.Empty; }
			set { /* dummy Mode=OneWay */}
		}

		public string HtmlCode
		{
			get 
			{
				if(HtmlModel != null) {
					return HtmlModel.ToHtml();
				} else {
					return null;
				}
			}
		}

		public string HtmlUri
		{
			get 
			{
				if(HtmlModel != null && HtmlModel.SourceURL != null) {
					return HtmlModel.SourceURL.OriginalString;
				} else {
					return string.Empty;
				}
			}
		}

		public BitmapSource Image
		{
			get {
				if(BodyModel.Image != null) {
					return BodyModel.Image;
				} else {
					return null;
				}
			}
		}

		public IEnumerable<ClipboardFileItem> Files
		{
			get
			{
				if(BodyModel.Files != null && BodyModel.Files.Any()) {
					return BodyModel.Files.Select(f => new ClipboardFileItem() {
						Path = f,
						Name = SystemEnvironmentUtility.IsExtensionShow() ? Path.GetFileName(f) : Path.GetFileNameWithoutExtension(f),
					});
				} else {
					return null;
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

						ClipboardUtility.OutputTextForNextWindow(hWnd, Text, AppNonProcess, AppNonProcess.ClipboardWatcher);
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
						var clipboardItem = new ClipboardData() {
							Type = Model.Type,
							Body = BodyModel,
						};

						ClipboardUtility.CopyClipboardItem(clipboardItem, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyTextCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ClipboardUtility.CopyText(Text, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyRtfCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ClipboardUtility.CopyRtf(Rtf, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyHtmlCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ClipboardUtility.CopyHtml(BodyModel.Html, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyImageCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ClipboardUtility.CopyImage(BodyModel.Image, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand CopyFilesCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ClipboardUtility.CopyFile(BodyModel.Files, AppNonProcess.ClipboardWatcher);
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void SetClipboardType(object obj, ClipboardType nowValue, ClipboardType clipboardType, string memberName, [CallerMemberName]string propertyName = "")
		{
			SetPropertyValue(obj, nowValue ^ clipboardType, memberName, propertyName);
		}

		#endregion

		#region SingleModelWrapperViewModelBase

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
				this._bodyModel = null;
				IsUnloaded = true;
			}
		}

		#endregion
	}
}
