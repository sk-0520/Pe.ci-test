namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// 各設定の統括。
	/// <para>その他の大きいやつは別クラスで管理しとく。</para>
	/// </summary>
	[DataContract(Namespace = ""), Serializable]
	public sealed class MainSettingModel: SettingModelBase
	{
		public MainSettingModel()
		{
			RunningInformation = new RunningInformationItemModel();
			Language = new LanguageItemModel();
			Logging = new LoggingItemModel();
			Toolbar = new ToolbarItemCollectionModel();
			WindowSave = new WindowSaveItemModel();
			SystemEnvironment = new SystemEnvironmentItemModel();
			Clipboard = new ClipboardItemModel();
			Template = new TemplateItemModel();
		}

		[DataMember]
		public RunningInformationItemModel RunningInformation { get; set; }
		[DataMember]
		public LanguageItemModel Language { get; set; }
		[DataMember]
		public LoggingItemModel Logging { get; set; }
		[DataMember]
		public ToolbarItemCollectionModel Toolbar { get; set; }
		[DataMember]
		public WindowSaveItemModel WindowSave { get; set; }
		[DataMember]
		public SystemEnvironmentItemModel SystemEnvironment { get; set; }
		[DataMember]
		public ClipboardItemModel Clipboard { get; set; }
		[DataMember]
		public TemplateItemModel Template { get; set; }
	}
}
