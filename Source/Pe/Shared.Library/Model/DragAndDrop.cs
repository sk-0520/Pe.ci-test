using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IDropable
    {
        /// <summary>
        /// ドラッグ中アイテムが上に存在しているか。
        /// </summary>
        bool IsDragOver { get; set; }
    }

    public interface IDragAndDrop
    {
        void MouseDown(UIElement sender, MouseButtonEventArgs e);
        void MouseMove(UIElement sender, MouseEventArgs e);
        void DragEnter(UIElement sender, DragEventArgs e);
        void DragOver(UIElement sender, DragEventArgs e);
        void DragLeave(UIElement sender, DragEventArgs e);
        void Drop(UIElement sender, DragEventArgs e);
    }


    public class DragParameter
    {
        #region property

        public UIElement Element { get; set; }
        public DataObject Data { get; set; }
        public DragDropEffects Effects { get; set; }

        #endregion
    }


    public abstract class DragAndDropBase : IDragAndDrop
    {
        public DragAndDropBase(ILogger logger)
        {
            Logger = logger;
        }
        public DragAndDropBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        [PixelKind(Px.Logical)]
        Point DragStartPosition { get; set; }

        public bool IsDragging { get; private set; }

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        public Size DragStartSize { get; set; } = new Size(10, 10);

        #endregion

        #region function

        protected abstract bool CanDragStartImpl(UIElement sender, MouseEventArgs e);
        protected abstract IResultSuccessValue<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e);

        void MouseDownCore(UIElement sender, MouseEventArgs e)
        {
            DragStartPosition = e.GetPosition(null);
        }

        void MouseMoveCore(UIElement sender, MouseEventArgs e)
        {
            var nowPosition = e.GetPosition(null);

            var isDragX = Math.Abs(nowPosition.X - DragStartPosition.X) > DragStartSize.Width;
            var isDragY = Math.Abs(nowPosition.Y - DragStartPosition.Y) > DragStartSize.Height;
            if(isDragX || isDragY) {
                var parameterResult = GetDragParameterImpl(sender, e);
                if(parameterResult.Success) {
                    var parameter = parameterResult.SuccessValue;
                    IsDragging = true;
                    DragDrop.DoDragDrop(parameter.Element, parameter.Data, parameter.Effects);
                    IsDragging = false;
                }
            }
        }

        #endregion

        #region IDragAndDrop

        public void MouseDown(UIElement sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) {
                MouseDownCore(sender, e);
            }
        }

        public void MouseMove(UIElement sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            if(IsDragging) {
                return;
            }

            if(!CanDragStartImpl(sender, e)) {
                return;
            }

            MouseMoveCore(sender, e);
        }

        public abstract void DragEnter(UIElement sender, DragEventArgs e);

        public abstract void DragOver(UIElement sender, DragEventArgs e);

        public abstract void DragLeave(UIElement sender, DragEventArgs e);

        public abstract void Drop(UIElement sender, DragEventArgs e);


        #endregion
    }

    public class ActionDragAndDrop : DragAndDropBase
    {
        public ActionDragAndDrop(ILogger logger)
            : base(logger)
        { }
        public ActionDragAndDrop(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region property

        public Func<UIElement, MouseEventArgs, bool> CanDragStart { get; set; }
        public Func<UIElement, MouseEventArgs, IResultSuccessValue<DragParameter>> GetDragParameter { get; set; }
        public Action<UIElement, DragEventArgs> DragEnterAction { get; set; }
        public Action<UIElement, DragEventArgs> DragLeaveAction { get; set; }
        public Action<UIElement, DragEventArgs> DragOverAction { get; set; }
        public Action<UIElement, DragEventArgs> DropAction { get; set; }

        #endregion

        #region DragAndDropBase

        protected override bool CanDragStartImpl(UIElement sender, MouseEventArgs e)
        {
            if(CanDragStart != null) {
                return CanDragStart(sender, e);
            }

            return false;
        }

        protected override IResultSuccessValue<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e)
        {
            if(GetDragParameter != null) {
                return GetDragParameter(sender, e);
            }

            return ResultSuccessValue.Failure<DragParameter>();
        }

        public override void DragEnter(UIElement sender, DragEventArgs e)
        {
            DragEnterAction?.Invoke(sender, e);
        }

        public override void DragLeave(UIElement sender, DragEventArgs e)
        {
            DragLeaveAction?.Invoke(sender, e);
        }

        public override void DragOver(UIElement sender, DragEventArgs e)
        {
            DragOverAction?.Invoke(sender, e);
        }

        public override void Drop(UIElement sender, DragEventArgs e)
        {
            DropAction?.Invoke(sender, e);
        }

        #endregion
    }

}
