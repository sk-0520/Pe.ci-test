using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Bridge.Models;
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

        double _rightPupilX;
        double _rightPupilY;

        Color _leftEyeColor = Colors.White;
        Color _rightEyeColor = Colors.White;

        Color _leftStrokeColor = Colors.Black;
        Color _rightStrokeColor = Colors.Black;

        Color _leftPupilColor = Colors.Black;
        Color _rightPupilColor = Colors.Black;

        bool _leftPressed;
        bool _rightPressed;

        #endregion

        public EyesWidgetViewModel(EyesWidgetWindow eyesWidgetWindow, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            LeftEye = eyesWidgetWindow.leftEye;
            RightEye = eyesWidgetWindow.rightEye;
        }

        #region property

        Ellipse LeftEye { get; }
        Ellipse RightEye { get; }

        public double EyeWidth { get; } = 100;
        public double EyeHeight { get; } = 100;


        public double PupilWidth { get; } = 20;
        public double PupilHeight { get; } = 20;

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

        public Color LeftEyeColor
        {
            get => this._leftEyeColor;
            set => SetProperty(ref this._leftEyeColor, value);
        }
        public Color RightEyeColor
        {
            get => this._rightEyeColor;
            set => SetProperty(ref this._rightEyeColor, value);
        }

        public Color LeftStrokeColor
        {
            get => this._leftStrokeColor;
            set => SetProperty(ref this._leftStrokeColor, value);
        }
        public Color RightStrokeColor
        {
            get => this._rightStrokeColor;
            set => SetProperty(ref this._rightStrokeColor, value);
        }


        public Color LeftPupilColor
        {
            get => this._leftPupilColor;
            set => SetProperty(ref this._leftPupilColor, value);
        }
        public Color RightPupilColor
        {
            get => this._rightPupilColor;
            set => SetProperty(ref this._rightPupilColor, value);
        }

        public bool LeftPressed
        {
            get => this._leftPressed;
            set => SetProperty(ref this._leftPressed, value);
        }
        public bool RightPressed
        {
            get => this._rightPressed;
            set => SetProperty(ref this._rightPressed, value);
        }


        public double RightPupilX
        {
            get => this._rightPupilX;
            set => SetProperty(ref this._rightPupilX, value);
        }
        public double RightPupilY
        {
            get => this._rightPupilY;
            set => SetProperty(ref this._rightPupilY, value);
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
            eyesBackground.MouseDown += EyesBackground_MouseDown;
            eyesBackground.MouseUp += EyesBackground_MouseUp;
            eyesBackground.KeyDown += EyesBackground_KeyDown;
            eyesBackground.KeyUp += EyesBackground_KeyUp;
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

            DispatcherWrapper.Begin(() => {
                var deviceCursorLocation = new Point(MouseX, MouseY);

                var cx = EyeWidth / 2;
                var cy = EyeWidth / 2;
                var checkr = (EyeWidth / 2) * (EyeWidth / 2);

                Point ToPupilLocation(Point deviceCursorLocation, Visual element)
                {
                    var logicalRelativeLocation = element.PointFromScreen(deviceCursorLocation);

                    if((logicalRelativeLocation.X - cx) * (logicalRelativeLocation.X - cx) + (logicalRelativeLocation.Y - cy) * (logicalRelativeLocation.Y - cy) < checkr) {
                        return new Point(
                            logicalRelativeLocation.X - PupilWidth / 2,
                            logicalRelativeLocation.Y - PupilHeight / 2
                        );
                    } else {
                        double radian = Math.Atan2(logicalRelativeLocation.Y - cy, logicalRelativeLocation.X - cx);
                        return new Point(
                            cx + (cx - PupilWidth) * Math.Cos(radian) - PupilWidth / 2,
                            cy + (cy - PupilHeight) * Math.Sin(radian) - PupilHeight / 2
                        );
                    }
                }

                var left = ToPupilLocation(deviceCursorLocation, LeftEye);
                LeftPupilX = left.X;
                LeftPupilY = left.Y;

                var right = ToPupilLocation(deviceCursorLocation, RightEye);
                RightPupilX = right.X;
                RightPupilY = right.Y;


            }, System.Windows.Threading.DispatcherPriority.Render);
        }

        private void EyesBackground_MouseDown(object? sender, BackgroundMouseButtonEventArgs e)
        {
            if(e.Button == System.Windows.Input.MouseButton.Left) {
                LeftPressed = e.State == System.Windows.Input.MouseButtonState.Pressed;
            }
            if(e.Button == System.Windows.Input.MouseButton.Right) {
                RightPressed = e.State == System.Windows.Input.MouseButtonState.Pressed;
            }
        }

        private void EyesBackground_MouseUp(object? sender, BackgroundMouseButtonEventArgs e)
        {
            if(e.Button == System.Windows.Input.MouseButton.Left) {
                LeftPressed = e.State == System.Windows.Input.MouseButtonState.Pressed;
            }
            if(e.Button == System.Windows.Input.MouseButton.Right) {
                RightPressed = e.State == System.Windows.Input.MouseButtonState.Pressed;
            }
        }



        private void EyesBackground_KeyDown(object? sender, BackgroundKeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.LeftShift) {
                LeftEyeColor = Colors.Red;
            }
            if(e.Key == System.Windows.Input.Key.RightShift) {
                RightEyeColor = Colors.Red;
            }

            if(e.Key == System.Windows.Input.Key.LeftAlt) {
                LeftStrokeColor = Colors.Blue;
            }
            if(e.Key == System.Windows.Input.Key.RightAlt) {
                RightStrokeColor = Colors.Blue;
            }

            if(e.Key == System.Windows.Input.Key.LeftCtrl) {
                LeftPupilColor = Colors.Lime;
            }
            if(e.Key == System.Windows.Input.Key.RightCtrl) {
                RightPupilColor = Colors.Lime;
            }

        }

        private void EyesBackground_KeyUp(object? sender, BackgroundKeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.LeftShift) {
                LeftEyeColor = Colors.White;
            }
            if(e.Key == System.Windows.Input.Key.RightShift) {
                RightEyeColor = Colors.White;
            }

            if(e.Key == System.Windows.Input.Key.LeftAlt) {
                LeftStrokeColor = Colors.Black;
            }
            if(e.Key == System.Windows.Input.Key.RightAlt) {
                RightStrokeColor = Colors.Black;
            }

            if(e.Key == System.Windows.Input.Key.LeftCtrl) {
                LeftPupilColor = Colors.Black;
            }
            if(e.Key == System.Windows.Input.Key.RightCtrl) {
                RightPupilColor = Colors.Black;
            }

        }


    }
}
