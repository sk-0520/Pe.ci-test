using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao
{
    [TestClass]
    public class DaoTest
    {
        #region function

        /// <summary>
        /// SQL ファイルと Dao の各種クラス・メソッドが対になっているか調査: #616
        /// <para>ファイルからメソッドを見つける形で対応する。</para>
        /// <para>現時点では Pe.Main.* のみが対象となる制限付き。</para>
        /// </summary>
        [TestMethod]
        public void SqlFileTest()
        {
            static string TrimDirectory(string inputPath, string rootDirectoryPath)
            {
                var value = inputPath.AsSpan().Slice(rootDirectoryPath.Length);
                while(true) {
                    if(value[0] == Path.DirectorySeparatorChar) {
                        value = value.Slice(1);
                    } else if(value[0] == Path.AltDirectorySeparatorChar) {
                        value = value.Slice(1);
                    } else {
                        break;
                    }
                }
                while(true) {
                    if(value[^1] == Path.DirectorySeparatorChar) {
                        value = value.Slice(0, value.Length - 1);
                    } else if(value[^1] == Path.AltDirectorySeparatorChar) {
                        value = value.Slice(0, value.Length - 1);
                    } else {
                        break;
                    }
                }

                return value.ToString().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            var errorMessages = new List<string>();
            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var sqlRootDirPath = Path.Combine(Path.GetDirectoryName(testAssemblyPath)!, "etc", "sql");
            var sqlFileGroups = Directory.EnumerateFiles(sqlRootDirPath, "*.sql", SearchOption.AllDirectories)
                .GroupBy(i => Path.GetDirectoryName(i)!, i => i)
            ;
            var asm = Assembly.GetAssembly(typeof(ContentTypeTextNet.Pe.Main.App))!;
            foreach(var sqlFileGroup in sqlFileGroups) {
                var classFullNameDirPath = TrimDirectory(sqlFileGroup.Key, sqlRootDirPath);
                var classFullName = classFullNameDirPath.Replace(Path.DirectorySeparatorChar, '.');
                var className = Path.GetFileName(classFullNameDirPath);

                // ファイルを正としてクラスに存在するか確認
                if(sqlFileGroup.Any()) {
                    var classType = asm.GetType(classFullName, false, false);
                    if(classType == null) {
                        errorMessages.Add($"not found class: {classFullName}");
                        continue;
                    }

                    var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(i => i.Name)
                        .ToHashSet()
                    ;
                    foreach(var sqlFileName in sqlFileGroup) {
                        var sqlMethodName = Path.GetFileNameWithoutExtension(sqlFileName);
                        if(!methods.Contains(sqlMethodName)) {
                            errorMessages.Add($"not found method: {classFullName}.{sqlMethodName}");
                        }
                    }
                }
            }

            Assert.IsTrue(errorMessages.Count == 0, string.Join(Environment.NewLine, errorMessages));
        }

        #endregion
    }
}
