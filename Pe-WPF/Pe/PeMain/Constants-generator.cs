namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	partial class Constants
	{
		/*
		このソースは自動生成のため Constants-generator.tt を編集すること。

		生成元ConstantsRangeフィールド数: 15
		*/

		// TripleRange
		#region streamFontSize

		/// <summary>
		/// Constants.streamFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double StreamFontMinimumSize
		{
			get
			{
				return streamFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.streamFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double StreamFontMedianSize
		{
			get
			{
				return streamFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.streamFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double StreamFontMaximumSize
		{
			get
			{
				return streamFontSize.maximum;
			}
		}
		

		#endregion
		#region commandHideTime

		/// <summary>
		/// Constants.commandHideTime.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan CommandHideMinimumTime
		{
			get
			{
				return commandHideTime.minimum;
			}
		}
		

		/// <summary>
		/// Constants.commandHideTime.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan CommandHideMedianTime
		{
			get
			{
				return commandHideTime.median;
			}
		}
		

		/// <summary>
		/// Constants.commandHideTime.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan CommandHideMaximumTime
		{
			get
			{
				return commandHideTime.maximum;
			}
		}
		

		#endregion
		#region commandWindowWidth

		/// <summary>
		/// Constants.commandWindowWidth.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandWindowMinimumWidth
		{
			get
			{
				return commandWindowWidth.minimum;
			}
		}
		

		/// <summary>
		/// Constants.commandWindowWidth.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandWindowMedianWidth
		{
			get
			{
				return commandWindowWidth.median;
			}
		}
		

		/// <summary>
		/// Constants.commandWindowWidth.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandWindowMaximumWidth
		{
			get
			{
				return commandWindowWidth.maximum;
			}
		}
		

		#endregion
		#region commandFontSize

		/// <summary>
		/// Constants.commandFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandFontMinimumSize
		{
			get
			{
				return commandFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.commandFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandFontMedianSize
		{
			get
			{
				return commandFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.commandFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double CommandFontMaximumSize
		{
			get
			{
				return commandFontSize.maximum;
			}
		}
		

		#endregion
		#region toolbarTextLength

		/// <summary>
		/// Constants.toolbarTextLength.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarTextMinimumLength
		{
			get
			{
				return toolbarTextLength.minimum;
			}
		}
		

		/// <summary>
		/// Constants.toolbarTextLength.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarTextMedianLength
		{
			get
			{
				return toolbarTextLength.median;
			}
		}
		

		/// <summary>
		/// Constants.toolbarTextLength.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarTextMaximumLength
		{
			get
			{
				return toolbarTextLength.maximum;
			}
		}
		

		#endregion
		#region toolbarFontSize

		/// <summary>
		/// Constants.toolbarFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarFontMinimumSize
		{
			get
			{
				return toolbarFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.toolbarFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarFontMedianSize
		{
			get
			{
				return toolbarFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.toolbarFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ToolbarFontMaximumSize
		{
			get
			{
				return toolbarFontSize.maximum;
			}
		}
		

		#endregion
		#region toolbarHideWaitTime

		/// <summary>
		/// Constants.toolbarHideWaitTime.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideWaitMinimumTime
		{
			get
			{
				return toolbarHideWaitTime.minimum;
			}
		}
		

		/// <summary>
		/// Constants.toolbarHideWaitTime.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideWaitMedianTime
		{
			get
			{
				return toolbarHideWaitTime.median;
			}
		}
		

		/// <summary>
		/// Constants.toolbarHideWaitTime.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideWaitMaximumTime
		{
			get
			{
				return toolbarHideWaitTime.maximum;
			}
		}
		

		#endregion
		#region toolbarHideAnimateTime

		/// <summary>
		/// Constants.toolbarHideAnimateTime.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideAnimateMinimumTime
		{
			get
			{
				return toolbarHideAnimateTime.minimum;
			}
		}
		

		/// <summary>
		/// Constants.toolbarHideAnimateTime.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideAnimateMedianTime
		{
			get
			{
				return toolbarHideAnimateTime.median;
			}
		}
		

		/// <summary>
		/// Constants.toolbarHideAnimateTime.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ToolbarHideAnimateMaximumTime
		{
			get
			{
				return toolbarHideAnimateTime.maximum;
			}
		}
		

		#endregion
		#region clipboardWaitTime

		/// <summary>
		/// Constants.clipboardWaitTime.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ClipboardWaitMinimumTime
		{
			get
			{
				return clipboardWaitTime.minimum;
			}
		}
		

		/// <summary>
		/// Constants.clipboardWaitTime.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ClipboardWaitMedianTime
		{
			get
			{
				return clipboardWaitTime.median;
			}
		}
		

		/// <summary>
		/// Constants.clipboardWaitTime.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan ClipboardWaitMaximumTime
		{
			get
			{
				return clipboardWaitTime.maximum;
			}
		}
		

		#endregion
		#region clipboardSaveCount

		/// <summary>
		/// Constants.clipboardSaveCount.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardSaveMinimumCount
		{
			get
			{
				return clipboardSaveCount.minimum;
			}
		}
		

		/// <summary>
		/// Constants.clipboardSaveCount.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardSaveMedianCount
		{
			get
			{
				return clipboardSaveCount.median;
			}
		}
		

		/// <summary>
		/// Constants.clipboardSaveCount.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardSaveMaximumCount
		{
			get
			{
				return clipboardSaveCount.maximum;
			}
		}
		

		#endregion
		#region clipboardDuplicationCount

		/// <summary>
		/// Constants.clipboardDuplicationCount.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardDuplicationMinimumCount
		{
			get
			{
				return clipboardDuplicationCount.minimum;
			}
		}
		

		/// <summary>
		/// Constants.clipboardDuplicationCount.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardDuplicationMedianCount
		{
			get
			{
				return clipboardDuplicationCount.median;
			}
		}
		

		/// <summary>
		/// Constants.clipboardDuplicationCount.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Int32 ClipboardDuplicationMaximumCount
		{
			get
			{
				return clipboardDuplicationCount.maximum;
			}
		}
		

		#endregion
		#region clipboardFontSize

		/// <summary>
		/// Constants.clipboardFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ClipboardFontMinimumSize
		{
			get
			{
				return clipboardFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.clipboardFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ClipboardFontMedianSize
		{
			get
			{
				return clipboardFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.clipboardFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double ClipboardFontMaximumSize
		{
			get
			{
				return clipboardFontSize.maximum;
			}
		}
		

		#endregion
		#region templateFontSize

		/// <summary>
		/// Constants.templateFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double TemplateFontMinimumSize
		{
			get
			{
				return templateFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.templateFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double TemplateFontMedianSize
		{
			get
			{
				return templateFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.templateFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double TemplateFontMaximumSize
		{
			get
			{
				return templateFontSize.maximum;
			}
		}
		

		#endregion
		#region windowSaveIntervalTime

		/// <summary>
		/// Constants.windowSaveIntervalTime.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan WindowSaveIntervalMinimumTime
		{
			get
			{
				return windowSaveIntervalTime.minimum;
			}
		}
		

		/// <summary>
		/// Constants.windowSaveIntervalTime.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan WindowSaveIntervalMedianTime
		{
			get
			{
				return windowSaveIntervalTime.median;
			}
		}
		

		/// <summary>
		/// Constants.windowSaveIntervalTime.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static TimeSpan WindowSaveIntervalMaximumTime
		{
			get
			{
				return windowSaveIntervalTime.maximum;
			}
		}
		

		#endregion
		#region noteFontSize

		/// <summary>
		/// Constants.noteFontSize.minimum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double NoteFontMinimumSize
		{
			get
			{
				return noteFontSize.minimum;
			}
		}
		

		/// <summary>
		/// Constants.noteFontSize.median取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double NoteFontMedianSize
		{
			get
			{
				return noteFontSize.median;
			}
		}
		

		/// <summary>
		/// Constants.noteFontSize.maximum取得用プロパティ。
		/// <para>XAMLで使用することを想定</para>
		/// </summary>
		public static Double NoteFontMaximumSize
		{
			get
			{
				return noteFontSize.maximum;
			}
		}
		

		#endregion

	}
}
