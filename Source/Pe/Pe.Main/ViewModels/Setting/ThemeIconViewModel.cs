using System;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class ThemeIconViewModel<TValue>: ViewModelBase
    {
        public ThemeIconViewModel(TValue value, Func<Color, object> iconGetter, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = value;
            IconGetter = iconGetter;
        }

        #region property
        public TValue Value { get; }
        Func<Color, object> IconGetter { get; }

        public Color IconColor { get; private set; }
        public object Icon => IconGetter(IconColor);

        #endregion

        #region function

        public void ChangeColor(Color color)
        {
            IconColor = color;
            RaisePropertyChanged(nameof(Icon));
        }

        #endregion
    }
}
