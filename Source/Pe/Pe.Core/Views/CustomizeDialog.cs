using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;

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
        /// <para>自動割り振りされる。</para>
        /// </summary>
        public int ControlId { get; private set; }

        /// <summary>
        /// <see cref="BuildImpl"/>で使用する生処理。
        /// </summary>
        protected ComWrapper<IFileDialogCustomize>? FileDialogCustomize { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// ビルド処理。
        /// <para>継承先で実装すること。</para>
        /// <para><see cref="ControlId"/>, <see cref="FileDialogCustomize"/>は有効。</para>
        /// </summary>
        protected abstract void BuildImpl();

        /// <summary>
        /// ビルド処理実施。
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="fileDialogCustomize"></param>
        internal void Build(int controlId, ComWrapper<IFileDialogCustomize> fileDialogCustomize)
        {
            ControlId = controlId;
            FileDialogCustomize = fileDialogCustomize;

            BuildImpl();
        }

        /// <summary>
        /// 状態変更内部処理。
        /// </summary>
        protected virtual void ChangeStatusImple()
        { }

        /// <summary>
        /// 状態変更。
        /// </summary>
        internal void ChangeStatus()
        {
            ChangeStatusImple();
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

        ISet<CustomizeDialogControlBase> Controls { get; } = new HashSet<CustomizeDialogControlBase>();

        #endregion

        #region function

        public void Close()
        {
            FileDialogCustomize!.Raw.EndVisualGroup();
        }

        public void AddControl(CustomizeDialogControlBase control) => Controls.Add(control);

        public bool ContainsControl(CustomizeDialogControlBase control) => Controls.Contains(control);

        #endregion

        #region CustomizeDialogControlBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.Raw.StartVisualGroup(ControlId, Header);
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

        public string Label { get; set; }
        #endregion

        #region CustomizeDialogControlBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.Raw.SetControlLabel(ControlId, Label);
        }

        #endregion
    }

    public class CustomizeDialogComboBoxItem<TValue>
    {
        public CustomizeDialogComboBoxItem(string displayText, TValue value)
        {
            DisplayText = displayText;
            Value = value;
        }

        #region property

        public string DisplayText { get; }
        public TValue Value { get; }
        #endregion
    }

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


    public class CustomizeDialogComboBox<TValue>: CustomizeDialogControlBase
    {
        public CustomizeDialogComboBox()
        { }

        #region property

        IList<CustomizeDialogComboBoxItem<TValue>> Items { get; } = new List<CustomizeDialogComboBoxItem<TValue>>();
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
            FileDialogCustomize!.Raw.AddComboBox(ControlId);
            foreach(var item in Items.Counting()) {
                FileDialogCustomize!.Raw.AddControlItem(ControlId, item.Number, item.Value.DisplayText);
            }
            FileDialogCustomize!.Raw.SetSelectedControlItem(ControlId, SelectedIndex);
        }

        protected override void ChangeStatusImple()
        {
            FileDialogCustomize!.Raw.GetSelectedControlItem(ControlId, out var index);
            SelectedIndex = index;
        }

        #endregion
    }

    public class CustomizeDialog
    {
        #region property

        public IList<CustomizeDialogControlBase> Controls { get; } = new List<CustomizeDialogControlBase>();

        public bool NowGrouping => CurrentGroup != null;
        CustomizeDialogGroup? CurrentGroup { get; set; }

        public bool IsBuilded { get; private set; }

        #endregion


        #region function

        private void AddControl(CustomizeDialogControlBase control)
        {
            Controls.Add(control);
            CurrentGroup?.AddControl(control);
        }

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

        public CustomizeDialogLabel AddLabel(string label)
        {
            var control = new CustomizeDialogLabel(label);

            AddControl(control);

            return control;
        }

        public CustomizeDialogComboBox<TValue> AddComboBox<TValue>()
        {
            var control = new CustomizeDialogComboBox<TValue>();

            AddControl(control);

            return control;
        }

        internal void Build(ComWrapper<IFileDialogCustomize> FileDialogCustomize)
        {
            if(IsBuilded) {
                FileDialogCustomize.Raw.ClearClientData();
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

            IsBuilded = true;
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
