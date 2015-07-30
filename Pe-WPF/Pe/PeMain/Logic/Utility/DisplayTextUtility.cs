namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
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
			where T: IComparable
		{
			if (string.IsNullOrWhiteSpace(name.Name)) {
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

		public static string MakeClipboardName(ClipboardItem clipboardItem, INonProcess nonProcess) 
		{

			var type = ClipboardUtility.GetSingleClipboardType(clipboardItem.Type);
			Debug.Assert(type != ClipboardType.None);

			string result;

			switch (type) {
				case ClipboardType.Text: {
						var text = clipboardItem.Body.Text
							.SplitLines()
							.Where(s => !string.IsNullOrWhiteSpace(s))
							.Select(s => s.Trim())
							.FirstOrDefault()
						;

						if (string.IsNullOrWhiteSpace(text)) {
							// TODO: i18n
							result = type.ToString();
						} else {
							result = text;
						}
					}
					break;

				case ClipboardType.Rtf: {
						var rt = new RichTextBox();
							string plainText;
							using(var reader = new MemoryStream(ASCIIEncoding.Default.GetBytes(clipboardItem.Body.Rtf))) {
								rt.Selection.Load(reader, DataFormats.Rtf);
								using(var writer = new MemoryStream()) {
									rt.Selection.Save(writer, DataFormats.Text);
									plainText = ASCIIEncoding.Default.GetString(writer.ToArray());
								}
							}

							var text = plainText
								.SplitLines()
								.Where(s => !string.IsNullOrWhiteSpace(s))
								.Select(s => s.Trim())
								.FirstOrDefault()
							;

							if (string.IsNullOrWhiteSpace(text)) {
								// TODO: i18n
								result = type.ToString();
							} else {
								result = text;
							}
						}
					
					break;

				case ClipboardType.Html: {
						var takeCount = 64;
						var converted = false;
						var lines = clipboardItem.Body.Html.SplitLines().Take(takeCount);
						var text = string.Join("", lines);
						//var text = clipboardItem.Html.Replace('\r', ' ').Replace('\n', ' ');

						var timeTitle = TimeSpan.FromMilliseconds(500);
						var timeHeader = TimeSpan.FromMilliseconds(500);

						// タイトル
						try {
							var regTitle = new Regex("<title>(.+)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline, timeTitle);
							var matchTitle = regTitle.Match(text);
							if (!converted && matchTitle.Success && matchTitle.Groups.Count > 1) {
								text = matchTitle.Groups[1].Value.Trim();
								converted = true;
							}
						} catch (RegexMatchTimeoutException ex) {
							//logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage("<title>", ex));
							nonProcess.Logger.Warning(ex);
						}

						// h1
						try {
							var regHeader = new Regex("<h1(?:.*)?>(.+)</h1>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
							var matchHeader = regHeader.Match(text);
							if (!converted && matchHeader.Success && matchHeader.Groups.Count > 1) {
								text = matchHeader.Groups[1].Value.Trim();
								Debug.WriteLine(text);
								converted = true;
							}
						} catch (RegexMatchTimeoutException ex) {
							//logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage("<header>", ex));
							nonProcess.Logger.Warning(ex);
						}

						if (!converted || string.IsNullOrWhiteSpace(text)) {
							// TODO: i18n
							result = type.ToString();
						} else {
							result = text;
						}
					}
					break;

				case ClipboardType.Image: {
						//var map = new Dictionary<string, string>() {
						//	{ ProgramLanguageName.imageType, ClipboardTypeToDisplayText(language, type) },
						//	{ ProgramLanguageName.imageWidth, clipboardItem.Image.Width.ToString() },
						//	{ ProgramLanguageName.imageHeight, clipboardItem.Image.Height.ToString() },
						//};

						//result = language["clipboard/title/image", map];
					// TODO: i18n
					result= string.Format("{0} x {1}", clipboardItem.Body.Image.PixelWidth, clipboardItem.Body.Image.PixelHeight);
					
					}
					break;

				case ClipboardType.File: {
						//var map = new Dictionary<string, string>() {
						//	{ ProgramLanguageName.fileType, ClipboardTypeToDisplayText(language, type) },
						//	{ ProgramLanguageName.fileCount, clipboardItem.Files.Count().ToString() },
						//};

						//result = language["clipboard/title/file", map];

						// TODO: i18n
						result = string.Format("{0}", clipboardItem.Body.Files.Count);
					}
					break;

				default:
					throw new NotImplementedException();
			}

			return result.SplitLines().FirstOrDefault();
		}
	}
}
