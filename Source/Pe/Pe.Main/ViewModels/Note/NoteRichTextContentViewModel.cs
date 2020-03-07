using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteRichTextContentViewModel : NoteContentViewModelBase, IFlushable
    {
        #region variable

        bool _isOpenToolbar;

        FontFamily? _selectionFontFamily;
        decimal _selectionFontHeight;
        Color _foregroundColor;
        Color _backgroundColor;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWrapper, loggerFactory)
        {
            TextChangeLazyAction = new LazyAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
        }

        #region property

        //Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }
        RichTextBox? ContentElement { get; set; }
        FlowDocument Document => ContentElement?.Document ?? throw new NullReferenceException(nameof(ContentElement));
        Popup? PopupElement { get; set; }

        LazyAction TextChangeLazyAction { get; }

        public bool IsOpenToolbar
        {
            get => this._isOpenToolbar;
            set => SetProperty(ref this._isOpenToolbar, value);
        }

        bool NowSelectionProcess { get; set; }

        public FontFamily? SelectionFontFamily
        {
            get => this._selectionFontFamily;
            set
            {
                SetProperty(ref this._selectionFontFamily, value);
                if(!NowSelectionProcess) {
                    ApplySelectionFontFamily();
                }
            }
        }

        public decimal SelectionFontHeight
        {
            get => this._selectionFontHeight;
            set
            {
                SetProperty(ref this._selectionFontHeight, value);
                if(!NowSelectionProcess) {
                    ApplySelectionFontHeight();
                }
            }
        }

        public Color SelectionForegroundColor
        {
            get => this._foregroundColor;
            set
            {
                SetProperty(ref this._foregroundColor, value);
                if(!NowSelectionProcess) {
                    ApplySelectionForegroundColor();
                }
            }
        }

        public Color SelectionBackgroundColor
        {
            get => this._backgroundColor;
            set
            {
                SetProperty(ref this._backgroundColor, value);
                if(!NowSelectionProcess) {
                    ApplySelectionBackgroundColor();
                }
            }
        }


        #endregion

        #region command

        public ICommand ApplySeletectionForegroundColorCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplySelectionForegroundColor();
            }
        ));
        public ICommand ApplySeletectionBackgroundColorCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplySelectionBackgroundColor();
            }
        ));

        public ICommand ToggleSeletectionUnderlineCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Underline);
            }
        ));

        public ICommand ToggleSeletectionStrikeThroughCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Strikethrough);
            }
        ));

        #endregion

        #region function

        private void ApplySelectionCore(Action<RichTextBox, TextSelection> action)
        {
            if(ContentElement == null) {
                return;
            }
            if(ContentElement.Selection.IsEmpty) {
                return;
            }
            action(ContentElement, ContentElement.Selection);
        }

        private void ToggleSelectionDecoration(TextDecorationLocation location)
        {
            ApplySelectionCore((r, s) => {
                var textDecorationsProperty = s.GetPropertyValue(Inline.TextDecorationsProperty);
                if(textDecorationsProperty == DependencyProperty.UnsetValue) {
                    var decs = location switch {
                        TextDecorationLocation.Underline => TextDecorations.Underline,
                        TextDecorationLocation.Strikethrough => TextDecorations.Strikethrough,
                        TextDecorationLocation.Baseline => TextDecorations.Baseline,
                        TextDecorationLocation.OverLine => TextDecorations.OverLine,
                        _ => throw new NotImplementedException(),
                    };
                    s.ApplyPropertyValue(Inline.TextDecorationsProperty, decs);
                } else {
                    var decorations = (TextDecorationCollection)textDecorationsProperty;
                    var current = decorations.FirstOrDefault(i => i.Location == location);
                    if(current != null) {
                        if(decorations.IsFrozen) {
                            var newDecorations = new TextDecorationCollection(decorations.Where(i => i != current));
                            s.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
                        } else {
                            decorations.Remove(current);
                        }
                    } else {
                        var dec = FreezableUtility.GetSafeFreeze(new TextDecoration() {
                            Location = location,
                        });
                        if(decorations.IsFrozen) {
                            var newDecorations = new TextDecorationCollection(decorations);
                            newDecorations.Add(dec);
                            s.ApplyPropertyValue(Inline.TextDecorationsProperty, newDecorations);
                        } else {
                            decorations.Add(dec);
                        }
                    }
                }
            });
        }

        private void ApplySelectionPropertyValue(DependencyProperty dependencyProperty, object value)
        {
            ApplySelectionCore((r, s) => s.ApplyPropertyValue(dependencyProperty, value));
        }

        private void ApplySelectionFontFamily()
        {
            if(SelectionFontFamily != null) {
                ApplySelectionPropertyValue(Inline.FontFamilyProperty, SelectionFontFamily);
            }
        }

        private void ApplySelectionFontHeight()
        {
            if(0 < SelectionFontHeight) {
                ApplySelectionPropertyValue(Inline.FontSizeProperty, Convert.ToDouble(SelectionFontHeight));
            }
        }

        private void ApplySelectionForegroundColor()
        {
            var brush = FreezableUtility.GetSafeFreeze(new SolidColorBrush(SelectionForegroundColor));
            ApplySelectionPropertyValue(Inline.ForegroundProperty, brush);
        }

        private void ApplySelectionBackgroundColor()
        {
            var brush = FreezableUtility.GetSafeFreeze(new SolidColorBrush(SelectionBackgroundColor));
            ApplySelectionPropertyValue(Inline.BackgroundProperty, brush);
        }

        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(FrameworkElement baseElement)
        {
            //Control = (Xceed.Wpf.Toolkit.RichTextBox)control;
            ContentElement = (RichTextBox)baseElement.FindName("content");
            PopupElement = (Popup)baseElement.FindName("popup");

            ContentElement.TextChanged += Control_TextChanged;
            ContentElement.SelectionChanged += RichTextBox_SelectionChanged;

            return Task.Run(() => {
                var content = Model.LoadRichTextContent();
                //RtfContent = content;

                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                var stream = noteContentConverter.ToRtfStream(content);
                DispatcherWrapper.Begin(arg => {
                    if(arg.@this.IsDisposed) {
                        return;
                    }

                    var range = new TextRange(arg.@this.Document.ContentStart, arg.@this.Document.ContentEnd);
                    range.Load(arg.stream, DataFormats.Rtf);
                    stream.Dispose();
                }, (@this: this, stream), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            });
        }

        protected override void UnloadContent()
        {
            Flush();

            if(ContentElement == null) {
                return;
            }

            ContentElement.TextChanged -= Control_TextChanged;
            ContentElement.SelectionChanged -= RichTextBox_SelectionChanged;

            PopupElement = null;
            ContentElement = null;
            IsOpenToolbar = false;
        }

        protected override IDataObject GetClipbordContentData()
        {
            var data = new DataObject();
            if(CanVisible) {
                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                data.SetText(noteContentConverter.ToRtfString(Document), TextDataFormat.Rtf);
            } else {
                var value = Model.LoadRichTextContent();
                data.SetText(value, TextDataFormat.Rtf);
            }

            return data;
        }

        void ChangedText()
        {
            if(CanVisible && EnabledUpdate) {
                DispatcherWrapper.Begin(vm => {
                    if(vm.IsDisposed) {
                        return;
                    }
                    var noteContentConverter = new NoteContentConverter(vm.LoggerFactory);
                    var content = noteContentConverter.ToRtfString(vm.Document);
                    vm.Model?.ChangeRichTextContent(content);
                }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlushable
        public void Flush()
        {
            TextChangeLazyAction.SafeFlush();
        }

        #endregion


        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeLazyAction.DelayAction(ChangedText);
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // ツールバー的なの何かできればいいなぁと
            Debug.Assert(ContentElement != null);
            Debug.Assert(PopupElement != null);

            NowSelectionProcess = true;
            using var _cleanup_ = new ActionDisposer(() => NowSelectionProcess = false);

            if(ContentElement.Selection.IsEmpty) {
                // 未選択。さよなら
                IsOpenToolbar = false;
                return;
            }

            // 選択状態から表示可能項目の抜出
            var fontFamliyProperty = ContentElement.Selection.GetPropertyValue(RichTextBox.FontFamilyProperty);
            var fontSizeProperty = ContentElement.Selection.GetPropertyValue(RichTextBox.FontSizeProperty);
            Logger.LogDebug("fontFamliyProperty: {0}", fontFamliyProperty);
            Logger.LogDebug("fontSizeProperty: {0}", fontSizeProperty);

            if(fontFamliyProperty == DependencyProperty.UnsetValue) {
                SelectionFontFamily = null;
            } else {
                SelectionFontFamily = (FontFamily)fontFamliyProperty;
            }

            if(fontSizeProperty != DependencyProperty.UnsetValue) {
                // 複数サイズの場合は直近のを表示しとくしかないなぁ(自家製 NumericUpDown が値無しを表示できるほど柔軟じゃない)
                SelectionFontHeight = Convert.ToDecimal(fontSizeProperty);
            }

            // 表示位置補正
            var startRect = ContentElement.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
            var endRect = ContentElement.Selection.End.GetCharacterRect(LogicalDirection.Forward);
            var rect = new Rect(
                ContentElement.PointToScreen(new Point(startRect.X, startRect.Y + startRect.Height)),
                new Size(endRect.Width, endRect.Height)
            );
            PopupElement.PlacementRectangle = rect;

            IsOpenToolbar = true;
        }

    }
}
