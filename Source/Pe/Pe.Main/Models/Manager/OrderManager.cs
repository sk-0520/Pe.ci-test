using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.ViewModels.Command;
using ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.ViewModels.Feedback;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Views.Command;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.Views.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Views.Feedback;
using ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Views.Note;
using ContentTypeTextNet.Pe.Main.Views.NotifyLog;
using ContentTypeTextNet.Pe.Main.Views.Setting;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class OrderWindowParameter
    {
        public OrderWindowParameter(WindowKind windowKind, ElementBase element)
        {
            WindowKind = windowKind;
            Element = element;
        }

        #region property

        public WindowKind WindowKind { get; }
        public ElementBase Element { get; }

        #endregion
    }

    public enum UpdateTarget
    {
        Application,
    }

    public enum UpdateProcess
    {
        Download,
        Update,
    }

    /// <summary>
    /// アプリケーションに対して指示発行を受け付ける役所。
    /// </summary>
    public interface IOrderManager
    {
        #region function

        void AddRedoItem(RedoExecutor redoExecutor);

        void StartUpdate(UpdateTarget target, UpdateProcess process);

        Task<LauncherGroupElement> CreateLauncherGroupElementAsync(LauncherGroupId launcherGroupId, CancellationToken cancellationToken);
        Task<LauncherToolbarElement> CreateLauncherToolbarElementAsync(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, CancellationToken cancellationToken);
        LauncherItemElement GetOrCreateLauncherItemElement(LauncherItemId launcherItemId);
        void RefreshLauncherItemElement(LauncherItemId launcherItemId);

        Task<LauncherItemCustomizeContainerElement> CreateCustomizeLauncherItemContainerElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken);
        Task<ExtendsExecuteElement> CreateExtendsExecuteElementAsync(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen, CancellationToken cancellationToken);
        Task<LauncherExtendsExecuteElement> CreateLauncherExtendsExecuteElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken);

        Task<NoteElement> CreateNoteElementAsync(NoteId noteId, IScreen? screen, NoteStartupPosition startupPosition, CancellationToken cancellationToken);
        bool RemoveNoteElement(NoteId noteId);
        Task<NoteContentElement> CreateNoteContentElementAsync(NoteId noteId, NoteContentKind contentKind, CancellationToken cancellationToken);
        Task<SavingFontElement> CreateFontElementAsync(DefaultFontKind defaultFontKind, FontId fontId, ParentUpdater parentUpdater, CancellationToken cancellationToken);

        Task<StandardInputOutputElement> CreateStandardInputOutputElementAsync(string caption, Process process, IScreen screen, CancellationToken cancellationToken);

        Task<LauncherItemExtensionElement> CreateLauncherItemExtensionElementAsync(IPluginInformation pluginInformation, LauncherItemId launcherItemId, CancellationToken cancellationToken);

        WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element);
        WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element);
        WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element);
        WindowItem CreateNoteWindow(NoteElement element);
        WindowItem CreateCommandWindow(CommandElement element);
        WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element);
        WindowItem CreateNotifyLogWindow(NotifyLogElement element);
        WindowItem CreateSettingWindow(SettingContainerElement element);


        #endregion
    }

    partial class ApplicationManager
    {
        class OrderManagerImpl: ManagerBase, IOrderManager
        {
            public OrderManagerImpl(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region property

            private ConcurrentDictionary<LauncherItemId, LauncherItemElement> LauncherItems { get; } = new ConcurrentDictionary<LauncherItemId, LauncherItemElement>();
            private ISet<RedoExecutor> RedoItems { get; } = new HashSet<RedoExecutor>();

            #endregion

            #region function
            #endregion

            #region IOrderManager

            public void StartUpdate(UpdateTarget target, UpdateProcess process)
            {
                throw new NotSupportedException();
            }

            public void AddRedoItem(RedoExecutor redoExecutor)
            {
                if(!redoExecutor.IsExited) {
                    redoExecutor.Exited += RedoExecutor_Exited;
                    RedoItems.Add(redoExecutor);
                }
            }

            public async Task<LauncherGroupElement> CreateLauncherGroupElementAsync(LauncherGroupId launcherGroupId, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<LauncherGroupElement>(launcherGroupId);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public async Task<LauncherToolbarElement> CreateLauncherToolbarElementAsync(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<LauncherToolbarElement>(dockScreen, launcherGroups);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public LauncherItemElement GetOrCreateLauncherItemElement(LauncherItemId launcherItemId)
            {
                return LauncherItems.GetOrAdd(launcherItemId, launcherItemIdKey => {
                    var launcherItemElement = DiContainer.Build<LauncherItemElement>(launcherItemIdKey);
                    // これたまたま動いてるだけだと思う
                    var task = launcherItemElement.InitializeAsync(CancellationToken.None);
                    task.ConfigureAwait(false);
                    task.Wait();
                    return launcherItemElement;
                });
            }

            /// <inheritdoc cref="IOrderManager.RefreshLauncherItemElement(Guid)"/>
            public void RefreshLauncherItemElement(LauncherItemId launcherItemId)
            {
                if(LauncherItems.TryGetValue(launcherItemId, out var element)) {
                    element.Refresh();
                }
            }

            public async Task<LauncherItemCustomizeContainerElement> CreateCustomizeLauncherItemContainerElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken)
            {
                var customizeLauncherEditorElement = DiContainer.Build<LauncherItemCustomizeEditorElement>(launcherItemId);
                await customizeLauncherEditorElement.InitializeAsync(cancellationToken);
                var customizeLauncherItemContainerElement = DiContainer.Build<LauncherItemCustomizeContainerElement>(screen, customizeLauncherEditorElement);
                await customizeLauncherItemContainerElement.InitializeAsync(cancellationToken);
                return customizeLauncherItemContainerElement;
            }

            public async Task<ExtendsExecuteElement> CreateExtendsExecuteElementAsync(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<ExtendsExecuteElement>(captionName, launcherFileData, launcherEnvironmentVariables, screen);
                await element.InitializeAsync(cancellationToken);
                return element;
            }
            public async Task<LauncherExtendsExecuteElement> CreateLauncherExtendsExecuteElementAsync(LauncherItemId launcherItemId, IScreen screen, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<LauncherExtendsExecuteElement>(launcherItemId, screen);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public async Task<NoteElement> CreateNoteElementAsync(NoteId noteId, IScreen? screen, NoteStartupPosition startupPosition, CancellationToken cancellationToken)
            {
                var element = screen == null
                    ? DiContainer.Build<NoteElement>(noteId, DiDefaultParameter.Create<IScreen>(), startupPosition)
                    : DiContainer.Build<NoteElement>(noteId, screen, startupPosition)
                ;
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public bool RemoveNoteElement(NoteId noteId)
            {
                throw new NotSupportedException($"{nameof(ApplicationManager)}.{nameof(RemoveNoteElement)}");
            }

            public async Task<NoteContentElement> CreateNoteContentElementAsync(NoteId noteId, NoteContentKind contentKind, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<NoteContentElement>(noteId, contentKind);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public async Task<SavingFontElement> CreateFontElementAsync(DefaultFontKind defaultFontKind, FontId fontId, ParentUpdater parentUpdater, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<SavingFontElement>(defaultFontKind, fontId, parentUpdater);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public async Task<StandardInputOutputElement> CreateStandardInputOutputElementAsync(string caption, Process process, IScreen screen, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<StandardInputOutputElement>(caption, process, screen);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public async Task<LauncherItemExtensionElement> CreateLauncherItemExtensionElementAsync(IPluginInformation pluginInformation, LauncherItemId launcherItemId, CancellationToken cancellationToken)
            {
                var element = DiContainer.Build<LauncherItemExtensionElement>(pluginInformation, launcherItemId);
                await element.InitializeAsync(cancellationToken);
                return element;
            }

            public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<LauncherToolbarViewModel>(element);
                });
                var window = DiContainer.BuildView<LauncherToolbarWindow>();
                viewModel.AppDesktopToolbarExtend = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(viewModel);
                    return c.Build<AppDesktopToolbarExtend>(window, element);
                });
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, element, window);
            }

            public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<LauncherItemCustomizeContainerViewModel>(element);
                });
                var window = DiContainer.BuildView<LauncherItemCustomizeWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherCustomize, element, window);
            }

            public WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<ExtendsExecuteViewModel>(element);
                });
                var window = DiContainer.BuildView<ExtendsExecuteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.ExtendsExecute, element, window);
            }

            public WindowItem CreateNoteWindow(NoteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<NoteViewModel>(element);
                });
                var window = DiContainer.BuildView<NoteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Note, element, window);
            }

            public WindowItem CreateCommandWindow(CommandElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<CommandViewModel>(element);
                });
                var window = DiContainer.BuildView<CommandWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Command, element, window);
            }


            public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<StandardInputOutputViewModel>(element);
                });
                var window = DiContainer.BuildView<StandardInputOutputWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.StandardInputOutput, element, window);
            }

            public WindowItem CreateNotifyLogWindow(NotifyLogElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<NotifyLogViewModel>(element);
                });
                var window = DiContainer.BuildView<NotifyLogWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.NotifyLog, element, window);
            }

            public WindowItem CreateSettingWindow(SettingContainerElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<SettingContainerViewModel>(element);
                });
                var window = DiContainer.BuildView<SettingWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Setting, element, window);
            }

            public WindowItem CreateFeedbackWindow(FeedbackElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<FeedbackViewModel>(element);
                });
                var window = DiContainer.BuildView<FeedbackWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Feedback, element, window);
            }

            #endregion

            private void RedoExecutor_Exited(object? sender, EventArgs e)
            {
                var redoExecutor = (RedoExecutor)sender!;
                redoExecutor.Exited -= RedoExecutor_Exited;
                RedoItems.Remove(redoExecutor);
            }

        }
    }
}
