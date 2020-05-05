using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public abstract class CommandItemElementBase: ElementBase, ICommandItem
    {
        public CommandItemElementBase(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        public List<HitValue> EditableHeaderValues { get; } = new List<HitValue>();
        public List<HitValue> EditableDescriptionValues { get; } = new List<HitValue>();
        public int EditableScore { get; set; }

        public Action? ExecuteAction { get; set; }

        #endregion

        #region IReadOnlyCommandItem

        public abstract CommandItemKind Kind { get; }

        public IReadOnlyList<HitValue> HeaderValues => EditableHeaderValues;

        public IReadOnlyList<HitValue> DescriptionValues => EditableDescriptionValues;
        public int Score => EditableScore;

        public abstract object GetIcon(IconBox iconBox);
        public abstract void Execute(IScreen screen, bool isExtend);

        #endregion

    }

    public sealed class LauncherCommandItemElement: CommandItemElementBase
    {
        public LauncherCommandItemElement(LauncherItemElement launcherItemElement, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            LauncherItemElement = launcherItemElement;
            EditableHeaderValues.AddRange(new[] { new HitValue(LauncherItemElement.Name, false) });
        }

        #region property

        LauncherItemElement LauncherItemElement { get; }
        public CommandItemKind EditableKind { get; set; } = CommandItemKind.LauncherItem;
        #endregion

        #region CommandItemElementBase

        public override CommandItemKind Kind => EditableKind;

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

    public sealed class ApplicationCommandUtemElement: CommandItemElementBase
    {
        public ApplicationCommandUtemElement(ApplicationCommandParameter parameter, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Parameter = parameter;
            EditableHeaderValues.AddRange(new[] { new HitValue(Parameter.Header, false) });
            EditableDescriptionValues.AddRange(new[] { new HitValue(Parameter.Description, false) });
        }

        #region property

        ApplicationCommandParameter Parameter { get; }

        #endregion

        #region CommandItemElementBase

        public override CommandItemKind Kind => CommandItemKind.ApplicationCommand;

        public override void Execute(IScreen screen, bool isExtend)
        {
            Parameter.Executor(screen, isExtend);
        }

        public override object GetIcon(IconBox iconBox)
        {
            return Parameter.IconGetter(iconBox);
            //return Application.Current.Resources["pack://application:,,,/Pe.Main;component/Resources/Icon/App.ico"];
        }

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
