using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// 標準ダイアログに対してカスタムコントロールを提供する基底クラス。
    /// </summary>
    public abstract class CustomizeDialogControlBase
    {
        #region property

        /// <summary>
        /// コントロールID。
        /// </summary>
        /// <remarks>
        /// <para>自動割り振りされる。</para>
        /// </remarks>
        public int ControlId { get; private set; }

        /// <summary>
        /// <see cref="BuildImpl"/>で使用する生処理。
        /// </summary>
        protected Com<IFileDialogCustomize>? FileDialogCustomize { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// ビルド処理。
        /// </summary>
        /// <remarks>
        /// <para>継承先で実装すること。</para>
        /// <para><see cref="ControlId"/>, <see cref="FileDialogCustomize"/>は有効。</para>
        /// </remarks>
        protected abstract void BuildImpl();

        /// <summary>
        /// ビルド処理実施。
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="fileDialogCustomize"></param>
        internal void Build(int controlId, Com<IFileDialogCustomize> fileDialogCustomize)
        {
            ControlId = controlId;
            FileDialogCustomize = fileDialogCustomize;

            BuildImpl();
        }

        /// <summary>
        /// 状態変更内部処理。
        /// </summary>
        protected virtual void ChangeStatusImpl()
        { }

        /// <summary>
        /// 状態変更。
        /// </summary>
        internal void ChangeStatus()
        {
            ChangeStatusImpl();
        }

        #endregion
    }

    /// <summary>
    /// 標準ダイアログ用グループ。
    /// </summary>
    public class CustomizeDialogGroup: CustomizeDialogControlBase
    {
        public CustomizeDialogGroup(string header)
        {
            Header = header;
        }

        #region property

        /// <summary>
        /// ヘッダ文言。
        /// </summary>
        public string Header { get; set; }

        private ISet<CustomizeDialogControlBase> Controls { get; } = new HashSet<CustomizeDialogControlBase>();

        #endregion

        #region function

        public void Close()
        {
            FileDialogCustomize!.Instance.EndVisualGroup();
        }

        public void AddControl(CustomizeDialogControlBase control) => Controls.Add(control);

        public bool ContainsControl(CustomizeDialogControlBase control) => Controls.Contains(control);

        #endregion

        #region CustomizeDialogControlBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.Instance.StartVisualGroup(ControlId, Header);
        }

        #endregion
    }

    public class CustomizeDialogLabel: CustomizeDialogControlBase
    {
        public CustomizeDialogLabel(string label)
        {
            Label = label;
        }

        #region property

        /// <summary>
        /// ラベル文言。
        /// </summary>
        public string Label { get; set; }

        #endregion

        #region CustomizeDialogControlBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.Instance.SetControlLabel(ControlId, Label);
        }

        #endregion
    }

    /// <summary>
    /// 標準型付きダイアログ用コンボボックスアイテム。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class CustomizeDialogComboBoxItem<TValue>
    {
        public CustomizeDialogComboBoxItem(string displayText, TValue value)
        {
            DisplayText = displayText;
            Value = value;
        }

        #region property

        /// <summary>
        /// 表示文言。
        /// </summary>
        public string DisplayText { get; }
        /// <summary>
        /// 設定値。
        /// </summary>
        public TValue Value { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="CustomizeDialogComboBoxItem{T}"/>ヘルパ。
    /// </summary>
    public static class CustomizeDialogComboBoxItem
    {
        #region function

        public static CustomizeDialogComboBoxItem<TValue> Create<TValue>(string displayText, TValue value)
        {
            return new CustomizeDialogComboBoxItem<TValue>(displayText, value);
        }

        public static CustomizeDialogComboBoxItem<TValue> Create<TValue>(TValue value)
        {
            return new CustomizeDialogComboBoxItem<TValue>(value!.ToString()!, value);
        }

        #endregion
    }

    /// <summary>
    /// 標準型付きダイアログ用コンボボックス。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class CustomizeDialogComboBox<TValue>: CustomizeDialogControlBase
    {
        public CustomizeDialogComboBox()
        { }

        #region property

        /// <summary>
        /// アイテム一覧。
        /// </summary>
        private IList<CustomizeDialogComboBoxItem<TValue>> Items { get; } = new List<CustomizeDialogComboBoxItem<TValue>>();
        /// <summary>
        /// 選択インデックス。
        /// </summary>
        public int SelectedIndex { get; set; } = 0;

        #endregion

        #region function

        public void AddItem(CustomizeDialogComboBoxItem<TValue> item)
        {
            Items.Add(item);
        }

        #endregion

        #region CustomizeDialogControlBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.Instance.AddComboBox(ControlId);
            foreach(var item in Items.Counting()) {
                FileDialogCustomize!.Instance.AddControlItem(ControlId, item.Number, item.Value.DisplayText);
            }
            FileDialogCustomize!.Instance.SetSelectedControlItem(ControlId, SelectedIndex);
        }

        protected override void ChangeStatusImpl()
        {
            FileDialogCustomize!.Instance.GetSelectedControlItem(ControlId, out var index);
            SelectedIndex = index;
        }

        #endregion
    }

    /// <summary>
    /// 標準ダイアログカスタマイズ。
    /// </summary>
    /// <remarks>
    /// <para>基本的にはこれ使ってりゃOK。</para>
    /// </remarks>
    public class CustomizeDialog
    {
        #region property

        /// <summary>
        /// コントロール一覧。
        /// </summary>
        public IList<CustomizeDialogControlBase> Controls { get; } = new List<CustomizeDialogControlBase>();

        /// <summary>
        /// 現在グルーピング中か。
        /// </summary>
        public bool NowGrouping => CurrentGroup != null;
        /// <summary>
        /// 現在のグループ。
        /// </summary>
        /// <remarks>
        /// <para><see langword="null" />はグループなし。</para>
        /// </remarks>
        CustomizeDialogGroup? CurrentGroup { get; set; }
        /// <summary>
        /// ビルド済み。
        /// </summary>
        public bool IsBuilt { get; private set; }

        #endregion


        #region function

        private void AddControl(CustomizeDialogControlBase control)
        {
            Controls.Add(control);
            CurrentGroup?.AddControl(control);
        }

        /// <summary>
        /// グルーピング開始。
        /// </summary>
        /// <param name="header"></param>
        /// <returns><see cref="IDisposable.Dispose"/>することでグルーピング終了。</returns>
        public IDisposable Grouping(string header)
        {
            if(NowGrouping) {
                throw new InvalidOperationException(nameof(NowGrouping));
            }

            var control = new CustomizeDialogGroup(header);

            AddControl(control);
            CurrentGroup = control;

            return new ActionDisposer(d => CurrentGroup = null);
        }

        /// <summary>
        /// ラベル追加。
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public CustomizeDialogLabel AddLabel(string label)
        {
            var control = new CustomizeDialogLabel(label);

            AddControl(control);

            return control;
        }

        /// <summary>
        /// コンボボックス追加。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public CustomizeDialogComboBox<TValue> AddComboBox<TValue>()
        {
            var control = new CustomizeDialogComboBox<TValue>();

            AddControl(control);

            return control;
        }

        internal void Build(Com<IFileDialogCustomize> FileDialogCustomize)
        {
            if(IsBuilt) {
                FileDialogCustomize.Instance.ClearClientData();
            }

            var lastControlId = 1;
            CustomizeDialogGroup? currentGroup = null;
            foreach(var control in Controls) {

                control.Build(lastControlId++, FileDialogCustomize);

                if(control is CustomizeDialogGroup group) {
                    currentGroup = group;
                } else if(currentGroup != null && !currentGroup.ContainsControl(control)) {
                    currentGroup.Close();
                    currentGroup = null;
                }
            }

            if(currentGroup != null) {
                currentGroup.Close();
            }

            IsBuilt = true;
        }

        internal void ChangeStatus()
        {
            foreach(var control in Controls) {
                control.ChangeStatus();
            }
        }

        #endregion
    }
}
