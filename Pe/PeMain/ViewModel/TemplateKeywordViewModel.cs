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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class TemplateKeywordViewModel: ViewModelBase, IHasNonProcess
    {
        public TemplateKeywordViewModel(string key, TemplateReplaceMode replaceMode, Tuple<string, string> bracket, INonProcess nonProcess)
        {
            CheckUtility.DebugEnforce(replaceMode != TemplateReplaceMode.None);

            Key = key;
            TemplateReplaceMode = replaceMode;
            Bracket = bracket;
            NonProcess = nonProcess;
        }

        #region property

        TemplateReplaceMode TemplateReplaceMode { get; set; }

        string Key { get; set; }

        public string Title { get { return Key; } }
        public string Comment
        {
            get
            {
                string langKey;
                if(TemplateReplaceMode == TemplateReplaceMode.Program) {
                    langKey = "template/keyword/program";
                } else {
                    langKey = "template/keyword/text";
                }

                return NonProcess.Language[langKey + "/" + Key];
            }
        }
        public string Keyword
        {
            get
            {
                var result = new StringBuilder();

                if(Type != null) {
                    result.Append("(");
                    result.Append(Type.Name);
                    result.Append(")");
                }
                if(Bracket != null) {
                    result.Append(Bracket.Item1);
                }
                result.Append(Key);
                if(Bracket != null) {
                    result.Append(Bracket.Item2);
                }

                return result.ToString();
            }
        }

        Tuple<string, string> Bracket { get; set; }

        /// <summary>
        /// Nameの示す型。
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 置き換え文字列を挿入時にスペース内部にキャレットを移動させるか。
        /// </summary>
        public bool CaretInSpace { get; set; }


        #endregion

        #region IHasNonProcess

        public INonProcess NonProcess { get; private set; }

        #endregion
    }
}
