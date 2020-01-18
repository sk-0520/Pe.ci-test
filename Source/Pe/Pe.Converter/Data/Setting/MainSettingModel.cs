/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
    /// <summary>
    /// 各設定の統括。
    /// <para>その他の大きいやつは別クラスで管理しとく。</para>
    /// </summary>
    [DataContract(Namespace = ""), Serializable]
    public sealed class MainSettingModel: SettingModelBase
    {
        public MainSettingModel()
        { }

        #region property

        [DataMember, IsDeepClone]
        public RunningInformationSettingModel RunningInformation { get; set; } = new RunningInformationSettingModel();
        [DataMember, IsDeepClone]
        public LanguageSettingModel Language { get; set; } = new LanguageSettingModel();
        [DataMember, IsDeepClone]
        public LoggingSettingModel Logging { get; set; } = new LoggingSettingModel();
        [DataMember, IsDeepClone]
        public ToolbarSettingModel Toolbar { get; set; } = new ToolbarSettingModel();
        [DataMember, IsDeepClone]
        public WindowSaveSettingModel WindowSave { get; set; } = new WindowSaveSettingModel();
        [DataMember, IsDeepClone]
        public SystemEnvironmentSettingModel SystemEnvironment { get; set; } = new SystemEnvironmentSettingModel();
        [DataMember, IsDeepClone]
        public CommandSettingModel Command { get; set; } = new CommandSettingModel();
        [DataMember, IsDeepClone]
        public ClipboardSettingModel Clipboard { get; set; } = new ClipboardSettingModel();
        [DataMember, IsDeepClone]
        public TemplateSettingModel Template { get; set; } = new TemplateSettingModel();
        [DataMember, IsDeepClone]
        public NoteSettingModel Note { get; set; } = new NoteSettingModel();
        [DataMember, IsDeepClone]
        public StreamSettingModel Stream { get; set; } = new StreamSettingModel();
        /// <summary>
        /// 基本設定。
        /// </summary>
        [DataMember, IsDeepClone]
        public GeneralSettingModel General { get; set; } = new GeneralSettingModel();

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (MainSettingModel)target;

        //    //RunningInformation.DeepCloneTo(obj.RunningInformation);
        //    obj.RunningInformation = (RunningInformationSettingModel)RunningInformation.DeepClone();
        //    //Language.DeepCloneTo(obj.Language);
        //    obj.Language = (LanguageSettingModel)Language.DeepClone();
        //    //Logging.DeepCloneTo(obj.Logging);
        //    obj.Logging = (LoggingSettingModel)Logging.DeepClone();
        //    //Toolbar.DeepCloneTo(obj.Toolbar);
        //    obj.Toolbar = (ToolbarSettingModel)Toolbar.DeepClone();
        //    //WindowSave.DeepCloneTo(obj.WindowSave);
        //    obj.WindowSave = (WindowSaveSettingModel)WindowSave.DeepClone();
        //    //SystemEnvironment.DeepCloneTo(obj.SystemEnvironment);
        //    obj.SystemEnvironment = (SystemEnvironmentSettingModel)SystemEnvironment.DeepClone();
        //    //Command.DeepCloneTo(obj.Command);
        //    obj.Command = (CommandSettingModel)Command.DeepClone();
        //    //Clipboard.DeepCloneTo(obj.Clipboard);
        //    obj.Clipboard = (ClipboardSettingModel)Clipboard.DeepClone();
        //    //Template.DeepCloneTo(obj.Template);
        //    obj.Template = (TemplateSettingModel)Template.DeepClone();
        //    //Note.DeepCloneTo(obj.Note);
        //    obj.Note = (NoteSettingModel)Note.DeepClone();
        //    //Stream.DeepCloneTo(obj.Stream);
        //    obj.Stream = (StreamSettingModel)Stream.DeepClone();
        //    obj.General = (GeneralSettingModel)General.DeepClone();
        //}


        #endregion
    }
}
