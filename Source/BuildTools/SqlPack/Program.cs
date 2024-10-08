using System;
using System.IO;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging.Abstractions;

namespace SqlPack
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = new CommandLine();
            var sqlRootDirKey = commandLine.Add(longKey: "sql-root-dir", hasValue: true);
            var outputPathKey = commandLine.Add(longKey: "output", hasValue: true);
            if(!commandLine.Parse()) {
                throw commandLine.ParseException!;
            }

            var rootDirPath = commandLine.Values[sqlRootDirKey].First;
            var outPath = commandLine.Values[outputPathKey].First;

            var executor = new Executor(rootDirPath, outPath);
            executor.Run();
        }
    }
}
