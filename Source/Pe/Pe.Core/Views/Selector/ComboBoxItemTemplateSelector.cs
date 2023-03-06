using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Selector
{
    public class ComboBoxItemTemplateSelector: DataTemplateSelector
    {
        #region property

        public DataTemplate? DropDownTemplate { get; set; }
        public DataTemplate? SelectedTemplate { get; set; }

        #endregion

        #region DataTemplateSelector

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var comboBoxItem = UIUtility.GetVisualClosest<ComboBoxItem>(container);
            if(comboBoxItem != null) {
                return DropDownTemplate!;
            }

            return SelectedTemplate!;
        }

        #endregion
    }
}
