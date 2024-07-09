using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace ContentTypeTextNet.Pe.CommonTest
{
    /// <summary>
    /// IO系のテスト系インフラ。
    /// </summary>
    public static class TestIO
    {
        #region property

        private static string RootDirectoryName { get; } = "_test_io_";

        private static bool InitializedProjectDirectory { get; set; }
        private static HashSet<string> InitializedClassDirectory { get; set; } = [];
        private static HashSet<string> InitializedMethodDirectory { get; set; } = [];

        #endregion

        #region function

        private static DirectoryInfo GetProjectDirectory()
        {
            var projectTestPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var projectTestDirPath = Path.Combine(projectTestPath, RootDirectoryName);

            return new DirectoryInfo(projectTestDirPath);
        }

        private static DirectoryInfo GetClassDirectory(object test)
        {
            var project = GetProjectDirectory();
            var classTestDirPath = Path.Combine(project.FullName, test.GetType().FullName ?? throw new Exception(test.ToString()));

            return new DirectoryInfo(classTestDirPath);
        }

        private static DirectoryInfo GetMethodDirectory(object test, string callerMemberName, int callerLineNumber)
        {
            var classTestDirPath = GetClassDirectory(test);
            var methodTestDirPath = Path.Combine(classTestDirPath.FullName, callerMemberName + "-L" + callerLineNumber.ToString(CultureInfo.InvariantCulture));

            return new DirectoryInfo(methodTestDirPath);
        }

        private static DirectoryInfo CreateDirectory(DirectoryInfo dir)
        {
            dir.Refresh();
            if(dir.Exists) {
                dir.Delete(true);
            }
            dir.Create();

            return dir;
        }

        /// <summary>
        /// テストプロジェクト用テスト初期化。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DirectoryInfo InitializeProject()
        {
            if(InitializedProjectDirectory) {
                throw new TestException();
            }

            var dir = GetProjectDirectory();
            var result = CreateDirectory(dir);

            InitializedProjectDirectory = true;

            return result;
        }

        /// <summary>
        /// テストクラス用テスト初期化。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static DirectoryInfo InitializeClass(Type type)
        {
            var dir = GetClassDirectory(type);

            if(InitializedClassDirectory.Contains(dir.FullName)) {
                throw new TestException();
            }

            var result = CreateDirectory(dir);

            InitializedClassDirectory.Add(dir.FullName);

            return result;
        }

        /// <summary>
        /// テストメソッド用テスト初期化。
        /// </summary>
        /// <param name="test"></param>
        /// <param name="callerMemberName"></param>
        /// <returns></returns>
        public static DirectoryInfo InitializeMethod(object test, string suffix = "", [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            var dir = GetMethodDirectory(test, callerMemberName, callerLineNumber);
            if(!string.IsNullOrWhiteSpace(suffix)) {
                dir = new DirectoryInfo(dir.FullName + "@" + suffix);
            }

            if(InitializedMethodDirectory.Contains(dir.FullName)) {
                throw new TestException();
            }

            var result = CreateDirectory(dir);

            InitializedMethodDirectory.Add(dir.FullName);

            return result;
        }

        public static DirectoryInfo CreateDirectory(DirectoryInfo directory, string name)
        {
            var dirPath = Path.Combine(directory.FullName, name);
            return Directory.CreateDirectory(dirPath);
        }

        public static FileInfo CreateEmptyFile(DirectoryInfo directory, string name)
        {
            var filePath = Path.Combine(directory.FullName, name);
            File.Create(filePath).Dispose();
            return new FileInfo(filePath);
        }

        public static FileInfo CreateTextFile(DirectoryInfo directory, string name, string content, Encoding encoding)
        {
            var filePath = Path.Combine(directory.FullName, name);

            using(var stream = File.Create(filePath)) {
                using var writer = new StreamWriter(stream, encoding);
                writer.Write(content);
            }

            return new FileInfo(filePath);
        }
        public static FileInfo CreateTextFile(DirectoryInfo directory, string name, string content) => CreateTextFile(directory, name, content, Encoding.UTF8);

        #endregion
    }
}
