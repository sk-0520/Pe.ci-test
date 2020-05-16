using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    partial class ApplicationManager
    {
        #region function

        IReadOnlyList<ApplicationCommandParameter> CreateApplicationCommandParameters()
        {
            Debug.Assert(CommandElement == null);

            var factory = ApplicationDiContainer.Build<ApplicationCommandParameterFactory>();

            var result = new ApplicationCommandParameter[] {
                factory.CreateParameter(ApplicationCommand.Close, p => {
                    CommandElement!.HideView(false);
                }),
                factory.CreateParameter(ApplicationCommand.Exit, p => {
                    Exit(false);
                }),
                factory.CreateParameter(ApplicationCommand.Shutdown, p => {
                    Exit(true);
                }),
                factory.CreateParameter(ApplicationCommand.Reboot, p => {
                    Reboot();
                }),
                factory.CreateParameter(ApplicationCommand.About, p => {
                    CommandElement!.HideView(false);
                    ShowAboutView();
                }),
                factory.CreateParameter(ApplicationCommand.Setting, p => {
                    CommandElement!.HideView(false);
                    ShowSettingView();
                }),
                factory.CreateParameter(ApplicationCommand.GarbageCollection, p => {
                    var old = GC.GetTotalMemory(false);
                    GC.Collect(0);
                    GC.Collect(1);
                    var now = GC.GetTotalMemory(false);
                    var sizeConverter = ApplicationDiContainer.Build<Core.Models.SizeConverter>();
                    Logger.LogInformation(
                        "GC: {0}({1}) -> {2}({3}), diff: {4}({5})",
                        sizeConverter.ConvertHumanLikeByte(old), old,
                        sizeConverter.ConvertHumanLikeByte(now), now,
                        sizeConverter.ConvertHumanLikeByte(old - now), old - now
                    );
                }),
                factory.CreateParameter(ApplicationCommand.GarbageCollectionFull, p => {
                    var old = GC.GetTotalMemory(false);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    var now = GC.GetTotalMemory(false);
                    var sizeConverter = ApplicationDiContainer.Build<Core.Models.SizeConverter>();
                    Logger.LogInformation(
                        "GC(FULL): {0}({1}) -> {2}({3}), diff: {4}({5})",
                        sizeConverter.ConvertHumanLikeByte(old), old,
                        sizeConverter.ConvertHumanLikeByte(now), now,
                        sizeConverter.ConvertHumanLikeByte(old - now), old - now
                    );
                }),
                factory.CreateParameter(ApplicationCommand.CopyShortInformation, p => {
                    var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
                    var s = infoCollector.GetShortInformation();
                    ClipboardManager.CopyText(s, ClipboardNotify.User);
                }),
                factory.CreateParameter(ApplicationCommand.CopyLongInformation, p => {
                    var infoCollector = ApplicationDiContainer.Build<ApplicationInformationCollector>();
                    var s = infoCollector.GetLongInformation();
                    ClipboardManager.CopyText(s, ClipboardNotify.User);
                }),
                factory.CreateParameter(ApplicationCommand.Help, p => {
                    ShowHelp();
                }),
            };

            return result;
        }

        #endregion
    }
}
