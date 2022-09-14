using System.Collections.Generic;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Command;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region function

        private IReadOnlyList<ApplicationCommandParameter> CreateApplicationCommandParameters()
        {
            var factory = ApplicationDiContainer.Build<ApplicationCommandParameterFactory>();

            var result = new ApplicationCommandParameter[] {
                factory.CreateParameter(ApplicationCommand.Close, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                }),
                factory.CreateParameter(ApplicationCommand.Exit, p => {
                    // 拡張機能としてアップデート無視
                    Exit(p.IsExtend);
                }),
                factory.CreateParameter(ApplicationCommand.Reboot, p => {
                    Reboot();
                }),
                factory.CreateParameter(ApplicationCommand.About, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                    ShowAboutView();
                }),
                factory.CreateParameter(ApplicationCommand.Setting, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                    ShowSettingView();
                }),
                factory.CreateParameter(ApplicationCommand.GarbageCollection, p => {
                    GarbageCollection(p.IsExtend);
                }),
                factory.CreateParameter(ApplicationCommand.CopyInformation, p => {
                    var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
                    var s = p.IsExtend
                        ? infoCollector.GetLongInformation()
                        : infoCollector.GetShortInformation()
                    ;
                    ClipboardManager.CopyText(s, ClipboardNotify.User);
                }),
                factory.CreateParameter(ApplicationCommand.Help, p => {
                    ShowHelp();
                }),
#if DEBUG
                factory.CreateParameter(ApplicationCommand.Exception, p => {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => {
                        throw new System.Exception($"{nameof(ApplicationCommand)}.{nameof(ApplicationCommand.Exception)}");
                    });
                }),
#endif
            };

            return result;
        }

        #endregion
    }
}
