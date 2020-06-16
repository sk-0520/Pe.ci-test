using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Eyes.Addon;
using ContentTypeTextNet.Pe.Plugins.Eyes.Views;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.ViewModels
{
    public class EyesWidgetViewModel: ViewModelSkeleton
    {
        #region variable

        double _mouseX;
        double _mouseY;

        double _leftPupilX;
        double _leftPupilY;

        #endregion

        public EyesWidgetViewModel(EyesWidgetWindow eyesWidgetWindow, ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        {
            LeftEye = eyesWidgetWindow.leftEye;
        }

        #region property

        Ellipse LeftEye { get; }

        public double EyeWidth { get; } = 100;
        public double EyeHeight { get; } = 100;


        public double PupilWidth { get; } = 10;
        public double PupilHeight { get; } = 10;

        public double LeftPupilX
        {
            get => this._leftPupilX;
            set => SetProperty(ref this._leftPupilX, value);
        }
        public double LeftPupilY
        {
            get => this._leftPupilY;
            set => SetProperty(ref this._leftPupilY, value);
        }

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

        Point GetDipScale(Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);
            if(source != null && source.CompositionTarget != null) {
                return new Point(
                    source.CompositionTarget.TransformToDevice.M11,
                    source.CompositionTarget.TransformToDevice.M22
                );
            }

            return new Point(1, 1);
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

            var deviceCursorLocation = new Point(MouseX, MouseY);
            var scale = GetDipScale(LeftEye);
            var logicalCursorLocation = new Point(deviceCursorLocation.X * scale.Y, deviceCursorLocation.Y * scale.Y);

            var relativeLocation = LeftEye.PointFromScreen(logicalCursorLocation);

            Logger.LogInformation("left {0}", relativeLocation);
        }
    }
}
