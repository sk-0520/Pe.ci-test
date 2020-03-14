using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.CrashReport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var options = new Core.Models.CommandLineSimpleConverter<Models.Options>(new Core.Models.CommandLine()).GetMappingData();
            if(options == null) {
                Shutdown(1);
                return;
            }

            var initializer = new Models.CrashReportInitializer(options);

            var model = initializer.CreateWorker();
            var viewModel = initializer.CreateViewModel(model);

            var view = new Views.MainWindow() {
                DataContext = viewModel,
            };
            view.Show();
        }

        #endregion
    }
}
