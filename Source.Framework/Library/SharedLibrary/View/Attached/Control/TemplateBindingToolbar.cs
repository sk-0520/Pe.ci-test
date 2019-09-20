/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Control
{
    /// <summary>
    /// http://stackoverflow.com/questions/1589034/how-to-get-a-wpf-toolbar-to-bind-to-a-collection-in-my-vm-without-using-expander?answertab=votes#tab-top
    /// </summary>
    public class TemplateBindingToolBar: ToolBar
    {
        #region define

        delegate void IvalidateMeasureJob();

        #endregion

        public TemplateBindingToolBar()
            : base()
        { }

        #region function

        public override void OnApplyTemplate()
        {
            Dispatcher.BeginInvoke(
                new IvalidateMeasureJob(InvalidateMeasure),
                DispatcherPriority.Background,
                null
            );
            base.OnApplyTemplate();
        }

        #endregion
    }
}
