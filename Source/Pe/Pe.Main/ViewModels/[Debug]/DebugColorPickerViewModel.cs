using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element._Debug_;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels._Debug_
{
    public class DebugColorPickerViewModel : DebugViewModelBase<DebugColorPickerElement>
    {
        #region variable

        Color _color = Colors.Red;

        #endregion

        public DebugColorPickerViewModel(DebugColorPickerElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public Color Color
        {
            get => this._color;
            set => SetProperty(ref this._color, value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }
}
