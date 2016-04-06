/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Event;
using Microsoft.VisualStudio.TextTemplating;

namespace ContentTypeTextNet.Pe.PeMain.Logic.T4Template
{
    /// <summary>
    /// <para>http://d.hatena.ne.jp/seraphy/20140419</para>
    /// </summary>
    [Serializable]
    public class TextTemplatingEngineHost: ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {
        public event EventHandler<TextTemplatingErrorEventArgs> Error = delegate { };

        /// <summary>
        /// 現在処理中のテンプレートファイル
        /// </summary>
        public string TemplateFile { set; get; }

        /// <summary>
        /// T4エンジンがコードを生成する先のAppDomain
        /// </summary>
        public AppDomain AppDomain { set; get; }

        public ITextTemplatingSession CreateSession()
        {
            return Session;
        }

        public ITextTemplatingSession Session { get; set; }

        /// <summary>
        /// オプションを処理する.
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        public object GetHostOption(string optionName)
        {
            object returnObject;
            switch(optionName) {
                case "CacheAssemblies":
                    returnObject = true;
                    break;
                default:
                    returnObject = null;
                    break;
            }
            return returnObject;
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            // フルパスで実在すれば、そのまま返す.
            if(File.Exists(assemblyReference)) {
                return assemblyReference;
            }

            // "System.Core"のようなアセンブリ参照が来たとき、
            // 標準アセンブリの位置からの相対パスでdllが実在するか判断する.
            foreach(string sysloc in StandardAssemblyReferences) {
                string dir = Path.GetDirectoryName(sysloc);
                string candidate = Path.Combine(dir, assemblyReference + ".dll");
                if(File.Exists(candidate)) {
                    return candidate;
                }
            }

            // このクラスがあるアセンブリの場所からの相対位置で判断する.
            {
                string dir = Path.GetDirectoryName(this.GetType().Assembly.Location);
                string candidate = Path.Combine(dir, assemblyReference + ".dll");
                if(File.Exists(candidate)) {
                    return candidate;
                }
            }

            // 不明
            return "";
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new Exception("Directive Processor not found");
        }

        public string ResolvePath(string fileName)
        {
            if(fileName == null) {
                throw new ArgumentNullException("the file name cannot be null");
            }

            if(!File.Exists(fileName)) {
                // 現在処理中のテンプレートファイルからの相対位置で実在チェックする.
                string dir = Path.GetDirectoryName(this.TemplateFile);
                string candidate = Path.Combine(dir, fileName);
                if(File.Exists(candidate)) {
                    return candidate;
                }
            }

            // 不明
            return fileName;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            location = ResolvePath(requestFileName);
            if(File.Exists(location)) {
                content = File.ReadAllText(location);
                return true;
            }

            content = "";
            return false;
        }

        public virtual void LogErrors(CompilerErrorCollection errors)
        {
            Error(this, new TextTemplatingErrorEventArgs(errors));
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            // テンプレートで、hostSpecific="true" の場合に、<#@ parameter ...#>を使用した場合で、
            // その変数名がSessionにない場合に、このメソッドが呼び出される.
            // (hostSpecific="false"であるか、セッションに変数がある場合は呼び出されない.)
            throw new NotImplementedException();
        }

        private string _extension;

        public string Extension
        {
            get
            {
                return _extension;
            }

            set
            {
                _extension = value;
            }
        }

        public void SetFileExtension(string extension)
        {
            this.Extension = extension;
        }

        private Encoding _encoding = Encoding.UTF8;

        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }

            set
            {
                _encoding = value;
            }
        }

        public void SetOutputEncoding(System.Text.Encoding encoding, bool fromOutputDirective)
        {
            this.Encoding = encoding;
        }

        /// <summary>
        /// 標準で参照するアセンブリの一覧
        /// </summary>
        public IList<string> StandardAssemblyReferences
        {
            get
            {
                var ret = new string[] {
                    typeof(System.Uri).Assembly.Location, // System名前空間用
					typeof(System.Linq.Enumerable).Assembly.Location, // Linq名前空間用
					typeof(ITextTemplatingEngineHost).Assembly.Location, // T4エンジンのホストインターフェイス用
				};
                return ret;
            }
        }

        /// <summary>
        /// 標準でインポートする名前空間の一覧
        /// </summary>
        public IList<string> StandardImports
        {
            get
            {
                return new string[] {
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Text"
                };
            }
        }
    }
}
