using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public abstract class CustomizeDialogControlBase
    {
        #region property

        public int ControlId { get; private set; }

        protected ComWrapper<IFileDialogCustomize>? FileDialogCustomize { get; private set; }

        #endregion

        #region function

        protected abstract void BuildImpl();

        public void Build(int controlId, ComWrapper<IFileDialogCustomize> fileDialogCustomize)
        {
            ControlId = controlId;
            FileDialogCustomize = fileDialogCustomize;

            BuildImpl();
        }

        protected virtual void ChangeStatusImple()
        { }

        public void ChangeStatus()
        {
            ChangeStatusImple();
        }

        #endregion
    }

    public class CustomizeDialogGroup: CustomizeDialogControlBase
    {
        public CustomizeDialogGroup(string header)
        {
            Header = header;
        }

        #region property

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
