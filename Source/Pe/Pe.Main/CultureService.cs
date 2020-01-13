using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using ContentTypeTextNet.Pe.Main.Models;

namespace ContentTypeTextNet.Pe.Main
{
    public sealed class CultureService : INotifyPropertyChanged
    {
        #region event

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public CultureService(EnumResourceManager enumResourceManager)
        {
            EnumResourceManager = enumResourceManager;
        }

        #region property

        EnumResourceManager EnumResourceManager { get; }

        static CultureInfo StartupCulture { get; } = CultureInfo.CurrentCulture;

        public static CultureService Current { get; private set; } = null!;

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

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //FrameworkElement.LanguageProperty.OverrideMetadata(
            //    typeof(FrameworkElement),
            //    new FrameworkPropertyMetadata(
            //        XmlLanguage.GetLanguage(culture.IetfLanguageTag)
            //    )
            //);

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

        public XmlLanguage GetXmlLanguage() => XmlLanguage.GetLanguage(Culture.IetfLanguageTag);

        public string GetString(object enumValue, ResourceNameKind resourceNameKind)
        {
            return EnumResourceManager.GetString(enumValue, resourceNameKind);
        }

        internal static void Initialize(CultureService cultureService)
        {
            if(Current != null) {
                throw new InvalidOperationException();
            }

            Current = cultureService;
            Current.ChangeAutoCulture();
        }

        #endregion
    }
}
