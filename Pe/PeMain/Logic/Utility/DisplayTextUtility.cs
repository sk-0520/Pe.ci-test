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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class DisplayTextUtility
    {
        ///// <summary>
        ///// 文字列IDとINameを保持したデータから表示文字列を取得。
        ///// </summary>
        ///// <typeparam name="TModel"></typeparam>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static string GetDisplayName<TModel,T>(TModel model)
        //	where TModel: ITId<T>, IName
        //	where T: IComparable
        //{
        //	return GetDisplayName(model, model);
        //}

        public static string GetDisplayName<T>(ITId<T> id, IName name)
            where T : IComparable
        {
            if(string.IsNullOrWhiteSpace(name.Name)) {
                return id.Id.ToString();
            }

            return name.Name ?? string.Empty;
        }

        //public static string GetDisplayName<T>(ITId<T> id)
        //	where T: IComparable
        //{
        //	var name = id as IName;
        //	if(name != null) {
        //		return GetDisplayName(id, name);
        //	}

        //	return id.Id == null ? id.Id.ToString(): string.Empty;
        //}

        public static string GetDisplayName(IName name)
        {
            return name.Name ?? string.Empty;
        }

        public static string MakeClipboardName(ClipboardData clipboardData, INonProcess nonProcess)
        {

            var type = ClipboardUtility.GetSingleClipboardType(clipboardData.Type);
            Debug.Assert(type != ClipboardType.None);

            string result;

            switch(type) {
                case ClipboardType.Text:
                    {
                        var text = ClipboardUtility.MakeClipboardNameFromText(clipboardData.Body.Text);

                        if(string.IsNullOrWhiteSpace(text)) {
                            result = LanguageUtility.GetTextFromEnum(type, nonProcess.Language);
                        } else {
                            result = text;
                        }
                    }
                    break;

                case ClipboardType.Rtf:
                    {
                        var text = ClipboardUtility.MakeClipboardNameFromRtf(clipboardData.Body.Rtf);

                        if(string.IsNullOrWhiteSpace(text)) {
                            result = LanguageUtility.GetTextFromEnum(type, nonProcess.Language);
                        } else {
                            result = text;
                        }
                    }

                    break;

                case ClipboardType.Html: 
                    {
                        var text = ClipboardUtility.MakeClipboardNameFromHtml(clipboardData.Body.Html, nonProcess);

                        if(string.IsNullOrWhiteSpace(text) && clipboardData.Type.HasFlag(ClipboardType.Rtf)) {
                            text = ClipboardUtility.MakeClipboardNameFromRtf(clipboardData.Body.Rtf);
                        }
                        if(string.IsNullOrWhiteSpace(text) && clipboardData.Type.HasFlag(ClipboardType.Text)) {
                            text = ClipboardUtility.MakeClipboardNameFromRtf(clipboardData.Body.Text);
                        }

                        if(string.IsNullOrWhiteSpace(text)) {
                            result = LanguageUtility.GetTextFromEnum(type, nonProcess.Language);
                        } else {
                            result = text;
                        }
                    }
                    break;

                case ClipboardType.Image:
                    {
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.clipboardType, LanguageUtility.GetTextFromEnum(type, nonProcess.Language) },
                            { LanguageKey.clipboardImageWidth, clipboardData.Body.Image.PixelWidth.ToString() },
                            { LanguageKey.clipboardImageHeight, clipboardData.Body.Image.PixelHeight.ToString() },
                        };

                        result = nonProcess.Language["clipboard/name/image", map];
                    }
                    break;

                case ClipboardType.Files:
                    {
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.clipboardType, LanguageUtility.GetTextFromEnum(type, nonProcess.Language) },
                            { LanguageKey.clipboardFileCount, clipboardData.Body.Files.Count().ToString() },
                        };

                        result = nonProcess.Language["clipboard/name/files", map];
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            return result.SplitLines().FirstOrDefault();
        }
    }
}
