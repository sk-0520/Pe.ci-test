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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using System.IO;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// 各種情報を取得する。
    /// </summary>
    public class InformationCollection: IDisposable
    {
        protected ManagementClass _managementOS = new ManagementClass("Win32_OperatingSystem");
        protected ManagementClass _managementCPU = new ManagementClass("Win32_Processor");

        public virtual FileVersionInfo GetVersionInfo
        {
            get { return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        protected virtual void Dispose(bool disposing)
        {
            this._managementOS.Dispose();
            this._managementCPU.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="managementClass"></param>
        /// <param name="groupName"></param>
        /// <param name="selectKeys">取り出すキー名を指定。nullで制限なし。</param>
        /// <returns></returns>
        protected virtual InformationGroup GetInfo(ManagementClass managementClass, string groupName, IEnumerable<string> selectKeys)
        {
            var result = new InformationGroup(groupName);
            using(var mc = managementClass.GetInstances()) {
                foreach(var mo in mc) {
                    var collection = mo.Properties
                        .OfType<PropertyData>()
                        .If(
                            selectKeys != null,
                            ps => ps.Where(p => selectKeys.Contains(p.Name))
                        )
                    ;
                    foreach(var property in collection) {
                        result.Items[property.Name] = property.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual InformationGroup GetCPU()
        {
            var result = GetInfo(this._managementCPU, "CPU", null);
            return result;
        }

        /// <summary>
        /// メモリ情報取得
        /// </summary>
        /// <returns></returns>
        public virtual InformationGroup GetMemory()
        {
            var result = GetInfo(this._managementOS, "Memory", null);
            return result;
        }

        public virtual InformationGroup GetEnvironment()
        {
            var result = new InformationGroup("Environment");

            result.Items["CommandLine"] = Environment.CommandLine;
            result.Items["CurrentDirectory"] = Environment.CurrentDirectory;
            result.Items["CurrentManagedThreadId"] = Environment.CurrentManagedThreadId;
            result.Items["ExitCode"] = Environment.ExitCode;
            result.Items["HasShutdownStarted"] = Environment.HasShutdownStarted;
            result.Items["Is64BitOperatingSystem"] = Environment.Is64BitOperatingSystem;
            result.Items["Is64BitProcess"] = Environment.Is64BitProcess;
            result.Items["MachineName"] = Environment.MachineName;
            result.Items["NewLine"] = BitConverter.ToString(Encoding.UTF8.GetBytes(Environment.NewLine));
            result.Items["OSVersion"] = Environment.OSVersion;
            result.Items["ProcessorCount"] = Environment.ProcessorCount;
            //result.Items["StackTrace"] = Environment.StackTrace;
            result.Items["SystemDirectory"] = Environment.SystemDirectory;
            result.Items["SystemPageSize"] = Environment.SystemPageSize;
            result.Items["TickCount"] = Environment.TickCount;
            result.Items["UserDomainName"] = Environment.UserDomainName;
            result.Items["UserInteractive"] = Environment.UserInteractive;
            result.Items["UserName"] = Environment.UserName;
            result.Items["Version"] = Environment.Version;
            result.Items["WorkingSet"] = Environment.WorkingSet;

            foreach(DictionaryEntry pair in Environment.GetEnvironmentVariables()) {
                result.Items[(string)pair.Key] = pair.Value;
            }

            return result;
        }

        public virtual InformationGroup GetApplication()
        {
            var result = new InformationGroup("Application");

            var versionInfo = GetVersionInfo;

            // バージョン番号
            result.Items["FileVersion"] = versionInfo.FileVersion;
            // メジャーバージョン番号
            result.Items["FileMajorPart"] = versionInfo.FileMajorPart;
            // マイナバージョン番号
            result.Items["FileMinorPart"] = versionInfo.FileMinorPart;
            // プライベートパート番号
            result.Items["FilePrivatePart"] = versionInfo.FilePrivatePart;
            // ビルド番号
            result.Items["FileBuildPart"] = versionInfo.FileBuildPart;
            // プライベートバージョン
            result.Items["PrivateBuild"] = versionInfo.PrivateBuild;
            // スペシャルビルド
            result.Items["SpecialBuild"] = versionInfo.SpecialBuild;

            // 説明
            result.Items["FileDescription"] = versionInfo.FileDescription;
            // 著作権
            result.Items["LegalCopyright"] = versionInfo.LegalCopyright;
            // 会社名
            result.Items["CompanyName"] = versionInfo.CompanyName;
            // コメント
            result.Items["Comments"] = versionInfo.Comments;
            // 内部名
            result.Items["InternalName"] = versionInfo.InternalName;
            // 言語
            result.Items["Language"] = versionInfo.Language;
            // 商標
            result.Items["LegalTrademarks"] = versionInfo.LegalTrademarks;
            // オリジナルファイル名
            result.Items["OriginalFilename"] = versionInfo.OriginalFilename;

            // 製品名
            result.Items["ProductName"] = versionInfo.ProductName;
            // 製品バージョン
            result.Items["ProductVersion"] = versionInfo.ProductVersion;
            // 製品メジャーバージョン番号
            result.Items["ProductMajorPart"] = versionInfo.ProductMajorPart;
            // 製品マイナバージョン番号
            result.Items["ProductMinorPart"] = versionInfo.ProductMinorPart;
            // 製品プライベートバージョン番号
            result.Items["ProductPrivatePart"] = versionInfo.ProductPrivatePart;
            // 製品ビルド番号
            result.Items["ProductBuildPart"] = versionInfo.ProductBuildPart;

            // デバッグ情報があるか
            result.Items["IsDebug"] = versionInfo.IsDebug;
            // パッチされているか
            result.Items["IsPatched"] = versionInfo.IsPatched;
            // プレリリースか
            result.Items["IsPreRelease"] = versionInfo.IsPreRelease;
            // スペシャルビルドか
            result.Items["IsSpecialBuild"] = versionInfo.IsSpecialBuild;

            return result;
        }


        public virtual InformationGroup GetScreen()
        {
            var result = new InformationGroup("Screen");
            var screens = Screen.AllScreens;
            for(var i = 0; i < screens.Length; i++) {
                var screen = screens[i];
                var head = string.Format("screen[{0}].", i);
                result.Items.Add(head + "BitsPerPixel", screen.BitsPerPixel);
                result.Items.Add(head + "DeviceBounds", screen.DeviceBounds);
                result.Items.Add(head + "DeviceName", screen.DeviceName);
                result.Items.Add(head + "Primary", screen.Primary);
                result.Items.Add(head + "DeviceWorkingArea", screen.DeviceWorkingArea);
            }

            return result;
        }

        public virtual IEnumerable<InformationGroup> Get()
        {
            return new[] {
                GetApplication(),
                GetCPU(),
                GetMemory(),
                GetEnvironment(),
                GetScreen(),
            };
        }

        public void WriteInformation(TextWriter writer)
        {
            foreach(var infoGroup in Get()) {
                infoGroup.WriteInformation(writer);
                writer.WriteLine();
            }
        }


        public override string ToString()
        {
            using(var writer = new StringWriter()) {
                WriteInformation(writer);
                return writer.ToString();
            }
        }
    }
}
