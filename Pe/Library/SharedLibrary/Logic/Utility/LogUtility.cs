namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using System.Reflection;

	public static class LogUtility
	{
		#region define

		const string indent = "    ";
		const string messageFormat = " [MSG] {0}";
		const string detailPadding = "\t";

		#endregion

		static string ToShowText(MethodBase method)
		{
			var parameters = string.Join(
				", ",
				method.GetParameters()
					.OrderBy(p => p.Position)
					.Select(p => p.ToString())
			);

			return method.ReflectedType.Name + "." + method.Name + "(" + parameters + ")";
		}

		public static string MakeLogDetailText(LogItemModel item)
		{
			var header = string.Format(
				"{0}[{1}] {2} <{3}({4})> , Thread: {5}/{6}, Assembly: {7}",
				item.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
				item.LogKind.ToString().ToUpper().Substring(0, 1),
				item.CallerMember,
				item.CallerFile,
				item.CallerLine,
				item.CallerThread.ManagedThreadId,
				item.CallerThread.ThreadState,
				item.CallerAssembly.GetName()
			);
			var message = string.Format(messageFormat, item.Message);
			var detail = item.DetailText;
			if (!string.IsNullOrEmpty(detail)) {
				var detailIndent = new string(' ', message.Length);
				var lines = detail.SplitLines();
				var nexts = lines.Skip(1).Select(s => detailIndent + detailPadding + s);
				if (nexts.Any()) {
					var first = detailPadding + lines.First();
					detail = first + Environment.NewLine + string.Join(Environment.NewLine, nexts);
				} else {
					detail = detailPadding + detail;
				}
			}
			var stack = string.Join(
				Environment.NewLine,
				item.StackTrace.GetFrames()
					.Select(sf => string.Format(
						indent + "-[{0:x8}][{1:x8}] {2}[{3}]",
						sf.GetNativeOffset(),
						sf.GetILOffset(),
						ToShowText(sf.GetMethod()),
						sf.GetFileLineNumber()
					)
				)
			);

			var result
				= header
				+ Environment.NewLine
				+ message
				+ detail
				+ Environment.NewLine
				+ " [STK]"
				+ Environment.NewLine
				+ indent + "+[ Native ][   IL   ] Method[line]"
				+ Environment.NewLine
				+ stack
				+ Environment.NewLine
			;
			return result;
		}
	}
}
