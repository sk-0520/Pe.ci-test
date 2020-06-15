using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Eyes.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.ViewModels
{
    public class EyesWidgetViewModel: ViewModelSkeleton
    {
        #region variable

        double _mouseX;
        double _mouseY;
        #endregion
        public EyesWidgetViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        { }

        #region property

        public double MouseX
        {
            get => this._mouseX;
            private set => SetProperty(ref this._mouseX, value);
        }
        public double MouseY
        {
            get => this._mouseY;
            private set => SetProperty(ref this._mouseY, value);
        }

        EyesBackground? EyesBackground { get; set; }

        #endregion

        #region command

        #endregion

        #region function

        internal void Attach(EyesBackground eyesBackground)
        {
            if(EyesBackground != null) {
                Detach();
            }

            EyesBackground = eyesBackground;
            eyesBackground.MouseMoved += EyesBackground_MouseMoved;
        }

        internal void Detach()
        {
            if(EyesBackground == null) {
                return;
            }

            EyesBackground.MouseMoved -= EyesBackground_MouseMoved;
            EyesBackground = null;
        }

        #endregion

        #region ViewModelSkeleton

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Detach();
            }
            base.Dispose(disposing);
        }

        #endregion

        private void EyesBackground_MouseMoved(object? sender, BackgroundMouseMoveEventArgs e)
        {
            MouseX = e.Location.X;
            MouseY = e.Location.Y;
        }
    }
}
