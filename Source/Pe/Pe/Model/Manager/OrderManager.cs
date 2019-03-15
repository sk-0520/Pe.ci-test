using System;
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
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.View.Extend;
using ContentTypeTextNet.Pe.Main.View.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModel.LauncherToolbar;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public enum ElementKind
    {
        LauncherGroup,
        LauncherToolbar,
    }

    public class OrderElementParameter
    {
        public OrderElementParameter(ElementKind elementKind)
        {
            ElementKind = elementKind;
        }

        #region property

        public ElementKind ElementKind { get; }

        #endregion
    }


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

        ElementBase CreateElement(OrderElementParameter parameter);
        WindowItem CreateWindow(OrderWindowParameter parameter);

        #endregion
    }

    partial class ApplicationManager
    {
        class OrderManagerIml : ManagerBase, IOrderManager
        {
            public OrderManagerIml(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region function

            LauncherGroupElement CreateLauncherGroupElementCore(Guid launcherGroupId)
            {
                var element = DiContainer.Make<LauncherGroupElement>(new object[] { launcherGroupId });
                element.Initialize();
                return element;
            }

            LauncherGroupElement CreateLauncherGroupElement(OrderElementParameter parameter)
            {
                var args = (OrderLauncherGroupElementParameter)parameter;
                return CreateLauncherGroupElementCore(args.LauncherGroupId);
            }


            LauncherToolbarElement CreateLauncherToolbarElementCore(Screen dockScreen, ObservableCollection<LauncherGroupElement> launcherGroups)
            {
                var element = DiContainer.Make<LauncherToolbarElement>(new object[] { dockScreen, launcherGroups });
                element.Initialize();
                return element;
            }

            LauncherToolbarElement CreateLauncherToolbarElement(OrderElementParameter parameter)
            {
                var args = (OrderLauncherToolbarElementParameter)parameter;
                return CreateLauncherToolbarElementCore(args.Screen, args.LauncherGroups);
            }

            LauncherToolbarWindow CreateLauncherToolbarWindow(LauncherToolbarElement element)
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

                return window;
            }

            Window BuildWindow(OrderWindowParameter parameter)
            {
                switch(parameter.WindowKind) {
                    case WindowKind.LauncherToolbar:
                        return CreateLauncherToolbarWindow((LauncherToolbarElement)parameter.Element);

                    default:
                        throw new NotImplementedException();
                }
            }

            #endregion

            #region IOrderManager

            public ElementBase CreateElement(OrderElementParameter parameter)
            {
                switch(parameter.ElementKind) {
                    case ElementKind.LauncherGroup:
                        return CreateLauncherGroupElement(parameter);

                    case ElementKind.LauncherToolbar:
                        return CreateLauncherToolbarElement(parameter);

                    default:
                        throw new NotImplementedException();

                }
            }

            public WindowItem CreateWindow(OrderWindowParameter parameter)
            {
                var window = BuildWindow(parameter);
                var windowItem = new WindowItem(parameter.WindowKind, window);

                return windowItem;
            }

            #endregion

        }
    }
}
