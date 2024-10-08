using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
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
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteRichTextContentViewModel: NoteContentViewModelBase<RichTextBox>, IFlushable
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
            TextChangeDelayAction = new DelayAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
            SelectionForegroundColor = Colors.Black;
            SelectionBackgroundColor = Colors.Yellow;
        }

        #region property

        private FlowDocument Document => ControlElement?.Document ?? throw new NullReferenceException(nameof(ControlElement));

        private DelayAction TextChangeDelayAction { get; }

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

        private ICommand? _ApplySelectionForegroundColorCommand;
        public ICommand ApplySelectionForegroundColorCommand => this._ApplySelectionForegroundColorCommand ??= new DelegateCommand(
            () => {
                ApplySelectionForegroundColor();
            }
        );

        private ICommand? _ApplySelectionBackgroundColorCommand;
        public ICommand ApplySelectionBackgroundColorCommand => this._ApplySelectionBackgroundColorCommand ??= new DelegateCommand(
            () => {
                ApplySelectionBackgroundColor();
            }
        );

        private ICommand? _ToggleSelectionUnderlineCommand;
        public ICommand ToggleSelectionUnderlineCommand => this._ToggleSelectionUnderlineCommand ??= new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Underline);
            }
        );

        private ICommand? _ToggleSelectionStrikeThroughCommand;
        public ICommand ToggleSelectionStrikeThroughCommand => this._ToggleSelectionStrikeThroughCommand ??= new DelegateCommand(
            () => {
                ToggleSelectionDecoration(TextDecorationLocation.Strikethrough);
            }
        );

        private ICommand? _ToggleSelectionIncreaseFontSizeCommand;
        public ICommand ToggleSelectionIncreaseFontSizeCommand => this._ToggleSelectionIncreaseFontSizeCommand ??= new DelegateCommand(
            () => EditingCommands.IncreaseFontSize.Execute(null, ControlElement)
        );

        private ICommand? _ToggleSelectionDecreaseFontSizeCommand;
        public ICommand ToggleSelectionDecreaseFontSizeCommand => this._ToggleSelectionDecreaseFontSizeCommand ??= new DelegateCommand(
            () => EditingCommands.DecreaseFontSize.Execute(null, ControlElement)
        );

        private ICommand? _ToggleSelectionSubscriptCommand;
        public ICommand ToggleSelectionSubscriptCommand => this._ToggleSelectionSubscriptCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleSubscript.Execute(null, ControlElement)
        );

        private ICommand? _ToggleSelectionSuperscriptCommand;
        public ICommand ToggleSelectionSuperscriptCommand => this._ToggleSelectionSuperscriptCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleSuperscript.Execute(null, ControlElement)
        );

        private ICommand? _ToggleSelectionBoldCommand;
        public ICommand ToggleSelectionBoldCommand => this._ToggleSelectionBoldCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleBold.Execute(null, ControlElement)
        );

        private ICommand? _ToggleSelectionItalicCommand;
        public ICommand ToggleSelectionItalicCommand => this._ToggleSelectionItalicCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleItalic.Execute(null, ControlElement)
        );

        private ICommand? _ToggleNumberingCommand;
        public ICommand ToggleNumberingCommand => this._ToggleNumberingCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleNumbering.Execute(null, ControlElement)
        );

        private ICommand? _ToggleBulletsCommand;
        public ICommand ToggleBulletsCommand => this._ToggleBulletsCommand ??= new DelegateCommand(
            () => EditingCommands.ToggleBullets.Execute(null, ControlElement)
        );

        private ICommand? _AlignLeftCommand;
        public ICommand AlignLeftCommand => this._AlignLeftCommand ??= new DelegateCommand(
            () => EditingCommands.AlignLeft.Execute(null, ControlElement)
        );

        private ICommand? _AlignCenterCommand;
        public ICommand AlignCenterCommand => this._AlignCenterCommand ??= new DelegateCommand(
            () => EditingCommands.AlignCenter.Execute(null, ControlElement)
        );

        private ICommand? _AlignRightCommand;
        public ICommand AlignRightCommand => this._AlignRightCommand ??= new DelegateCommand(
            () => EditingCommands.AlignRight.Execute(null, ControlElement)
        );

        private ICommand? _AlignJustifyCommand;
        public ICommand AlignJustifyCommand => this._AlignJustifyCommand ??= new DelegateCommand(
            () => EditingCommands.AlignJustify.Execute(null, ControlElement)
        );

        #endregion

        #region function

        private void ApplySelectionCore(Action<RichTextBox, TextSelection> action)
        {
            if(BaseElement is null) {
                return;
            }
            if(ControlElement.Selection.IsEmpty) {
                return;
            }
            action(ControlElement, ControlElement.Selection);
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

        private TextRange? SearchContentCore(string searchValue, bool searchNext, TextRange searchRange, StringComparison comparisonType)
        {
            //TODO: 前検索ができねんだわ

            var start = searchNext
                ? searchRange.Start
                : searchRange.End
                ;
            var direction = searchNext
                ? LogicalDirection.Forward
                : LogicalDirection.Backward
                ;
            while(start != null) {
                var runText = start.GetTextInRun(LogicalDirection.Forward);
                Logger.LogDebug("runText: {RunText}", runText);
                if(0 < runText.Length) {
                    int index = searchNext
                        ? runText.IndexOf(searchValue, comparisonType)
                        : runText.LastIndexOf(searchValue, comparisonType)
                        ;
                    if(index != -1) {
                        var selectionStart = start.GetPositionAtOffset(index);
                        var selectionEnd = start.GetPositionAtOffset(index + searchValue.Length);
                        return new TextRange(selectionStart, selectionEnd);
                    }
                }
                start = start.GetNextContextPosition(direction);
            }

            return null;
        }

        #endregion

        #region NoteContentViewModelBase

        protected override Task<bool> LoadContentAsync(CancellationToken cancellationToken)
        {
            return DispatcherWrapper.InvokeAsync(() => {

                ControlElement.TextChanged -= Control_TextChanged;
                ControlElement.SelectionChanged -= RichTextBox_SelectionChanged;

                ControlElement.TextChanged += Control_TextChanged;
                ControlElement.SelectionChanged += RichTextBox_SelectionChanged;

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
                    if(BaseElement is null) {
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

            if(BaseElement is null) {
                return;
            }

            ControlElement.TextChanged -= Control_TextChanged;
            ControlElement.SelectionChanged -= RichTextBox_SelectionChanged;
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
                    if(BaseElement is null) {
                        Logger.LogDebug("RichTextBox が破棄されている");
                        return;
                    }

                    var noteContentConverter = new NoteContentConverter(vm.LoggerFactory);
                    var content = noteContentConverter.ToRtfString(vm.Document);
                    vm.Model?.ChangeRichTextContent(content);
                }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        public override void SearchContent(string searchValue, bool searchNext)
        {
            Logger.LogDebug("text: {SearchValue}, {SearchNext}", searchValue, searchNext);

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            TextRange searchRange;
            if(searchNext) {
                var start = ControlElement.Selection.Start.GetPositionAtOffset(1);
                if(start is null) {
                    Logger.LogWarning("start is null");
                    return;
                }
                searchRange = new TextRange(start, ControlElement.Document.ContentEnd);
            } else {
                searchRange = new TextRange(ControlElement.Document.ContentStart, ControlElement.Selection.Start);
            }

            var selectionRange = SearchContentCore(searchValue, searchNext, searchRange, comparisonType);

            if(selectionRange is null) {
                searchRange = new TextRange(ControlElement.Document.ContentStart, ControlElement.Document.ContentEnd);
                selectionRange = SearchContentCore(searchValue, searchNext, searchRange, comparisonType);
            }

            if(selectionRange is not null) {
                ControlElement.Selection.Select(selectionRange.Start, selectionRange.End);
                ControlElement.Focus();
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
            TextChangeDelayAction.SafeFlush();
        }

        #endregion


        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeDelayAction.Callback(ChangedText);
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // ツールバー的なの何かできればいいなぁと
            Debug.Assert(ControlElement != null);

            NowSelectionProcess = true;
            using var _cleanup_ = new ActionDisposer(d => NowSelectionProcess = false);

            if(ControlElement.Selection.IsEmpty) {
                // 未選択。さよなら
                return;
            }

            var inlineContainer = ControlElement.Selection.Start.GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
            if(inlineContainer != null && inlineContainer.Child is Image image) {
                if(inlineContainer == ControlElement.Selection.End.GetAdjacentElement(LogicalDirection.Backward)) {
                    // TODO: 画像選択からの処理は未実装
                    return;
                }
            }

            // 選択状態から表示可能項目の抜出
            var fontFamilyProperty = ControlElement.Selection.GetPropertyValue(RichTextBox.FontFamilyProperty);
            var fontSizeProperty = ControlElement.Selection.GetPropertyValue(RichTextBox.FontSizeProperty);
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
            var startRect = ControlElement.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
            var endRect = ControlElement.Selection.End.GetCharacterRect(LogicalDirection.Backward);
            var mixX = Math.Min(endRect.X, startRect.X);
            var maxX = Math.Max(endRect.X, startRect.X);
            var rect = new Rect(
                ControlElement.PointToScreen(new Point(maxX - mixX, endRect.Y + endRect.Height)),
                new Size(endRect.Width, endRect.Height)
            );
        }
    }
}
