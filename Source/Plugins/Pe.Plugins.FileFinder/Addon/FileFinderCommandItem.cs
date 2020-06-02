using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder.Addon
{
    internal class FileFinderCommandItem: ICommandItem
    {
        public FileFinderCommandItem(string path)
        {
            Path = path;
        }

        #region property

        private string Path { get; }

        #endregion


        #region ICommandItem

        public CommandItemKind Kind { get; } = CommandItemKind.Plugin;

        public List<HitValue> HeaderValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.HeaderValues => HeaderValues;

        public List<HitValue> DescriptionValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.DescriptionValues => DescriptionValues;

        public List<HitValue> ExtendDescriptionValues { get; } = new List<HitValue>();
        IReadOnlyList<HitValue> ICommandItem.ExtendDescriptionValues => ExtendDescriptionValues;

        public int Score { get; internal set; }

        public string FullMatchValue => Path;

        public void Execute(ICommandExecuteParameter parameter)
        {
            throw new NotImplementedException();
        }

        public object GetIcon(IconBox iconBox)
        {
            return null!;
        }

        public bool IsEquals(ICommandItem? commandItem)
        {
            if(commandItem == null) {
                return false;
            }

            if(Kind != commandItem.Kind) {
                return false;
            }

            if(commandItem is FileFinderCommandItem fileCommandItem) {
                var a = System.IO.Path.TrimEndingDirectorySeparator(Path);
                var b = System.IO.Path.TrimEndingDirectorySeparator(fileCommandItem.Path);

                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        #endregion
    }
}
