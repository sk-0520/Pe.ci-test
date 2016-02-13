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
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
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
            General = new GeneralSettingModel();
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
        /// <summary>
        /// 基本設定。
        /// </summary>
        [DataMember, IsDeepClone]
        public GeneralSettingModel General { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (MainSettingModel)target;

            //RunningInformation.DeepCloneTo(obj.RunningInformation);
            obj.RunningInformation = (RunningInformationSettingModel)RunningInformation.DeepClone();
            Language.DeepCloneTo(obj.Language);
            //Logging.DeepCloneTo(obj.Logging);
            obj.Logging = (LoggingSettingModel)Logging.DeepClone();
            Toolbar.DeepCloneTo(obj.Toolbar);
            WindowSave.DeepCloneTo(obj.WindowSave);
            SystemEnvironment.DeepCloneTo(obj.SystemEnvironment);
            Command.DeepCloneTo(obj.Command);
            Clipboard.DeepCloneTo(obj.Clipboard);
            Template.DeepCloneTo(obj.Template);
            Note.DeepCloneTo(obj.Note);
            //Stream.DeepCloneTo(obj.Stream);
            obj.Stream = (StreamSettingModel)Stream.DeepClone();
            obj.General = (GeneralSettingModel)General.DeepClone();
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
