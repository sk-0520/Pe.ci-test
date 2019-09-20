using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Element;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.View.Extend;
using ContentTypeTextNet.Pe.Main.View.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.View.Note;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.Note;
using ContentTypeTextNet.Pe.Main.Model.Element.Font;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Main.Model.Element.StandardInputOutput;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Main.ViewModel.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.View.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherItem;
using ContentTypeTextNet.Pe.Main.View.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
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
        CustomizeLauncherItemElement CreateCustomizeLauncherItemElement(Guid launcherItemId, LauncherIconElement iconElement, Screen screen);
        NoteElement CreateNoteElement(Guid noteId, Screen screen, NotePosition notePosition);
        bool RemoveNoteElement(Guid noteId);
        NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind);
        FontElement CreateFontElement(Guid fontId, ParentUpdater parentUpdater);

        StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process);

        WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element);
        WindowItem CreateCustomizeLauncherItemWindow(CustomizeLauncherItemElement element);
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

                    var launcherIconImageLoaders = EnumUtility.GetMembers<IconScale>()
                        .Select(i => DiContainer.Make<LauncherIconLoader>(new object[] { launcherItemId, i }))
                    ;
                    var iconImageLoaderPack = new IconImageLoaderPack(launcherIconImageLoaders);

                    var launcherIconElement = DiContainer.Make<LauncherIconElement>(new object[] { launcherItemId, iconImageLoaderPack });

                    var launcherItemElement = DiContainer.Make<LauncherItemElement>(new object[] { launcherItemIdKey, launcherIconElement });
                    launcherItemElement.Initialize();
                    return launcherItemElement;
                });
            }

            public CustomizeLauncherItemElement CreateCustomizeLauncherItemElement(Guid launcherItemId, LauncherIconElement iconElement, Screen screen)
            {
                var customizeLauncherItemElement = DiContainer.Build<CustomizeLauncherItemElement>(launcherItemId, iconElement, screen);
                customizeLauncherItemElement.Initialize();
                return customizeLauncherItemElement;
            }


            public NoteElement CreateNoteElement(Guid noteId, Screen screen, NotePosition notePosition)
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

            public StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process)
            {
                var element = DiContainer.Build<StandardInputOutputElement>(id, process);
                element.Initialize();
                return element;
            }


            public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Make<LauncherToolbarViewModel>(new[] { element, });
                });
                var window = DiContainer.Make<LauncherToolbarWindow>();
                viewModel.AppDesktopToolbarExtend = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(viewModel);
                    return c.Make<AppDesktopToolbarExtend>(new object[] { window, element, });
                });
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, window);
            }

            public WindowItem CreateCustomizeLauncherItemWindow(CustomizeLauncherItemElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<CustomizeLauncherItemViewModel>(element);
                });
                var window = DiContainer.Build<CustomizeLauncherItemWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, window);
            }

            public WindowItem CreateNoteWindow(NoteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<NoteViewModel>(element);
                });
                var window = DiContainer.Build<NoteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Note, window);
            }

            public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<StandardInputOutputViewModel>(element);
                });
                var window = DiContainer.Build<StandardInputOutputWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.StandardInputOutput, window);
            }
            #endregion

        }
    }
}
