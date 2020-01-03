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

        public List<Range> EditableHeaderMatchers { get; } = new List<Range>();
        public List<Range> EditableDescriptionMatchers { get; } = new List<Range>();
        public Action? ExecuteAction { get; set; }

        #endregion

        #region IReadOnlyCommandItem

        public abstract string Header { get; }

        public abstract string Description { get; }

        public abstract string Kind { get; }

        public IReadOnlyList<Range> HeaderMatches => EditableHeaderMatchers;

        public IReadOnlyList<Range> DescriptionMatches => EditableDescriptionMatchers;
        public abstract double Score { get; }

        public abstract object GetIcon(IconBox iconBox);
        public void Execute() => ExecuteAction?.Invoke();

        #endregion

    }

    public sealed class LauncherCommandItemElement : CommandItemElementBase
    {
        public LauncherCommandItemElement(LauncherItemElement launcherItemElement, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherItemElement = launcherItemElement;
        }

        #region property

        LauncherItemElement LauncherItemElement { get; }
        public string EditableDescription { get; set; } = string.Empty;
        public string EditableKind { get; set; } = "launcher item";
        public double EditableScore { get; set; }
        #endregion

        #region CommandItemElementBase

        public override string Header => LauncherItemElement.Name;

        public override string Description => EditableDescription;

        public override string Kind => EditableKind;
        public override double Score => EditableScore;

        public override object GetIcon(IconBox iconBox)
        {
            return LauncherItemElement.Icon.IconImageLoaderPack.IconItems[iconBox];
        }

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
