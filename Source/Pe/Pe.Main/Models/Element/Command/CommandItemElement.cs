using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Command
{
    public abstract class CommandItemElementBase: ElementBase, ICommandItem
    {
        public CommandItemElementBase(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }

        public List<HitValue> EditableHeaderValues { get; } = new List<HitValue>();
        public List<HitValue> EditableDescriptionValues { get; } = new List<HitValue>();
        public int EditableScore { get; set; }

        public Action? ExecuteAction { get; set; }

        #endregion

        #region function

        protected abstract object GetIconImpl(IconBox iconBox);
        protected abstract void ExecuteImpl(ICommandExecuteParameter parameter);

        #endregion

        #region IReadOnlyCommandItem

        public abstract CommandItemKind Kind { get; }

        public IReadOnlyList<HitValue> HeaderValues => EditableHeaderValues;

        public IReadOnlyList<HitValue> DescriptionValues => EditableDescriptionValues;
        public int Score => EditableScore;

        public object GetIcon(IconBox iconBox)
        {
            DispatcherWrapper.VerifyAccess();

            return GetIconImpl(iconBox);
        }
        public void Execute(ICommandExecuteParameter parameter) => ExecuteImpl(parameter);

        #endregion

    }

    public sealed class LauncherCommandItemElement: CommandItemElementBase
    {
        public LauncherCommandItemElement(LauncherItemElement launcherItemElement, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
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

        protected override object GetIconImpl(IconBox iconBox)
        {
            return LauncherItemElement.Icon.IconImageLoaderPack.IconItems[iconBox];
        }

        protected override void ExecuteImpl(ICommandExecuteParameter parameter)
        {
            if(parameter.IsExtend) {
                LauncherItemElement.OpenExtendsExecuteView(parameter.Screen);
            } else {
                LauncherItemElement.Execute(parameter.Screen);
            }
        }

        protected override void InitializeImpl()
        { }

        #endregion
    }

    public sealed class ApplicationCommandUtemElement: CommandItemElementBase
    {
        public ApplicationCommandUtemElement(ApplicationCommandParameter parameter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(dispatcherWrapper, loggerFactory)
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

        protected override void ExecuteImpl(ICommandExecuteParameter parameter)
        {
            Parameter.Executor(parameter);
        }

        protected override object GetIconImpl(IconBox iconBox)
        {
            return Parameter.IconGetter(iconBox);
            //return Application.Current.Resources["pack://application:,,,/Pe.Main;component/Resources/Icon/App.ico"];
        }

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
