using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public interface IDragAndDrop
    {
        #region property

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        Size DragStartSize { get; set; }

        /// <summary>
        ///<see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用する。
        /// </summary>
        /// <remarks>
        ///<para>基本的には偽で <see cref="UIElement.MouseMove"/> を使用する。 なんにせよ<see cref="UIElement.PreviewMouseDown"/> は強制される。</para>
        /// </remarks>
        bool UsePreviewEvent { get; }

        #endregion

        #region function

        void MouseDown(UIElement sender, MouseButtonEventArgs e);
        void MouseMove(UIElement sender, MouseEventArgs e);
        void DragEnter(UIElement sender, DragEventArgs e);
        void DragOver(UIElement sender, DragEventArgs e);
        void DragLeave(UIElement sender, DragEventArgs e);
        void Drop(UIElement sender, DragEventArgs e);

        #endregion
    }

    public class DragParameter
    {
        public DragParameter(UIElement element, DragDropEffects effects, IDataObject data)
        {
            Element = element;
            Effects = effects;
            Data = data;
        }

        #region property

        /// <summary>
        /// 対象要素。
        /// </summary>
        public UIElement Element { get; }
        /// <summary>
        /// <inheritdoc cref="DragDropEffects"/>
        /// </summary>
        public DragDropEffects Effects { get; set; }
        /// <summary>
        /// ドラッグデータ。
        /// </summary>
        public IDataObject Data { get; }

        #endregion
    }

    public abstract class DragAndDropBase: IDragAndDrop
    {
        /// <summary>
        /// <see cref="UsePreviewEvent"/> は false を使用する。
        /// </summary>
        /// <param name="logger"></param>
        protected DragAndDropBase(ILogger logger)
        {
            UsePreviewEvent = false;
            Logger = logger;
        }
        /// <summary>
        /// <see cref="UsePreviewEvent"/> は false を使用する。
        /// </summary>
        /// <param name="loggerFactory"></param>
        protected DragAndDropBase(ILoggerFactory loggerFactory)
        {
            UsePreviewEvent = false;
            Logger = loggerFactory.CreateLogger(GetType());
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="usingPreviewEvent"><see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用するか。</param>
        /// <param name="logger"></param>
        protected DragAndDropBase(bool usingPreviewEvent, ILogger logger)
        {
            UsePreviewEvent = usingPreviewEvent;
            Logger = logger;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="usingPreviewEvent"><see cref="UIElement.PreviewMouseMove"/> 的な Preview のイベントを使用するか。</param>
        /// <param name="loggerFactory"></param>
        protected DragAndDropBase(bool usingPreviewEvent, ILoggerFactory loggerFactory)
        {
            UsePreviewEvent = usingPreviewEvent;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        [PixelKind(Px.Logical)]
        private Point DragStartPosition { get; set; }

        /// <summary>
        /// ドラッグ中か。
        /// </summary>
        public bool IsDragging { get; private set; }

        /// <summary>
        /// ドラッグ開始とみなす距離。
        /// </summary>
        [PixelKind(Px.Device)]
        public Size DragStartSize { get; set; } = new Size(10, 10);

        #endregion

        #region function

        protected abstract bool CanDragStartImpl(UIElement sender, MouseEventArgs e);
        /// <summary>
        /// ドラッグした<see cref="DragParameter"/>の取得。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected abstract IResultSuccess<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e);

        private void MouseDownCore(UIElement sender, MouseEventArgs e)
        {
            DragStartPosition = e.GetPosition(null);
        }

        private void MouseMoveCore(UIElement sender, MouseEventArgs e)
        {
            var nowPosition = e.GetPosition(null);

            var isDragX = Math.Abs(nowPosition.X - DragStartPosition.X) > DragStartSize.Width;
            var isDragY = Math.Abs(nowPosition.Y - DragStartPosition.Y) > DragStartSize.Height;
            if(isDragX || isDragY) {
                var parameterResult = GetDragParameterImpl(sender, e);
                if(parameterResult.Success) {
                    var parameter = parameterResult.SuccessValue;
                    if(parameter == null) {
                        Logger.LogWarning("{Parameter} is null, 後続D&D処理スキップ", nameof(parameter));
                    } else {
                        IsDragging = true;
                        try {
                            DragDrop.DoDragDrop(parameter.Element, parameter.Data, parameter.Effects);
                        } finally {
                            IsDragging = false;
                        }
                    }
                }
            }
        }

        #endregion

        #region IDragAndDrop

        /// <summary>
        /// <see cref="IDragAndDrop.UsePreviewEvent"/>
        /// </summary>
        public bool UsePreviewEvent { get; }

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

    public delegate bool CanDragStartDelegate(UIElement sender, MouseEventArgs e);
    public delegate IResultSuccess<DragParameter> GetDragParameterDelegate(UIElement sender, MouseEventArgs e);
    public delegate void DragAndDropDelegate(UIElement sender, DragEventArgs e);
    public delegate Task DragAndDropAsyncDelegate(UIElement sender, DragEventArgs e, CancellationToken cancellationToken);
    
    public class DelegateDragAndDrop: DragAndDropBase
    {
        public DelegateDragAndDrop(ILogger logger)
            : base(logger)
        { }
        public DelegateDragAndDrop(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        public DelegateDragAndDrop(bool usingPreviewEvent, ILogger logger)
            : base(usingPreviewEvent, logger)
        { }
        public DelegateDragAndDrop(bool usingPreviewEvent, ILoggerFactory loggerFactory)
            : base(usingPreviewEvent, loggerFactory)
        { }

        #region property

        public CanDragStartDelegate? CanDragStart { get; set; }
        public GetDragParameterDelegate? GetDragParameter { get; set; }
        public DragAndDropDelegate? DragEnterAction { get; set; }
        public DragAndDropDelegate? DragLeaveAction { get; set; }
        public DragAndDropDelegate? DragOverAction { get; set; }
        public DragAndDropAsyncDelegate? DropActionAsync { get; set; }

        #endregion

        #region DragAndDropBase

        protected override bool CanDragStartImpl(UIElement sender, MouseEventArgs e)
        {
            if(CanDragStart != null) {
                return CanDragStart(sender, e);
            }

            return false;
        }

        protected override IResultSuccess<DragParameter> GetDragParameterImpl(UIElement sender, MouseEventArgs e)
        {
            if(GetDragParameter != null) {
                return GetDragParameter(sender, e);
            }

            return Result.CreateFailure<DragParameter>();
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
            DropActionAsync?.Invoke(sender, e, CancellationToken.None);
        }

        #endregion
    }
}
