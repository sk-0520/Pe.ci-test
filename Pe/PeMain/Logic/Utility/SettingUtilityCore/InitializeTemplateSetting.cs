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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityCore
{
    internal sealed class InitializeTemplateSetting: InitializeBase<TemplateSettingModel>
    {
        public InitializeTemplateSetting(TemplateSettingModel model, Version previousVersion, INonProcess nonProcess)
            :base(model, previousVersion, nonProcess)
        { }

        #region InitializeBase

        protected override void Correction_Last()
        {
            Model.Font.Size = Constants.templateFontSize.GetClamp(Model.Font.Size);

            if(SettingUtility.IsIllegalPlusNumber(Model.ItemsListWidth)) {
                Model.ItemsListWidth = Constants.templateItemsListWidth;
            }
            if(SettingUtility.IsIllegalPlusNumber(Model.ReplaceListWidth)) {
                Model.ReplaceListWidth = Constants.templateReplaceListWidth;
            }

            if(SettingUtility.IsIllegalPlusNumber(Model.WindowWidth)) {
                Model.WindowWidth = Constants.templateDefaultWindowSize.Width;
            }
            if(SettingUtility.IsIllegalPlusNumber(Model.WindowHeight)) {
                Model.WindowHeight = Constants.templateDefaultWindowSize.Height;
            }
        }

        protected override void Correction_First()
        {
            Model.Font.Size = Constants.templateFontSize.median;
            Model.ItemsListWidth = Constants.templateItemsListWidth;
            Model.ReplaceListWidth = Constants.templateReplaceListWidth;
            Model.WindowWidth = Constants.templateDefaultWindowSize.Width;
            Model.WindowHeight = Constants.templateDefaultWindowSize.Height;
        }

        #endregion
    }
}
