using System;
using System.IO;

public class UpdaterScript
{
	void RemoveFiles(string baseDirectoryPath, string platform)
	{
		var targets = new[] {
			""
		};
	}

	public void Main(string[] args)
	{
		var prevFore = Console.ForegroundColor;
		var prevBack = Console.BackgroundColor;
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.BackgroundColor = ConsoleColor.Black;

		Console.WriteLine("<<UpdaterScript: START>>");
		try {
			if(args.Length != 3) {
				throw new ArgumentException();
			}
			var scriptFilePath = args[0];
			var baseDirectoryPath = args[1];
			var platform = args[2];
			Console.WriteLine("S: {0}", scriptFilePath);
			Console.WriteLine("D: {0}", baseDirectoryPath);
			Console.WriteLine("P: {0}", platform);
			
			RemoveFiles(baseDirectoryPath, platform);

		} catch(Exception ex) {
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("<<UpdaterScript: ERROR>>");
			Console.WriteLine(ex);
		}

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("<<UpdaterScript: END>>");
		Console.ForegroundColor = prevFore;
		Console.BackgroundColor = prevBack;
	}
}
