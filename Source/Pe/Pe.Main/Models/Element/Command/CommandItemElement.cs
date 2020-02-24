using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public abstract class CommandItemElementBase : ElementBase, ICommandItem
    {
        public CommandItemElementBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        public List<HitValue> EditableHeaderValues { get; } = new List<HitValue>();
        public List<HitValue> EditableDescriptionValues { get; } = new List<HitValue>();
        public Action? ExecuteAction { get; set; }

        #endregion

        #region IReadOnlyCommandItem

        public abstract CommandItemKind Kind { get; }

        public IReadOnlyList<HitValue> HeaderValues => EditableHeaderValues;

        public IReadOnlyList<HitValue> DescriptionValues => EditableDescriptionValues;
        public abstract int Score { get; }

        public abstract object GetIcon(IconBox iconBox);
        public abstract void Execute(IScreen screen, bool isExtend);

        #endregion

    }

    public sealed class LauncherCommandItemElement : CommandItemElementBase
    {
        public LauncherCommandItemElement(LauncherItemElement launcherItemElement, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherItemElement = launcherItemElement;
            EditableHeaderValues.AddRange(new[] { new HitValue(LauncherItemElement.Name, false) });
        }

        #region property

        LauncherItemElement LauncherItemElement { get; }
        public CommandItemKind EditableKind { get; set; } = CommandItemKind.LauncherItem;
        public int EditableScore { get; set; }
        #endregion

        #region CommandItemElementBase

        public override CommandItemKind Kind => EditableKind;
        public override int Score => EditableScore;

        public override object GetIcon(IconBox iconBox)
        {
            return LauncherItemElement.Icon.IconImageLoaderPack.IconItems[iconBox];
        }

        public override void Execute(IScreen screen, bool isExtend)
        {
            if(isExtend) {
                LauncherItemElement.OpenExtendsExecuteView(screen);
            } else {
                LauncherItemElement.Execute(screen);
            }
        }

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
