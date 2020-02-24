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

        } catch(Exception ex) {
            ChangeTemporaryColor("<UpdaterScript: ERROR>", ConsoleColor.Magenta, prevBack);
            Console.WriteLine(ex);
        }

        Console.WriteLine("<UpdaterScript: END>");
        Console.ForegroundColor = prevFore;
        Console.BackgroundColor = prevBack;
    }
}
