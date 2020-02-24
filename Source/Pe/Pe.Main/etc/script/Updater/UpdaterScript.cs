/*-*/using System;
/*-*/using System.IO;
/*-*/using System.Data;
/*-*/using System.Linq;

public class UpdaterScript
{
    void ChangeTemporaryColor(string s, ConsoleColor fore, ConsoleColor back)
    {
        var tempFore = Console.ForegroundColor;
        var tempBack = Console.BackgroundColor;

        Console.ForegroundColor = fore;
        Console.BackgroundColor = back;
        Console.WriteLine(s);

        Console.ForegroundColor = tempFore;
        Console.BackgroundColor = tempBack;
    }

    void RemoveFiles(string baseDirectoryPath, string platform)
    {
        var platformDir = platform + @"\";
        var notPlatformDir = string.Compare(platform, "x86", true) == 0 ? @"x64\" : @"x86\";
        var targets = new[] {
            @"bat\log.bat",
            @"bat\accept.bat",
            @"lib\",
            @"etc\style\",
            @"etc\script\autosize.js",
            @"etc\script\changelog.js",
            @"etc\script\changelog-core.js",
            @"etc\script\jquery.js",
            @"etc\lang\",
            @"etc\image\",
            @"etc\html\",
            @"doc\components.xml",
            @"doc\model-changelogs.js",
            @"PeMain.exe.config",
        };
        var tagetPathList = targets.Select(s => Path.Combine(baseDirectoryPath, s));
        foreach(var targetPath in tagetPathList) {
            var found = false;
            var isDir = targetPath.Last() == Path.DirectorySeparatorChar;
            var fileOrDir = isDir ? 'D' : 'F';
            try {
                if(isDir) {
                    if(Directory.Exists(targetPath)) {
                        found = true;
                        Directory.Delete(targetPath, true);
                    }
                } else if(File.Exists(targetPath)) {
                    found = true;
                    File.Delete(targetPath);
                }
                Console.WriteLine("DEL:{0}: {1}, FOUND = {2}", fileOrDir, targetPath, found);
            } catch(Exception ex) {
                Console.WriteLine("ERR:{0}: {1}, {2}", fileOrDir, targetPath, ex);
            }
        }
    }
    public void Main(string[] args)
    {
        var prevFore = Console.ForegroundColor;
        var prevBack = Console.BackgroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.BackgroundColor = ConsoleColor.Black;

        Console.WriteLine("<UpdaterScript: START>");
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

            Console.WriteLine("0.83.0 互換性 スクリプト");
            RemoveFiles(baseDirectoryPath, platform);

        } catch(Exception ex) {
            ChangeTemporaryColor("<UpdaterScript: ERROR>", ConsoleColor.Magenta, prevBack);
            Console.WriteLine(ex);
        }

        Console.WriteLine("<UpdaterScript: END>");
        Console.ForegroundColor = prevFore;
        Console.BackgroundColor = prevBack;
    }
}
