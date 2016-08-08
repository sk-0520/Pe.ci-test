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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
    public class IndexBodyArchive: DisposeFinalizeBase
    {
        #region property

        public ZipArchive Body { get; private set; }

        public bool EnabledArchive => Body != null;

        string Path { get; set; }

        #endregion

        #region function

        static string GetFilePath(IndexKind indexKind, VariableConstants variableConstants)
        {
            return Environment.ExpandEnvironmentVariables(IndexItemUtility.GetBodyArchiveFilePath(indexKind, variableConstants));
        }

        void OpenArchiveFileCore()
        {
            FileUtility.MakeFileParentDirectory(Path);
            Body = new ZipArchive(new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), ZipArchiveMode.Update);
        }

        public void OpenArchiveFile(IndexKind indexKind, VariableConstants variableConstants)
        {
            Path = GetFilePath(indexKind, variableConstants);
            OpenArchiveFileCore();
        }

        public void OpenIfExists(IndexKind indexKind, VariableConstants variableConstants)
        {
            var path = Environment.ExpandEnvironmentVariables(IndexItemUtility.GetBodyArchiveFilePath(indexKind, variableConstants));
            if(File.Exists(path)) {
                OpenArchiveFile(indexKind, variableConstants);
            }
        }

        public void Flush()
        {
            if(EnabledArchive) {
                Body.Dispose();
                OpenArchiveFileCore();
            }
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Body != null) {
                    Body.Dispose();
                    Body = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
