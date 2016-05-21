/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeStreamSetting: InitializeBase<StreamSettingModel>
    {
        public InitializeStreamSetting(StreamSettingModel model, Version previousVersion, INonProcess nonProcess)
            :base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.Font.Size = Constants.streamFontSize.GetClamp(Model.Font.Size);

            if(Model.OutputColor.ForeColor == default(Color)) {
                Model.OutputColor.ForeColor = Constants.streamOutputColor.ForeColor;
            }
            if(Model.OutputColor.BackColor == default(Color)) {
                Model.OutputColor.BackColor = Constants.streamOutputColor.BackColor;
            }

            if(Model.ErrorColor.ForeColor == default(Color)) {
                Model.ErrorColor.ForeColor = Constants.streamErrorColor.ForeColor;
            }
            if(Model.ErrorColor.BackColor == default(Color)) {
                Model.ErrorColor.BackColor = Constants.streamErrorColor.BackColor;
            }
        }

        protected override void Correction_First()
        {
            Model.Font.Size = Constants.streamFontSize.median;
            //Constants.streamOutputColor.DeepCloneTo(setting.OutputColor);
            //Constants.streamErrorColor.DeepCloneTo(setting.ErrorColor);
            Model.OutputColor = (ColorPairItemModel)Constants.streamOutputColor.DeepClone();
            Model.ErrorColor = (ColorPairItemModel)Constants.streamErrorColor.DeepClone();
        }

        #endregion
    }
}
