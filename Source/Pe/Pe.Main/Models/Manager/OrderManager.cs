using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Views.Note;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Views.ExtendsExecute;

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

    /// <summary>
    /// アプリケーションに対して指示発行を受け付ける役所。
    /// </summary>
    public interface IOrderManager
    {
        #region function

        LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId);
        LauncherToolbarElement CreateLauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups);
        LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId);
        LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, Screen screen, LauncherIconElement iconElement);
        ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen);
        LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, Screen screen);

        NoteElement CreateNoteElement(Guid noteId, Screen? screen, NotePosition notePosition);
        bool RemoveNoteElement(Guid noteId);
        NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind);
        FontElement CreateFontElement(Guid fontId, ParentUpdater parentUpdater);

        StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process, Screen screen);

        WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element);
        WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element);
        WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element);
        WindowItem CreateNoteWindow(NoteElement element);
        WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element);

        #endregion
    }

    partial class ApplicationManager
    {
        class OrderManagerImpl : ManagerBase, IOrderManager
        {
            public OrderManagerImpl(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region property

            ConcurrentDictionary<Guid, LauncherItemElement> LauncherItems { get; } = new ConcurrentDictionary<Guid, LauncherItemElement>();

            #endregion

            #region function
            #endregion

            #region IOrderManager

            public LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId)
            {
                var element = DiContainer.Make<LauncherGroupElement>(new object[] { launcherGroupId });
                element.Initialize();
                return element;
            }

            public LauncherToolbarElement CreateLauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
            {
                var element = DiContainer.Make<LauncherToolbarElement>(new object[] { dockScreen, launcherGroups });
                element.Initialize();
                return element;
            }

            public LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId)
            {
                return LauncherItems.GetOrAdd(launcherItemId, launcherItemIdKey => {

                    var launcherIconImageLoaders = EnumUtility.GetMembers<IconBox>()
                        .Select(i => DiContainer.Make<LauncherIconLoader>(new object[] { launcherItemId, i }))
                    ;
                    var iconImageLoaderPack = new IconImageLoaderPack(launcherIconImageLoaders);

                    var launcherIconElement = DiContainer.Make<LauncherIconElement>(new object[] { launcherItemId, iconImageLoaderPack });

                    var launcherItemElement = DiContainer.Make<LauncherItemElement>(new object[] { launcherItemIdKey, launcherIconElement });
                    launcherItemElement.Initialize();
                    return launcherItemElement;
                });
            }

            public LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, Screen screen, LauncherIconElement iconElement)
            {
                var customizeLauncherEditorElement = DiContainer.Build<LauncherItemCustomizeEditorElement>(launcherItemId);
                customizeLauncherEditorElement.Initialize();
                var customizeLauncherItemContainerElement = DiContainer.Build<LauncherItemCustomizeContainerElement>(screen, customizeLauncherEditorElement, iconElement);
                customizeLauncherItemContainerElement.Initialize();
                return customizeLauncherItemContainerElement;
            }

            public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, Screen screen)
            {
                var element = DiContainer.Build<ExtendsExecuteElement>(captionName, launcherFileData, launcherEnvironmentVariables, screen);
                element.Initialize();
                return element;
            }
            public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, Screen screen)
            {
                var element = DiContainer.Build<LauncherExtendsExecuteElement>(launcherItemId, screen);
                element.Initialize();
                return element;
            }

            public NoteElement CreateNoteElement(Guid noteId, Screen? screen, NotePosition notePosition)
            {
                var element = screen == null
                    ? DiContainer.Build<NoteElement>(noteId, DiDefaultParameter.Create<Screen>(), notePosition)
                    : DiContainer.Build<NoteElement>(noteId, screen, notePosition)
                ;
                element.Initialize();
                return element;
            }

            public bool RemoveNoteElement(Guid noteId)
            {
                throw new NotSupportedException($"{nameof(ApplicationManager)}.{nameof(RemoveNoteElement)}");
            }

            public NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind)
            {
                var element = DiContainer.Build<NoteContentElement>(noteId, contentKind);
                element.Initialize();
                return element;
            }

            public FontElement CreateFontElement(Guid fontId, ParentUpdater parentUpdater)
            {
                var element = DiContainer.Build<FontElement>(fontId, parentUpdater);
                element.Initialize();
                return element;
            }

            public StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process, Screen screen)
            {
                var element = DiContainer.Build<StandardInputOutputElement>(id, process, screen);
                element.Initialize();
                return element;
            }


            public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Make<LauncherToolbarViewModel>(new[] { element, });
                });
                var window = DiContainer.BuildView<LauncherToolbarWindow>();
                viewModel.AppDesktopToolbarExtend = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(viewModel);
                    return c.Make<AppDesktopToolbarExtend>(new object[] { window, element, });
                });
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, window);
            }

            public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<LauncherItemCustomizeContainerViewModel>(element);
                });
                var window = DiContainer.BuildView<LauncherItemCustomizeWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherCustomize, window);
            }

            public WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<ExtendsExecuteViewModel>(element);
                });
                var window = DiContainer.BuildView<ExtendsExecuteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.ExtendsExecute, window);
            }

            public WindowItem CreateNoteWindow(NoteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<NoteViewModel>(element);
                });
                var window = DiContainer.BuildView<NoteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Note, window);
            }

            public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<StandardInputOutputViewModel>(element);
                });
                var window = DiContainer.BuildView<StandardInputOutputWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.StandardInputOutput, window);
            }
            #endregion

        }
    }
}
