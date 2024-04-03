using System.Collections.Generic;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region function

#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
        private async Task<IReadOnlyList<ApplicationCommandParameter>> CreateApplicationCommandParametersAsync()
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
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
                    _ = ShowAboutViewAsync();
                }),
                factory.CreateParameter(ApplicationCommand.Setting, p => {
                    Debug.Assert(CommandElement != null);
                    CommandElement.HideView(false);
                     _ = ShowSettingViewAsync();
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
                factory.CreateParameter(ApplicationCommand.Proxy, p => {
                    ToggleProxyIsEnabled();
                    var isEnabledProxy = GetProxyIsEnabled();
                    var log = new NotifyMessage(
                        NotifyLogKind.Normal,
                        Properties.Resources.String_Proxy_Toggle_Header,
                        new NotifyLogContent(isEnabledProxy ? Properties.Resources.String_Proxy_Toggle_Content_IsEnabled: Properties.Resources.String_Proxy_Toggle_Content_IsDisabled)
                    );
                    NotifyManager.AppendLogAsync(log);
                }),
                factory.CreateParameter(ApplicationCommand.Help, p => {
                    ShowHelp();
                }),
                factory.CreateParameter(ApplicationCommand.Exception, p => {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => {
                        throw new System.Exception($"{nameof(ApplicationCommand)}.{nameof(ApplicationCommand.Exception)}");
                    });
                }),
            };

            return result;
        }

        #endregion
    }
}
