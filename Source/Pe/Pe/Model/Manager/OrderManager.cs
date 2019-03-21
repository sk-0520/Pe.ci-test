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
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Logic;
using ContentTypeTextNet.Pe.Main.View.Extend;
using ContentTypeTextNet.Pe.Main.View.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;

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

    public interface IOrderManager
    {
        #region function

        LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId);
        LauncherToolbarElement CreateLauncherToolbarElement(Screen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups);
        LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId);

        WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element);

        #endregion
    }

    partial class ApplicationManager
    {
        class OrderManagerIml : ManagerBase, IOrderManager
        {
            public OrderManagerIml(IDiContainer diContainer, ILoggerFactory loggerFactory)
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

            public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Make<LauncherToolbarViewModel>(new[] { element, });
                });
                var window = DiContainer.Make<LauncherToolbarWindow>();
                viewModel.AppDesktopToolbarExtend = DiContainer.UsingTemporaryContainer(c => {
                    c.Register<IAppDesktopToolbarExtendData, LauncherToolbarViewModel>(viewModel);
                    c.Register<ILoggerFactory, ILoggerFactory>(viewModel);
                    return c.Make<AppDesktopToolbarExtend>(new object[] { window, viewModel, });
                });
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, window);
            }

            #endregion

        }
    }
}
