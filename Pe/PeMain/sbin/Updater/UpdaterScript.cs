using System;
using System.IO;

class UpdaterScript
{
	void RemoveFiles(string baseDirectoryPath)
	{
		var targets = new[] {
			""
		};
	}

	public void Main(string[] args)
	{
		Console.WriteLine("<<UpdaterScript: START>>");
		try {
			if(args.Length != 2) {
				throw new ArgumentException();
			}
			var scriptFilePath = args[0];
			var baseDirectoryPath = args[1];
			Console.WriteLine("S: {0}", scriptFilePath);
			Console.WriteLine("D: {0}", baseDirectoryPath);

		} catch(Exception ex) {
			Console.WriteLine(ex);
		}
		Console.WriteLine("<<UpdaterScript: END>>");
	}
}
