using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteRichTextContentViewModel: NoteContentViewModelBase, IFlushable
    {
        #region variable

        private FontFamily? _selectionFontFamily;
        private decimal _selectionFontHeight;
        private Color _foregroundColor;
        private Color _backgroundColor;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory)
        {
            TextChangeLazyAction = new LazyAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
            SelectionForegroundColor = Colors.Black;
            SelectionBackgroundColor = Colors.Yellow;
        }

        #region property

        //Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }
        private RichTextBox? RichText { get; set; }
        private FlowDocument Document => RichText?.Document ?? throw new NullReferenceException(nameof(RichText));

        private LazyAction TextChangeLazyAction { get; }

        public double FontMinimumSize => NoteConfiguration.FontSize.Minimum;
        public double FontMaximumSize => NoteConfiguration.FontSize.Maximum;


        private bool NowSelectionProcess { get; set; }

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

        public ICommand ApplySelectionForegroundColorCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplySelectionForegroundColor();
            }
        ));
        public ICommand ApplySelectionBackgroundColorCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplySelectionBackgroundColor();
            }
        ));

        public ICommand ToggleSelectionUnderlineCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Underline);
            }
        ));

        public ICommand ToggleSelectionStrikeThroughCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Strikethrough);
            }
        ));

        public ICommand ToggleSelectionIncreaseFontSizeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.IncreaseFontSize.Execute(null, RichText)
        ));
        public ICommand ToggleSelectionDecreaseFontSizeCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.DecreaseFontSize.Execute(null, RichText)
        ));

        public ICommand ToggleSelectionSubscriptCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleSubscript.Execute(null, RichText)
        ));
        public ICommand ToggleSelectionSuperscriptCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleSuperscript.Execute(null, RichText)
        ));
        public ICommand ToggleSelectionBoldCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleBold.Execute(null, RichText)
        ));
        public ICommand ToggleSelectionItalicCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleItalic.Execute(null, RichText)
        ));
        public ICommand ToggleNumberingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleNumbering.Execute(null, RichText)
        ));
        public ICommand ToggleBulletsCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.ToggleBullets.Execute(null, RichText)
        ));
        public ICommand AlignLeftCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.AlignLeft.Execute(null, RichText)
        ));
        public ICommand AlignCenterCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.AlignCenter.Execute(null, RichText)
        ));
        public ICommand AlignRightCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.AlignRight.Execute(null, RichText)
        ));
        public ICommand AlignJustifyCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => EditingCommands.AlignJustify.Execute(null, RichText)
        ));

        #endregion

        #region function

        private void ApplySelectionCore(Action<RichTextBox, TextSelection> action)
        {
            if(RichText == null) {
                return;
            }
            if(RichText.Selection.IsEmpty) {
                return;
            }
            action(RichText, RichText.Selection);
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

        protected override Task<bool> LoadContentAsync(FrameworkElement baseElement)
        {
            return DispatcherWrapper.InvokeAsync(() => {
                RichText = (RichTextBox)baseElement;
                if(RichText == null) {
                    return false;
                }

                RichText.TextChanged -= Control_TextChanged;
                RichText.SelectionChanged -= RichTextBox_SelectionChanged;

                RichText.TextChanged += Control_TextChanged;
                RichText.SelectionChanged += RichTextBox_SelectionChanged;

                return true;
            }).ContinueWith(t => {
                bool success = false;
                string content;
                if(t.IsCompletedSuccessfully) {
                    try {
                        content = Model.LoadRichTextContent();
                        success = t.Result;
                    } catch(Exception ex) {
                        content = ex.Message;
                    }
                } else {
                    content = t.Exception?.Message ?? "";
                }

                DispatcherWrapper.BeginAsync(() => {
                    var noteContentConverter = new NoteContentConverter(LoggerFactory);
                    using var stream = noteContentConverter.ToRtfStream(content);

                    if(IsDisposed) {
                        return;
                    }
                    if(RichText == null) {
                        return;
                    }

                    var range = new TextRange(Document.ContentStart, Document.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                return success;
            });
        }

        protected override void UnloadContent()
        {
            Flush();

            if(RichText == null) {
                return;
            }

            RichText.TextChanged -= Control_TextChanged;
            RichText.SelectionChanged -= RichTextBox_SelectionChanged;

            RichText = null;
        }

        protected override IDataObject GetClipboardContentData()
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

        private void ChangedText()
        {
            if(CanVisible && EnabledUpdate) {
                DispatcherWrapper.BeginAsync(vm => {
                    if(vm.IsDisposed) {
                        return;
                    }
                    if(RichText == null) {
                        Logger.LogDebug("RichTextBox が破棄されている");
                        return;
                    }

                    var noteContentConverter = new NoteContentConverter(vm.LoggerFactory);
                    var content = noteContentConverter.ToRtfString(vm.Document);
                    vm.Model?.ChangeRichTextContent(content);
                }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        public override void SearchContent(string value, bool toNext)
        {
            Logger.LogDebug("text: {value}, {toNext}", value, toNext);
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
            Debug.Assert(RichText != null);

            NowSelectionProcess = true;
            using var _cleanup_ = new ActionDisposer(d => NowSelectionProcess = false);

            if(RichText.Selection.IsEmpty) {
                // 未選択。さよなら
                return;
            }

            var inlineContainer = RichText.Selection.Start.GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
            if(inlineContainer != null && inlineContainer.Child is Image image) {
                if(inlineContainer == RichText.Selection.End.GetAdjacentElement(LogicalDirection.Backward)) {
                    // TODO: 画像選択からの処理は未実装
                    return;
                }
            }

            // 選択状態から表示可能項目の抜出
            var fontFamilyProperty = RichText.Selection.GetPropertyValue(RichTextBox.FontFamilyProperty);
            var fontSizeProperty = RichText.Selection.GetPropertyValue(RichTextBox.FontSizeProperty);
            Logger.LogDebug("fontFamilyProperty: {0}", fontFamilyProperty);
            Logger.LogDebug("fontSizeProperty: {0}", fontSizeProperty);

            if(fontFamilyProperty == DependencyProperty.UnsetValue) {
                SelectionFontFamily = null;
            } else {
                SelectionFontFamily = (FontFamily)fontFamilyProperty;
            }

            if(fontSizeProperty != DependencyProperty.UnsetValue) {
                // 複数サイズの場合は直近のを表示しとくしかないなぁ(自家製 NumericUpDown が値無しを表示できるほど柔軟じゃない)
                SelectionFontHeight = Convert.ToDecimal(fontSizeProperty, CultureInfo.InvariantCulture);
            }

            // 表示位置補正
            var startRect = RichText.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
            var endRect = RichText.Selection.End.GetCharacterRect(LogicalDirection.Backward);
            var mixX = Math.Min(endRect.X, startRect.X);
            var maxX = Math.Max(endRect.X, startRect.X);
            var rect = new Rect(
                RichText.PointToScreen(new Point(maxX - mixX, endRect.Y + endRect.Height)),
                new Size(endRect.Width, endRect.Height)
            );
        }
    }
}
