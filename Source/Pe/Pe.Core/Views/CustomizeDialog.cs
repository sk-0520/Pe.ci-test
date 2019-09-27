using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public abstract class CustomizeDialogItemBase
    {
        #region property

        public int ControlId { get; private set; }

        protected IFileDialogCustomize? FileDialogCustomize { get; private set; }

        #endregion

        #region function

        protected abstract void BuildImpl();

        public void Build(int controlId, IFileDialogCustomize fileDialogCustomize)
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

    public class CustomizeDialogGroup : CustomizeDialogItemBase
    {
        public CustomizeDialogGroup(string header)
        {
            Header = header;
        }

        #region property

        public string Header { get; set; }
        public ISet<CustomizeDialogItemBase> Controls { get; } = new HashSet<CustomizeDialogItemBase>();

        #endregion

        #region function

        public void Close()
        {
            FileDialogCustomize!.EndVisualGroup();
        }

        #endregion

        #region CustomizeDialogItemBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.StartVisualGroup(ControlId, Header);
        }

        #endregion
    }

    public class CustomizeDialogLabel : CustomizeDialogItemBase
    {
        public CustomizeDialogLabel(string label)
        {
            Label = label;
        }

        #region property

        public string Label { get; set; }
        #endregion

        #region CustomizeDialogItemBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.SetControlLabel(ControlId, Label);
        }

        #endregion
    }

    public class CustomizeDialogComboBox : CustomizeDialogItemBase
    {
        public CustomizeDialogComboBox()
        { }

        #region property

        public IList<string> Items { get; } = new List<string>();
        public int SelectedIndex { get; set; } = 0;

        #endregion

        #region CustomizeDialogItemBase

        protected override void BuildImpl()
        {
            FileDialogCustomize!.AddComboBox(ControlId);
            foreach(var item in Items.Counting()) {
                FileDialogCustomize!.AddControlItem(ControlId, item.Number, item.Value);
            }
            FileDialogCustomize!.SetSelectedControlItem(ControlId, SelectedIndex);
        }

        protected override void ChangeStatusImple()
        {
            FileDialogCustomize!.GetSelectedControlItem(ControlId, out var index);
            SelectedIndex = index;
        }

        #endregion
    }

    public class CustomizeDialog
    {
        #region property

        public IList<CustomizeDialogItemBase> Controls { get; } = new List<CustomizeDialogItemBase>();

        public bool NowGrouping => CurrentGroup != null;
        CustomizeDialogGroup? CurrentGroup { get; set; }

        public bool IsBuilded { get; private set; }

        #endregion


        #region function

        private void AddControl(CustomizeDialogItemBase control)
        {
            Controls.Add(control);
            CurrentGroup?.Controls.Add(control);
        }

        public IDisposable Grouping(string header)
        {
            if(NowGrouping) {
                throw new InvalidOperationException(nameof(NowGrouping));
            }

            var control = new CustomizeDialogGroup(header);

            AddControl(control);
            CurrentGroup = control;

            return new ActionDisposer(() => CurrentGroup = null);
        }

        public CustomizeDialogLabel AddLabel(string label)
        {
            var control = new CustomizeDialogLabel(label);

            AddControl(control);

            return control;
        }

        public CustomizeDialogComboBox AddComboBox()
        {
            var control = new CustomizeDialogComboBox();

            AddControl(control);

            return control;
        }

        internal void Build(IFileDialogCustomize FileDialogCustomize)
        {
            if(IsBuilded) {
                FileDialogCustomize.ClearClientData();
            }

            var lastControlId = 1;
            CustomizeDialogGroup? currentGroup = null;
            foreach(var control in Controls) {

                control.Build(lastControlId++, FileDialogCustomize);

                if(control is CustomizeDialogGroup group) {
                    currentGroup = group;
                } else if(currentGroup != null && !currentGroup.Controls.Contains(control)) {
                    currentGroup.Close();
                    currentGroup = null;
                }
            }
            if(currentGroup != null) {
                currentGroup.Close();
#pragma warning disable IDE0059 // 値の不必要な割り当て
                currentGroup = null;
#pragma warning restore IDE0059 // 値の不必要な割り当て
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
