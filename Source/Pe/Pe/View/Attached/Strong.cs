using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Effects;

namespace ContentTypeTextNet.Pe.Main.View.Attached
{
    public class Strong
    {
        #region

        public static readonly DependencyProperty IsStrongProperty = DependencyProperty.RegisterAttached(
            "IsStrong",
            typeof(bool),
            typeof(Strong),
            new FrameworkPropertyMetadata(false, OnIsStrongChanged)
        );

        static void OnIsStrongChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            SetIsStrong(dependencyObject, (bool)e.NewValue);
        }

        public static bool GetIsStrong(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsStrongProperty);
        }
        public static void SetIsStrong(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsStrongProperty, value);
            if(dependencyObject is MenuItem menuItem) {
                ApplyMenuItem(menuItem, (bool)value);
            }
        }

        static void ApplyMenuItem(MenuItem menuItem, bool value)
        {
            var iconElement = ((UIElement)menuItem.Icon);
            if(iconElement != null) {
                if(value) {
                    var effect = (Effect)Application.Current.Resources["Effect-Strong"];
                    iconElement.Effect = effect;
                } else {
                    iconElement.Effect = null;
                }
            }

            if(value) {
                menuItem.FontWeight = FontWeights.Bold;
            } else {
                menuItem.FontWeight = FontWeights.Normal;
            }
        }

        #endregion

    }
}
