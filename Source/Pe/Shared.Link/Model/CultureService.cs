using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Link.Model
{
    public sealed class CultureService : INotifyPropertyChanged
    {
        #region event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region property

        static CultureInfo StartupCulture { get; } = CultureInfo.CurrentCulture;

        public static CultureService Current { get; } = new CultureService();

        public Properties.Resources Resources { get; } = new Properties.Resources();

        public CultureInfo Culture => Properties.Resources.Culture;

        #endregion

        #region function

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void ChangeCultureCore(CultureInfo culture)
        {
            Properties.Resources.Culture = culture;

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            OnPropertyChanged(nameof(Resources));
        }

        public void ChangeCulture(string name)
        {
            var culture = CultureInfo.GetCultureInfo(name);
            ChangeCultureCore(culture);
        }

        public void ChangeAutoCulture()
        {
            ChangeCultureCore(StartupCulture);
        }

        #endregion
    }
}
