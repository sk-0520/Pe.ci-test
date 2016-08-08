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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
//	using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;
using System.Windows.Controls;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public class ClipboardUtility
    {
        #region define

        class CaseInsensitiveComparer: IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
            }
        }

        #endregion

        /// <summary>
        /// コピー処理の親玉。
        /// </summary>
        /// <param name="action">コピー処理。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        static void Copy(Action action, IClipboardWatcher watcher)
        {
            CheckUtility.DebugEnforceNotNull(watcher);

            bool? enabledWatch = null;
            if(!watcher.ClipboardEnabledApplicationCopy) {
                enabledWatch = watcher.ClipboardWatching;
                if(enabledWatch.Value) {
                    watcher.ClipboardWatchingChange(false);
                }
            }

            try {
                // TODO: 再試行用ロジック未使用, 定数直打ち
                var count = 0;
                var maxCount = 5;
                do {
                    try {
                        action();
                        break;
                    } catch(Exception ex) {
                        Debug.WriteLine(ex);
                    }
                } while(count++ < maxCount);
            } finally {
                if(enabledWatch.HasValue) {
                    Debug.Assert(!watcher.ClipboardEnabledApplicationCopy);
                    Debug.Assert(watcher != null);

                    if(enabledWatch.Value) {
                        watcher.ClipboardWatchingChange(true);
                    }
                }
            }
        }

        /// <summary>
        /// 文字列をクリップボードへ転写。
        /// </summary>
        /// <param name="text">対象文字列。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyText(string text, IClipboardWatcher watcher)
        {
            Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), watcher);
        }

        /// <summary>
        /// RTFをクリップボードへ転写。
        /// </summary>
        /// <param name="rtf">対象RTF。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyRtf(string rtf, IClipboardWatcher watcher)
        {
            Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), watcher);
        }

        /// <summary>
        /// HTMLをクリップボードへ転写。
        /// </summary>
        /// <param name="html">対象HTML。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyHtml(string html, IClipboardWatcher watcher)
        {
            Copy(() => Clipboard.SetText(html, TextDataFormat.Html), watcher);
        }

        /// <summary>
        /// 画像をクリップボードへ転写。
        /// </summary>
        /// <param name="image">対象画像。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyImage(BitmapSource image, IClipboardWatcher watcher)
        {
            Copy(() => Clipboard.SetImage(image), watcher);
        }

        /// <summary>
        /// ファイルをクリップボードへ転写。
        /// </summary>
        /// <param name="file">対象ファイル。</param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyFile(IEnumerable<string> file, IClipboardWatcher watcher)
        {
            var sc = TextUtility.ToStringCollection(file);
            Copy(() => Clipboard.SetFileDropList(sc), watcher);
        }

        /// <summary>
        /// 複合データをクリップノードへ転写。
        /// <para>基本的にはvoid CopyClipboardItem(ClipboardItem, ClipboardSetting)を使用する</para>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
        public static void CopyDataObject(IDataObject data, IClipboardWatcher watcher)
        {
            Copy(() => Clipboard.SetDataObject(data), watcher);
        }

        public static void CopyClipboardItem(ClipboardData clipboardItem, IClipboardWatcher watcher)
        {
            Debug.Assert(clipboardItem.Type != ClipboardType.None);

            var data = new DataObject();
            var typeFuncs = new Dictionary<ClipboardType, Action>() {
                { ClipboardType.Text, () => data.SetText(clipboardItem.Body.Text, TextDataFormat.UnicodeText) },
                { ClipboardType.Rtf, () => data.SetText(clipboardItem.Body.Rtf, TextDataFormat.Rtf) },
                { ClipboardType.Html, () => data.SetText(clipboardItem.Body.Html, TextDataFormat.Html) },
                { ClipboardType.Image, () => data.SetImage(clipboardItem.Body.Image) },
                { ClipboardType.Files, () => {
                    data.SetFileDropList(TextUtility.ToStringCollection(clipboardItem.Body.Files));
                }},
            };
            foreach(var type in ClipboardUtility.GetClipboardTypeList(clipboardItem.Type)) {
                typeFuncs[type]();
            }
            CopyDataObject(data, watcher);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="rawHtml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        static string ConvertStringFromRawHtml(RangeModel<int> range, byte[] rawHtml, Encoding encoding)
        {
            if(-1 < range.Head && -1 < range.Tail && range.Head <= range.Tail) {
                var raw = rawHtml.Skip(range.Head).Take(range.Tail - range.Head);
                return encoding.GetString(raw.ToArray());
            }

            return null;
        }

        /// <summary>
        /// <para>UTF-8</para>
        /// </summary>
        /// <param name="range"></param>
        /// <param name="rawHtml"></param>
        /// <returns></returns>
        static string ConvertStringFromDefaultRawHtml(RangeModel<int> range, byte[] rawHtml)
        {
            return ConvertStringFromRawHtml(range, rawHtml, Encoding.UTF8);
        }

        public static ContentTypeTextNet.Pe.PeMain.Data.ClipboardHtmlData ConvertClipboardHtmlFromFromRawHtml(string rawClipboardHtml, INonProcess nonProcess)
        {
            var result = new ContentTypeTextNet.Pe.PeMain.Data.ClipboardHtmlData();

            //Version:0.9
            //StartHTML:00000213
            //EndHTML:00001173
            //StartFragment:00000247
            //EndFragment:00001137
            //SourceURL:file:///C:/Users/sk/Documents/Programming/SharpDevelop%20Project/Pe/Pe/PeMain/etc/lang/ja-JP.accept.html

            var map = new Dictionary<string, Action<string>>() {
                { "version", s => result.Version = decimal.Parse(s) },
                { "starthtml", s => result.Html.Head = int.Parse(s) },
                { "endhtml", s => result.Html.Tail = int.Parse(s) },
                { "startfragment", s => result.Fragment.Head = int.Parse(s) },
                { "endfragment", s => result.Fragment.Tail = int.Parse(s) },
                { "sourceurl", s => result.SourceURL = new Uri(s) },
            };
            var reg = new Regex(@"
				^\s*
				(?<KEY>
					Version
					|StartHTML
					|EndHTML
					|StartFragment
					|EndFragment
					|SourceURL
				)
				\s*:\s*
				(?<VALUE>
					.+?
				)
				\s*$
				",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
            );
            for(var match = reg.Match(rawClipboardHtml); match.Success; match = match.NextMatch()) {
                var key = match.Groups["KEY"].Value.ToLower();
                var value = match.Groups["VALUE"].Value;
                try {
                    map[key](value);
                } catch(Exception ex) {
                    nonProcess.Logger.Warning(ex);
                }
            }

            var rawHtml = Encoding.UTF8.GetBytes(rawClipboardHtml);
            result.HtmlText = ConvertStringFromDefaultRawHtml(result.Html, rawHtml);
            result.FragmentText = ConvertStringFromDefaultRawHtml(result.Fragment, rawHtml);
            result.SelectionText = ConvertStringFromDefaultRawHtml(result.Selection, rawHtml);

            return result;
        }

        /// <summary>
        /// .NET Frameworkからクリップボードデータを取得
        /// </summary>
        /// <param name="enabledTypes"></param>
        /// <returns></returns>
        static ClipboardData GetClipboardDataFromFramework(ClipboardType enabledTypes, INonProcess nonProcess)
        {
            var clipboardData = new ClipboardData();
            SettingUtility.InitializeClipboardBodyItem(clipboardData.Body, true, nonProcess);

            var clipboardObject = Clipboard.GetDataObject();
            if(clipboardObject != null) {
                if(enabledTypes.HasFlag(ClipboardType.Text)) {
                    if(clipboardObject.GetDataPresent(DataFormats.UnicodeText)) {
                        clipboardData.Body.Text = (string)clipboardObject.GetData(DataFormats.UnicodeText);
                        clipboardData.Type |= ClipboardType.Text;
                    } else if(clipboardObject.GetDataPresent(DataFormats.Text)) {
                        clipboardData.Body.Text = (string)clipboardObject.GetData(DataFormats.Text);
                        clipboardData.Type |= ClipboardType.Text;
                    }
                }

                if(enabledTypes.HasFlag(ClipboardType.Rtf) && clipboardObject.GetDataPresent(DataFormats.Rtf)) {
                    clipboardData.Body.Rtf = (string)clipboardObject.GetData(DataFormats.Rtf);
                    clipboardData.Type |= ClipboardType.Rtf;
                }

                if(enabledTypes.HasFlag(ClipboardType.Html) && clipboardObject.GetDataPresent(DataFormats.Html)) {
                    clipboardData.Body.Html = (string)clipboardObject.GetData(DataFormats.Html);
                    clipboardData.Type |= ClipboardType.Html;
                }

                if(enabledTypes.HasFlag(ClipboardType.Image) && clipboardObject.GetDataPresent(DataFormats.Bitmap)) {
                    var image = clipboardObject.GetData(DataFormats.Bitmap) as BitmapSource;
                    if(image != null) {
                        var bitmap = new FormatConvertedBitmap(image, PixelFormats.Bgr32, null, 0);
                        if(bitmap.CanFreeze) {
                            bitmap.Freeze();
                        }
                        clipboardData.Body.Image = bitmap;
                        clipboardData.Type |= ClipboardType.Image;
                    }
                }

                if(enabledTypes.HasFlag(ClipboardType.Files) && clipboardObject.GetDataPresent(DataFormats.FileDrop)) {
                    var files = clipboardObject.GetData(DataFormats.FileDrop) as string[];
                    if(files != null) {
                        var sortedFiles = files.OrderBy(s => s, new CaseInsensitiveComparer());
                        clipboardData.Body.Files.AddRange(sortedFiles);
                        clipboardData.Body.Text = string.Join(Environment.NewLine, sortedFiles);
                        clipboardData.Type |= ClipboardType.Text | ClipboardType.Files;
                    }
                }
            }

            return clipboardData;
        }

        /// <summary>
        /// ハッシュ算出用アルゴリズムの取得。
        /// </summary>
        /// <param name="hashType"></param>
        /// <returns></returns>
        static HashAlgorithm GetHashAlgorithm(HashType hashType)
        {
            switch(hashType) {
                case HashType.SHA1:
                    return new SHA1CryptoServiceProvider();

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 文字列からハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromString(HashType hashType, string s)
        {
            using(var hash = GetHashAlgorithm(hashType)) {
                var binary = Encoding.Unicode.GetBytes(s);
                return hash.ComputeHash(binary);
            }
        }

        /// <summary>
        /// テキストからハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromText(HashType hashType, ClipboardBodyItemModel bodyItem)
        {
            return CalculateHashCodeFromString(hashType, bodyItem.Text);
        }

        /// <summary>
        /// RTFからハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromRtf(HashType hashType, ClipboardBodyItemModel bodyItem)
        {
            return CalculateHashCodeFromString(hashType, bodyItem.Rtf);
        }

        /// <summary>
        /// HTMLからハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromHtml(HashType hashType, ClipboardBodyItemModel bodyItem)
        {
            return CalculateHashCodeFromString(hashType, bodyItem.Html);
        }

        /// <summary>
        /// 画像からハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromImage(HashType hashType, ClipboardBodyItemModel bodyItem)
        {
            byte[] binaryWidth, binaryHeight;
            if(!bodyItem.Image.IsFrozen) {
                binaryWidth = BitConverter.GetBytes(bodyItem.Image.PixelWidth);
                binaryHeight = BitConverter.GetBytes(bodyItem.Image.PixelHeight);
            } else {
                var image = BitmapFrame.Create(bodyItem.Image);
                image.Freeze();
                binaryWidth = BitConverter.GetBytes(image.PixelWidth);
                binaryHeight = BitConverter.GetBytes(image.PixelHeight);
            }

            var binaryImage = bodyItem.ImageCore;
            var binaryList = new[] {
                binaryWidth,
                binaryHeight,
                binaryImage,
            };

            return CalculateHashCodeFromBinaryList(hashType, binaryList);
        }

        /// <summary>
        /// ファイルからハッシュ値を算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromFiles(HashType hashType, ClipboardBodyItemModel bodyItem)
        {
            var binaryList = bodyItem.Files.Select((s, i) => Encoding.Unicode.GetBytes($"{s}{i}"));
            return CalculateHashCodeFromBinaryList(hashType, binaryList);
        }

        /// <summary>
        /// バイナリ群からハッシュ値の算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="binaryList"></param>
        /// <returns></returns>
        static byte[] CalculateHashCodeFromBinaryList(HashType hashType, IEnumerable<byte[]> binaryList)
        {
            using(var stream = new MemoryStream()) {
                foreach(var binary in binaryList) {
                    stream.Write(binary, 0, binary.Length);
                }

                using(var hash = GetHashAlgorithm(hashType)) {
                    stream.Seek(0, SeekOrigin.Begin);
                    return hash.ComputeHash(stream);
                }
            }
        }

        /// <summary>
        /// 各種値からハッシュ値の算出。
        /// </summary>
        /// <param name="hashType"></param>
        /// <param name="clipboardType"></param>
        /// <param name="bodyItem"></param>
        /// <returns></returns>
        public static byte[] CalculateHashCode(HashType hashType, ClipboardType clipboardType, ClipboardBodyItemModel bodyItem)
        {
            var map = new Dictionary<ClipboardType, Func<HashType, ClipboardBodyItemModel, byte[]>>() {
                { ClipboardType.Text, CalculateHashCodeFromText },
                { ClipboardType.Rtf, CalculateHashCodeFromRtf },
                { ClipboardType.Html, CalculateHashCodeFromHtml },
                { ClipboardType.Image, CalculateHashCodeFromImage },
                { ClipboardType.Files, CalculateHashCodeFromFiles },
            };
            var binaryDataList = map
                .Where(p => clipboardType.HasFlag(p.Key))
                .Select(p => p.Value(hashType, bodyItem))
                .ToList()
            ;
            var binaryClipboardType = BitConverter.GetBytes((int)clipboardType);
            binaryDataList.Add(binaryClipboardType);

            return CalculateHashCodeFromBinaryList(hashType, binaryDataList);
        }

        /// <summary>
        /// <see cref="GetClipboardData"/>の内部実装。
        /// </summary>
        /// <param name="enabledTypes"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        static ClipboardData GetClipboardDataCore(ClipboardType enabledTypes, IntPtr hWnd, INonProcess nonProcess)
        {
            var clipboardItem = GetClipboardDataFromFramework(enabledTypes, nonProcess);

            return clipboardItem;
        }
        /// <summary>
        /// 現在のクリップボードからクリップボードアイテムを生成する。
        /// </summary>
        /// <param name="enabledTypes">取り込み対象とするクリップボード種別。</param>
        /// <returns>生成されたクリップボードアイテム。nullが返ることはない。</returns>
        public static ClipboardData GetClipboardData(ClipboardType enabledTypes, IntPtr hWnd, INonProcess nonProcess)
        {
            return GetClipboardDataCore(enabledTypes, hWnd, nonProcess);
        }

        static void OutputFilter(ClipboardType clipboardType, int settingLength, int currentLength, INonProcess nonProcess, int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            var messagemap = new Dictionary<string, string>() {
                { LanguageKey.logClipboardFilterType, LanguageUtility.GetTextFromEnum(clipboardType, nonProcess.Language) },
            };
            var detailMap = new Dictionary<string, string>() {
                { LanguageKey.logClipboardFilterLengthCurrent, currentLength.ToString() },
                { LanguageKey.logClipboardFilterLengthSetting, settingLength.ToString() },
            };
            var message = nonProcess.Language["log/clipboard/filter/message", messagemap];
            var detail = nonProcess.Language["log/clipboard/filter-length/detail", detailMap];

            nonProcess.Logger.Debug(message, detail, frame, callerFile, callerLine, callerMember);
        }


        public static void FilterLimitSize(ClipboardData clipboardData, ClipboardLimitSizeItemModel limitSize, INonProcess nonProcess)
        {
            CheckUtility.DebugEnforceNotNull(clipboardData);
            CheckUtility.DebugEnforceNotNull(limitSize);

            var types = ClipboardUtility.GetClipboardTypeList(clipboardData.Type & ~ClipboardType.Files);
            var limitType = limitSize.LimitType;
            foreach(var type in types) {
                if(limitType.HasFlag(type)) {
                    switch(type) {
                        case ClipboardType.Text:
                            {
                                var textLength = clipboardData.Body.Text.Length;
                                if(limitSize.Text < textLength) {
                                    clipboardData.Type &= ~type;
                                    OutputFilter(type, limitSize.Text, textLength, nonProcess);
                                }
                            }
                            break;

                        case ClipboardType.Rtf:
                            {
                                var bynaryRtfLength = Encoding.ASCII.GetByteCount(clipboardData.Body.Rtf);
                                if(limitSize.Rtf < bynaryRtfLength) {
                                    clipboardData.Type &= ~type;
                                    OutputFilter(type, limitSize.Rtf, bynaryRtfLength, nonProcess);
                                }
                            }
                            break;

                        case ClipboardType.Html:
                            {
                                var bynaryHtmlLength = Encoding.UTF8.GetByteCount(clipboardData.Body.Html);
                                if(limitSize.Html < bynaryHtmlLength) {
                                    clipboardData.Type &= ~type;
                                    OutputFilter(type, limitSize.Html, bynaryHtmlLength, nonProcess);
                                }
                            }
                            break;

                        case ClipboardType.Image:
                            {
                                var limitOverWidth = limitSize.ImageWidth < clipboardData.Body.Image.PixelWidth;
                                var limitOverHeight = limitSize.ImageHeight < clipboardData.Body.Image.PixelHeight;
                                if(limitOverWidth || limitOverHeight) {
                                    clipboardData.Type &= ~type;
                                    var messagemap = new Dictionary<string, string>() {
                                        { LanguageKey.logClipboardFilterType, LanguageUtility.GetTextFromEnum(type, nonProcess.Language) },
                                    };
                                    var detailMap = new Dictionary<string, string>() {
                                        { LanguageKey.logClipboardFilterImageWidthCurrent, clipboardData.Body.Image.PixelWidth.ToString() },
                                        { LanguageKey.logClipboardFilterImageHeightCurrent, clipboardData.Body.Image.PixelHeight.ToString() },
                                        { LanguageKey.logClipboardFilterImageWidthSetting, limitSize.ImageWidth.ToString() },
                                        { LanguageKey.logClipboardFilterImageHeightSetting, limitSize.ImageHeight.ToString() },
                                    };
                                    var message = nonProcess.Language["log/clipboard/filter/message", messagemap];
                                    var detail = nonProcess.Language["log/clipboard/filter-image/detail", detailMap];

                                    nonProcess.Logger.Debug(message, detail);
                                }
                            }
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        /// <summary>
        /// 指定ウィンドウハンドルに文字列を転送する。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="outputText"></param>
        /// <param name="nonProcess"></param>
        /// <param name="clipboardWatcher"></param>
        public static void OutputTextForWindowHandle(IntPtr hWnd, string outputText, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
        {
            if(string.IsNullOrEmpty(outputText)) {
                nonProcess.Logger.Information("empty");
                return;
            }

            if(hWnd == IntPtr.Zero) {
                nonProcess.Logger.Warning("notfound");
                return;
            }

            NativeMethods.SetForegroundWindow(hWnd);
            if(clipboardWatcher.UsingClipboard) {
                // 現在クリップボードを一時退避
                //var clipboardItem = ClipboardUtility.GetClipboardData_Impl(ClipboardType.All, hWnd);
                var clipboardData = ClipboardUtility.GetClipboardDataDefault(ClipboardType.All, hWnd, nonProcess);
                if(clipboardData != null) {
                    try {
                        ClipboardUtility.CopyText(outputText, clipboardWatcher);
                        NativeMethods.SendMessage(hWnd, WM.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
                    } finally {
                        if(clipboardData.Type != ClipboardType.None) {
                            ClipboardUtility.CopyClipboardItem(clipboardData, clipboardWatcher);
                        } else {
                            Clipboard.Clear();
                        }
                    }
                } else {
                    nonProcess.Logger.Error(nonProcess.Language["log/clipboard/using-error"]);
                }
            } else {
                SendKeysUtility.Send(outputText);
            }
        }

        /// <summary>
        /// 指定ウィンドウハンドルの次のウィンドウハンドルに文字列を転送する。
        /// </summary>
        /// <param name="hBaseWnd"></param>
        /// <param name="outputText"></param>
        /// <param name="nonProcess"></param>
        /// <param name="clipboardWatcher"></param>
        public static void OutputTextForNextWindow(IntPtr hBaseWnd, string outputText, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
        {

            var windowHandles = new List<IntPtr>();
            var hWnd = hBaseWnd;
            do {
                hWnd = NativeMethods.GetWindow(hWnd, GW.GW_HWNDNEXT);
                windowHandles.Add(hWnd);
            } while(!NativeMethods.IsWindowVisible(hWnd));

            try {
                OutputTextForWindowHandle(hWnd, outputText, nonProcess, clipboardWatcher);
            } catch(Exception ex) {
                nonProcess.Logger.Error(ex);
            }
        }

        private static IEnumerable<ClipboardType> GetEnabledClipboardTypeList(ClipboardType types, IEnumerable<ClipboardType> list)
        {
            return list.Where(t => types.HasFlag(t));
        }

        /// <summary>
        /// このアイテムが保持する有効なデータ種別を列挙する。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ClipboardType> GetClipboardTypeList(ClipboardType types)
        {
            Debug.Assert(types != ClipboardType.None);

            var list = new[] {
                ClipboardType.Text,
                ClipboardType.Rtf,
                ClipboardType.Html,
                ClipboardType.Image,
                ClipboardType.Files,
            };

            return GetEnabledClipboardTypeList(types, list);
        }

        public static ClipboardType GetSingleClipboardType(ClipboardType types)
        {
            var list = new[] {
                ClipboardType.Html,
                ClipboardType.Rtf,
                ClipboardType.Files,
                ClipboardType.Text,
                ClipboardType.Image,
            };

            return GetEnabledClipboardTypeList(types, list).First();
        }

        /// <summary>
        /// 同一設定でうまいことデータを取得する。
        /// </summary>
        /// <param name="captureType"></param>
        /// <param name="hWnd"></param>
        /// <param name="nonProcess"></param>
        /// <returns></returns>
        public static ClipboardData GetClipboardDataDefault(ClipboardType captureType, IntPtr hWnd, INonProcess nonProcess)
        {
            try {
                var exceptions = new List<Exception>();
                var retry = new TimeRetry<ClipboardData>() {
                    WaitTime = Constants.clipboardGetDataRetryWaitTime,
                    WaitMaxCount = Constants.clipboardGetDataRetryMaxCount,
                    ExecuteFunc = (int waitCurrentCount, ref ClipboardData result) => {
                        ClipboardData data = null;
                        try {
                            data = ClipboardUtility.GetClipboardData(captureType, hWnd, nonProcess);
                        } catch(Exception ex) {
                            exceptions.Add(ex);
                        }
                        var hasData = data != null;
                        if(hasData) {
                            result = data;
                        }
                        return hasData;
                    }
                };
                retry.Run();

                if(!retry.WaitOver) {
                    return retry.Result;
                } else if(exceptions.Any()) {
                    if(exceptions.Count == 1) {
                        nonProcess.Logger.Error(exceptions.First());
                    } else {
                        nonProcess.Logger.Error(
                            nonProcess.Language["log/clipboard/get-error"],
                            string.Join(Environment.NewLine, exceptions.Select(ex => ex.ToString()))
                        );
                    }
                }
            } catch(AccessViolationException ex) {
                // #251
                nonProcess.Logger.Error(ex);
            }

            return null;
        }


        public static string MakeClipboardNameFromText(string text)
        {
            var result = text
                .SplitLines()
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .FirstOrDefault()
            ;

            return result;
        }

        public static string MakeClipboardNameFromRtf(string rtf)
        {
            var rt = new RichTextBox();
            string plainText;
            using(var reader = new MemoryStream(Encoding.ASCII.GetBytes(rtf))) {
                rt.Selection.Load(reader, DataFormats.Rtf);
                using(var writer = new MemoryStream()) {
                    rt.Selection.Save(writer, DataFormats.Text);
                    plainText = Encoding.UTF8.GetString(writer.ToArray());
                }
            }

            var result = MakeClipboardNameFromText(plainText);

            return result;
        }

        public static string MakeClipboardNameFromHtml(string html, INonProcess nonProcess)
        {
            var takeCount = 64;
            var converted = false;
            var lines = Regex.Replace(html, @"<!--.*?-->", string.Empty, RegexOptions.Multiline)
                .SplitLines()
                .Take(takeCount)
            ;
            var text = string.Join(string.Empty, lines);

            var timeTitle = TimeSpan.FromMilliseconds(100);
            var timeHeading = TimeSpan.FromMilliseconds(500);

            // <title>
            try {
                var regTitle = new Regex(
                    @"
                    <title>
                        (?<TITLE>.+)
                    </title>
                    ",
                    RegexOptions.IgnoreCase 
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.ExplicitCapture
                    ,
                    timeTitle
                );
                var matchTitle = regTitle.Match(text);
                if(!converted && matchTitle.Success && 0 < matchTitle.Groups.Count) {
                    text = matchTitle.Groups["TITLE"].Value.Trim();
                    converted = true;
                }
            } catch(RegexMatchTimeoutException ex) {
                //logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage("<title>", ex));
                nonProcess.Logger.Warning(ex);
            }

            if(!converted) {
                // <h1-6>
                try {
                    // TODO: 終了タグが一致しない
                    var regHeader = new Regex(
                        @"
                        <h[1-6](.*)?>
                            (?<HEADING>.+?)
                        </h[1-6]>
                        ",
                        RegexOptions.IgnoreCase
                        | RegexOptions.Multiline
                        | RegexOptions.IgnorePatternWhitespace
                        | RegexOptions.ExplicitCapture
                        ,
                        timeHeading
                   );
                    var matchHeading = regHeader.Match(text);
                    if(matchHeading.Success && 0 < matchHeading.Groups.Count) {
                        text = matchHeading.Groups["HEADING"].Value.Trim();
                        Debug.WriteLine(text);
                        converted = true;
                    }
                } catch(RegexMatchTimeoutException ex) {
                    //logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage("<header>", ex));
                    nonProcess.Logger.Warning(ex);
                }
            }

            // まだ何かタグに囲まれている場合はそれを除外
            if(converted && text != null) {
                var regPlain = new Regex(
                    @"
                        (<.+>)?
                            (?<TEXT>.+)
                        (</.+>)?
                        ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.ExplicitCapture
               );
                var matchPlain = regPlain.Match(text);
                if(matchPlain.Success && 0 < matchPlain.Groups.Count) {
                    text = matchPlain.Groups["TEXT"].Value.Trim();
                }

                // 文字参照をテキスト化
                text = System.Net.WebUtility.HtmlDecode(text);
            }

            return converted ? text : string.Empty;
        }
    }
}
