namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	/// <summary>
	/// 各設定の統括。
	/// <para>その他の大きいやつは別クラスで管理しとく。</para>
	/// </summary>
	[DataContract(Namespace = ""), Serializable]
	public sealed class MainSettingModel: SettingModelBase, IDeepClone
	{
		public MainSettingModel()
		{
			RunningInformation = new RunningInformationSettingModel();
			Language = new LanguageSettingModel();
			Logging = new LoggingSettingModel();
			Toolbar = new ToolbarSettingModel();
			WindowSave = new WindowSaveSettingModel();
			SystemEnvironment = new SystemEnvironmentSettingModel();
			Command = new CommandSettingModel();
			Clipboard = new ClipboardSettingModel();
			Template = new TemplateSettingModel();
			Note = new NoteSettingModel();
			Stream = new StreamSettingModel();
		}

		#region property

		[DataMember]
		public RunningInformationSettingModel RunningInformation { get; set; }
		[DataMember]
		public LanguageSettingModel Language { get; set; }
		[DataMember]
		public LoggingSettingModel Logging { get; set; }
		[DataMember]
		public ToolbarSettingModel Toolbar { get; set; }
		[DataMember]
		public WindowSaveSettingModel WindowSave { get; set; }
		[DataMember]
		public SystemEnvironmentSettingModel SystemEnvironment { get; set; }
		[DataMember]
		public CommandSettingModel Command { get; set; }
		[DataMember]
		public ClipboardSettingModel Clipboard { get; set; }
		[DataMember]
		public TemplateSettingModel Template { get; set; }
		[DataMember]
		public NoteSettingModel Note { get; set; }
		[DataMember]
		public StreamSettingModel Stream { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (MainSettingModel)target;

			RunningInformation.DeepCloneTo(obj.RunningInformation);
			Language.DeepCloneTo(obj.Language);
			Logging.DeepCloneTo(obj.Logging);
			Toolbar.DeepCloneTo(obj.Toolbar);
			WindowSave.DeepCloneTo(obj.WindowSave);
			SystemEnvironment.DeepCloneTo(obj.SystemEnvironment);
			Command.DeepCloneTo(obj.Command);
			Clipboard.DeepCloneTo(obj.Clipboard);
			Template.DeepCloneTo(obj.Template);
			Note.DeepCloneTo(obj.Note);
			Stream.DeepCloneTo(obj.Stream);
		}

		public IDeepClone DeepClone()
		{
			var result = new MainSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
