using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using ContentTypeTextNet.Pe.Main.Models;

namespace ContentTypeTextNet.Pe.Main
{
    public interface ICultureService
    {
        #region property

        CultureInfo Culture { get; }

        #endregion

        #region function

        /// <summary>
        /// 現在カルチャから XML の言語を取得。
        /// </summary>
        /// <returns></returns>
        XmlLanguage GetXmlLanguage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="resourceNameKind"></param>
        /// <returns></returns>
        string GetString(object enumValue, ResourceNameKind resourceNameKind);
        string GetString(object enumValue, ResourceNameKind resourceNameKind, bool undefinedIsRaw);

        #endregion
    }

    public sealed class CultureService: ICultureService, INotifyPropertyChanged
    {
        #region event

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public CultureService(EnumResourceManager enumResourceManager)
        {
            EnumResourceManager = enumResourceManager;
        }

        #region property

        private EnumResourceManager EnumResourceManager { get; }

        private static CultureInfo StartupCulture { get; } = CultureInfo.CurrentCulture;

        /// <summary>
        /// TODO: あまり使わない方針で行きたい
        /// </summary>
        public static CultureService Instance { get; private set; } = null!;

        public Properties.Resources Resources { get; } = new Properties.Resources();

        #endregion

        #region function

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeCultureCore(CultureInfo culture)
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

        internal static void Initialize(CultureService cultureService)
        {
            if(Instance != null) {
                throw new InvalidOperationException();
            }

            Instance = cultureService;
            Instance.ChangeAutoCulture();
        }

        #endregion

        #region ICultureService

        public CultureInfo Culture => Properties.Resources.Culture;


        public XmlLanguage GetXmlLanguage() => XmlLanguage.GetLanguage(Culture.IetfLanguageTag);

        public string GetString(object enumValue, ResourceNameKind resourceNameKind)
        {
            return EnumResourceManager.GetString(enumValue, resourceNameKind, false);
        }
        public string GetString(object enumValue, ResourceNameKind resourceNameKind, bool undefinedIsRaw)
        {
            return EnumResourceManager.GetString(enumValue, resourceNameKind, undefinedIsRaw);
        }

        #endregion
    }
}
