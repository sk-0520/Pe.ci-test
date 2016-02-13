/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.Data;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting;
    using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Property;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;
    using Microsoft.Win32;
    using System.ComponentModel;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System.Xml;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using System.Text.RegularExpressions;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using System.Reflection;
    public class TemplateViewModel: HasViewSingleModelWrapperIndexViewModelBase<TemplateSettingModel, TemplateWindow, TemplateIndexItemCollectionModel, TemplateIndexItemModel, TemplateItemViewModel>
    {
        #region variable

        TemplateItemViewModel _selectedViewModel;
        TemplateKeywordViewModel _selectedKeyword;

        IHighlightingDefinition _highlightText;
        IHighlightingDefinition _highlightProgram;

        #endregion

        public TemplateViewModel(TemplateSettingModel model, TemplateWindow view, TemplateIndexSettingModel indexModel, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view, indexModel, appNonProcess, appSender)
        {
            InitializeSyntax();
        }

        #region property

        public TemplateItemViewModel SelectedViewModel
        {
            get { return this._selectedViewModel; }
            set
            {
                var prevViewModel = this._selectedViewModel;
                if(SetVariableValue(ref this._selectedViewModel, value)) {
                    if(HasView) {
                        View.pageSource.IsSelected = true;
                    }

                    CallReplaceModeChange();
                    if(this._selectedViewModel != null) {
                        this._selectedViewModel.PropertyChanged += SelectedViewModel_PropertyChanged;
                    }

                    if(prevViewModel != null) {
                        prevViewModel.PropertyChanged -= SelectedViewModel_PropertyChanged;

                        SaveItemViewModel(prevViewModel);
                        prevViewModel = null;
                    }
                }
            }
        }

        /// <summary>
        /// リスト部の幅。
        /// </summary>
        public double ItemsListWidth
        {
            get { return Model.ItemsListWidth; }
            set { SetModelValue(value); }
        }

        /// <summary>
        /// 置き換えリスト部の幅。
        /// </summary>
        public double ReplaceListWidth
        {
            get { return Model.ReplaceListWidth; }
            set { SetModelValue(value); }
        }

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Font); }
            //set { FontModelProperty.SetFamily(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Font); }
            //set { FontModelProperty.SetBold(Model.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Font); }
            //set { FontModelProperty.SetItalic(Model.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Font); }
            //set { FontModelProperty.SetSize(Model.Font, value, OnPropertyChanged); }
        }

        #endregion

        public TemplateKeywordViewModel SelectedKeyword
        {
            get { return this._selectedKeyword; }
            set { SetVariableValue(ref this._selectedKeyword, value); }
        }

        public IEnumerable<TemplateKeywordViewModel> KeywordList
        {
            get
            {
                if(SelectedViewModel != null && SelectedViewModel.TemplateReplaceMode != TemplateReplaceMode.None) {
                    if(SelectedViewModel.TemplateReplaceMode == TemplateReplaceMode.Program) {
                        var typeMap = TemplateReplaceKey.ProgramTypes;
                        return TemplateReplaceKey
                            .ProgramKeyList
                            .Select(k => new { Key = k, CaretInSpace = TemplateReplaceKey.caretInSpaceKeys.Any(s => s == k) })
                            .Select(v => new TemplateKeywordViewModel(v.Key, SelectedViewModel.TemplateReplaceMode, v.CaretInSpace ? null : Tuple.Create("app[\"", "\"]"), AppNonProcess) {
                                Type = TemplateReplaceKey.ProgramTypes[v.Key],
                                CaretInSpace = v.CaretInSpace,
                            })
                        ;
                    } else {
                        Debug.Assert(SelectedViewModel.TemplateReplaceMode == TemplateReplaceMode.Text);
                        return TemplateReplaceKey
                            .TextKeyList
                            .Select(k => new TemplateKeywordViewModel(k, SelectedViewModel.TemplateReplaceMode, Tuple.Create("@[", "]"), AppNonProcess))
                        ;
                    }
                } else {
                    return null;
                }
            }
        }

        public IHighlightingDefinition SyntaxHighlighting
        {
            get
            {
                
                if(SelectedViewModel != null) {
                    switch(SelectedViewModel.TemplateReplaceMode) {
                        case TemplateReplaceMode.None:
                            return null;
                        case TemplateReplaceMode.Text:
                            return this._highlightText;
                        case TemplateReplaceMode.Program:
                            return this._highlightProgram;
                        default:
                            throw new NotImplementedException();
                    }
                } else {
                    return null;
                }
            }
        }

        #endregion

        #region command

        public ICommand CreateItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var indexModel = SettingUtility.CreateTemplateIndexItem(IndexModel.Items, AppNonProcess);
                        SettingUtility.UpdateUniqueGuid(indexModel, IndexPairList.ModelList);

                        var pair = IndexPairList.Insert(0, indexModel, null);
                        SelectedViewModel = pair.ViewModel;
                        AppSender.SendSaveIndex(IndexKind.Template, Timing.Delay);
                    }
                );

                return result;
            }
        }

        public ICommand RemoveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var nowViewModel = SelectedViewModel;
                        if(nowViewModel == null) {
                            return;
                        }

                        var index = IndexPairList.ViewModelList.IndexOf(nowViewModel);

                        IndexPairList.Remove(nowViewModel);
                        AppSender.SendRemoveIndex(IndexKind.Template, nowViewModel.Model.Id, Timing.Delay);

                        if(IndexPairList.Any()) {
                            while(IndexPairList.ViewModelList.Count <= index) {
                                index -= 1;
                            }
                            SelectedViewModel = IndexPairList.ViewModelList[index];
                        }
                    }
                );

                return result;
            }
        }

        //public ICommand ListItemSelectionChangedCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				if (HasView) {
        //					View.pageSource.IsSelected = true;
        //				}
        //			}
        //		);

        //		return result;
        //	}
        //}

        public ICommand WindowDeactivCommnad
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(SelectedViewModel != null && SelectedViewModel.IsChanged) {
                            SelectedViewModel.SaveBody(Timing.Delay);
                        }
                    }
                );

                return result;
            }
        }

        public ICommand MoveUpItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        MoveItem(true, o as TemplateItemViewModel);
                    }
                );

                return result;
            }
        }

        public ICommand MoveDownItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        MoveItem(false, o as TemplateItemViewModel);
                    }
                );

                return result;
            }
        }

        public ICommand SaveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(SelectedViewModel == null) {
                            return;
                        }

                        SelectedViewModel.SetReplacedValue();
                        if(!string.IsNullOrEmpty(SelectedViewModel.Replaced)) {
                            SaveFileInDialog(SelectedViewModel);
                        }
                    }
                );

                return result;
            }
        }

        public ICommand InsertKeywordCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(SelectedViewModel == null) {
                            return;
                        }
                        if(SelectedKeyword == null) {
                            return;
                        }

                        var keyword = SelectedKeyword.Keyword;

                        if(HasView) {
                            var editControl = View.editSource;
                            var startPosition = editControl.SelectionStart;
                            editControl.SelectedText = keyword;
                            if(SelectedKeyword.CaretInSpace) {
                                var index = keyword.IndexOf(' ');
                                if(index != -1) {
                                    var count = keyword.Skip(index).TakeWhile(c => c == ' ').Count();
                                    var targetIndex = index + (int)(count / 2);
                                    editControl.Select(startPosition + targetIndex, 0);
                                }
                            } else {
                                editControl.Select(startPosition, keyword.Length);
                            }
                            editControl.Focus();
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        void MoveItem(bool moveUp, TemplateItemViewModel itemViewModel)
        {
            if(itemViewModel != null) {
                var model = itemViewModel.Model;
                var index = IndexModel.Items.IndexOf(model);
                var next = 0;
                if(moveUp) {
                    if(index == 0) {
                        return;
                    }
                    next = -1;
                } else {
                    if(index == IndexModel.Items.Count - 1) {
                        return;
                    }
                    next = +1;
                }

                IndexPairList.SwapIndex(index, index + next);
                //IndexModel.Items.SwapIndex(index, index + next);
                //IndexItems.SwapIndex(index, index + next);

                SelectedViewModel = itemViewModel;
            }
        }

        protected override TemplateItemViewModel CreateIndexViewModel(TemplateIndexItemModel model, object data)
        {
            var result = new TemplateItemViewModel(
                model,
                AppSender,
                AppNonProcess
            );

            return result;
        }

        void SaveItemViewModel(TemplateItemViewModel vm)
        {
            if(vm.IsChanged) {
                vm.SaveBody(Timing.Delay);
            }
        }

        bool SaveFileInDialog(TemplateItemViewModel vm)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(vm.Replaced);

            var filter = new DialogFilterList() {
                { new DialogFilterItem(AppNonProcess.Language["dialog/filter/txt"], Constants.dialogFilterText) },
            };

            var name = PathUtility.ToSafeNameDefault(vm.Model.Name);
            if(string.IsNullOrWhiteSpace(name)) {
                name = Constants.GetNowTimestampFileName();
            }
            //var dialog = new SaveFileDialog() {
            //	Filter = filter.FilterText,
            //	FilterIndex = 0,
            //	AddExtension = true,
            //	CheckPathExists = true,
            //	ValidateNames = true,
            //	InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            //	FileName = name,
            //};
            var dialogResult = DialogUtility.ShowSaveFileDialog(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), name, filter);

            //var dialogResult = dialog.ShowDialog();
            //if(dialogResult.GetValueOrDefault()) {
            //	return SaveFile(dialog.FileName, vm);
            //} else {
            //	return false;
            //}
            if(dialogResult != null) {
                return SaveFile(dialogResult, vm);
            } else {
                return false;
            }
        }

        bool SaveFile(string path, TemplateItemViewModel vm)
        {
            CheckUtility.EnforceNotNullAndNotEmpty(vm.Replaced);

            var writeValue = vm.Replaced;
            try {
                File.WriteAllText(path, writeValue);
                return true;
            } catch(Exception ex) {
                AppNonProcess.Logger.Error(ex);
                return false;
            }
        }

        static XshdSyntaxDefinition GetSyntaxText(INonProcess nonProcess)
        {
            // 予約語
            var keywordColor = new XshdColor() {
                Name = "KEYWORD",
                FontWeight = FontWeights.Bold,
            };
            var keywordKeywords = new XshdKeywords() {
                ColorReference = new XshdReference<XshdColor>(keywordColor),
            };
            // 値は使用しない
            var keys = TemplateUtility.GetTextTemplateMap(DateTime.MinValue, nonProcess).Keys;
            foreach(var key in keys.Select(k => $"{TemplateUtility.textReplaceKeywordHead}{k}{TemplateUtility.textReplaceKeywordTail}")) {
                keywordKeywords.Words.Add(key);
            }

            // 入力中 or なんか変な場合のワード
            var unknownColor = new XshdColor() {
                Name = "UNKNOWN",
                Underline = true,
            };
            var unknownRule = new XshdRule() {
                ColorReference = new XshdReference<XshdColor>(unknownColor),
            };
            unknownRule.Regex 
                = Regex.Escape(TemplateUtility.textReplaceKeywordHead) 
                + "(.*?)"
                + Regex.Escape(TemplateUtility.textReplaceKeywordTail)
            ;

            var ruleSet = new XshdRuleSet();
            ruleSet.Elements.Add(keywordKeywords);
            ruleSet.Elements.Add(unknownRule);

            var def = new XshdSyntaxDefinition();
            def.Elements.Add(ruleSet);

            return def;
        }

        static XshdRuleSet GetCSharpRuleSet()
        {
            var xshd = RestrictUtility.Block(() => {
                // TODO: もっと良さげなアクセス方法があるはず
                var avalonEdit = typeof(ICSharpCode.AvalonEdit.TextEditor).Assembly;
                using(var stream = avalonEdit.GetManifestResourceStream("ICSharpCode.AvalonEdit.Highlighting.Resources.CSharp-Mode.xshd")) {
                    using(var reader = new XmlTextReader(stream)) {
                        return HighlightingLoader.LoadXshd(reader);
                    }
                }
            });

            var ruleSet = xshd.Elements.OfType<XshdRuleSet>().Single(x => x.Name == null);

            // プリプロセッサ ディレクティブいらない
            var preprocessor = ruleSet.Elements.Single(e => CastUtility.AsFunc<XshdSpan, bool>(e, span => span.SpanColorReference.ReferencedElement == "Preprocessor"));
            ruleSet.Elements.Remove(preprocessor);
            var targetElements = xshd.Elements
                .Where(e => e != ruleSet)
            ;
            foreach(var elemtnt in targetElements) {
                ruleSet.Elements.Add(elemtnt);
            }

            return ruleSet;
        }

        static XshdSyntaxDefinition GetSyntaxProgram(INonProcess nonProcess)
        {
            // c#
            var csName = "C#";
            var csRuleSet = GetCSharpRuleSet();
            csRuleSet.Name = csName;

            // 標準コントロール
            var t4StandardControlColor = new XshdColor() {
                Name = "T4-STANDARD",
                Foreground = new SimpleHighlightingBrush(Constants.TemplateT4ControlColor.ForeColor),
                Background = new SimpleHighlightingBrush(Constants.TemplateT4ControlColor.BackColor),
            };
            var t4StandardControlSpan = new XshdSpan() {
                Multiline = true,
                BeginColorReference = new XshdReference<XshdColor>(null, t4StandardControlColor.Name),
                BeginRegex = "<#",
                EndColorReference = new XshdReference<XshdColor>(null, t4StandardControlColor.Name),
                EndRegex = "#>",
                RuleSetReference = new XshdReference<XshdRuleSet>(null, csName),
            };

            // クラス
            var t4ClassControlColor = new XshdColor() {
                Name = "T4-CLASS",
                Foreground = new SimpleHighlightingBrush(Constants.TemplateT4ClassColor.ForeColor),
                Background = new SimpleHighlightingBrush(Constants.TemplateT4ClassColor.BackColor),
            };
            var t4ClassSpan = new XshdSpan() {
                Multiline = true,
                BeginColorReference = new XshdReference<XshdColor>(null, t4ClassControlColor.Name),
                BeginRegex = @"<#\+",
                EndColorReference = new XshdReference<XshdColor>(null, t4ClassControlColor.Name),
                EndRegex = "#>",
                RuleSetReference = new XshdReference<XshdRuleSet>(null, csName),
            };

            // 式
            var t4ExpressionControlColor = new XshdColor() {
                Name = "T4-EXPRESSION",
                Foreground = new SimpleHighlightingBrush(Constants.TemplateT4ExpressionColor.ForeColor),
                Background = new SimpleHighlightingBrush(Constants.TemplateT4ExpressionColor.BackColor),
            };
            var t4ExpressionSpan = new XshdSpan() {
                Multiline = true,
                BeginColorReference = new XshdReference<XshdColor>(null, t4ExpressionControlColor.Name),
                BeginRegex = "<#=",
                EndColorReference = new XshdReference<XshdColor>(null, t4ExpressionControlColor.Name),
                EndRegex = "#>",
                RuleSetReference = new XshdReference<XshdRuleSet>(null, csName),
            };

            var ruleSet = new XshdRuleSet();
            ruleSet.Elements.Add(t4StandardControlColor);
            ruleSet.Elements.Add(t4ClassControlColor);
            ruleSet.Elements.Add(t4ExpressionControlColor);

            ruleSet.Elements.Add(t4ExpressionSpan);
            ruleSet.Elements.Add(t4ClassSpan);
            ruleSet.Elements.Add(t4StandardControlSpan);

            var def = new XshdSyntaxDefinition() {
                Name = "T4",
            };
            def.Elements.Add(ruleSet);
            def.Elements.Add(csRuleSet);

            return def;
        }

        void InitializeSyntax()
        {
            var syntaxText = GetSyntaxText(AppNonProcess);
            this._highlightText = HighlightingLoader.Load(syntaxText, HighlightingManager.Instance);

            var syntaxProgram = GetSyntaxProgram(AppNonProcess);
            this._highlightProgram = HighlightingLoader.Load(syntaxProgram, HighlightingManager.Instance);
        }

        void CallReplaceModeChange()
        {
            if(HasView) {
                LanguageUtility.RecursiveSetLanguage(View.listItems, AppNonProcess.Language);
            }
            var propertyNames = new[] {
                nameof(KeywordList),
                nameof(SyntaxHighlighting),
                //nameof(TemplateItemViewModel.Document),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion

        #region IWindowStatus

        public double WindowLeft
        {
            get { return WindowStatusProperty.GetWindowLeft(Model); }
            set { WindowStatusProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
        }

        public double WindowTop
        {
            get { return WindowStatusProperty.GetWindowTop(Model); }
            set { WindowStatusProperty.SetWindowTop(Model, value, OnPropertyChanged); }
        }

        public double WindowWidth
        {
            get { return WindowStatusProperty.GetWindowWidth(Model); }
            set { WindowStatusProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
        }

        public double WindowHeight
        {
            get { return WindowStatusProperty.GetWindowHeight(Model); }
            set { WindowStatusProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
        }

        public WindowState WindowState
        {
            get { return WindowStatusProperty.GetWindowState(Model); }
            set { WindowStatusProperty.SetWindowState(Model, value, OnPropertyChanged); }
        }

        #region IVisible

        public Visibility Visibility
        {
            get { return VisibleVisibilityProperty.GetVisibility(Model); }
            set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
        }

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model); }
            set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
        }

        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get { return TopMostProperty.GetTopMost(Model); }
            set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
        }

        #endregion

        #endregion

        #region HasViewSingleModelWrapperIndexViewModelBase

        protected override void InitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing += View_UserClosing;

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing -= View_UserClosing;
            //foreach(var itemViewModel in IndexPairList.ViewModelList) {
            //	itemViewModel.Unload();
            //}

            base.UninitializeView();
        }

        #endregion

        private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            IsVisible = false;
        }

        void SelectedViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var refreshTargets = new[] {
                nameof(TemplateItemViewModel.TemplateReplaceMode),
            };
            if(SelectedViewModel != null && refreshTargets.Any(s => s == e.PropertyName)) {
                CallReplaceModeChange();
            }
        }


    }
}
