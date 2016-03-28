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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Event;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.IF;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;

namespace ContentTypeTextNet.Pe.PeMain.Logic.T4Template
{
    /// <summary>
    /// 各種変換、実行をサポートする。
    /// 
    /// Bugs: VBはその、コンパイルできん。
    /// <para>これSerializableいらんと思うんだけどなぁ。</para>
    /// </summary>
    [Serializable]
    public class T4TemplateProcessor: DisposeFinalizeBase
    {
        #region variable

        /// <summary>
        /// テンプレートソース。
        /// </summary>
        private string _templateSource;
        /// <summary>
        /// プログラミング言語。
        /// </summary>
        private string _programmingLanguage;
        /// <summary>
        /// アプリケーションドメイン名。
        /// </summary>
        private string _templateAppDomainName;
        /// <summary>
        /// ネームスペース名。
        /// </summary>
        private string _namespaceName;
        /// <summary>
        /// クラス名。
        /// </summary>
        private string _className;
        /// <summary>
        /// テンプレートソース -> プログラムソースでのエラー。
        /// </summary>
        private List<CompilerError> _generatedErrorList = new List<CompilerError>();
        /// <summary>
        /// プログラムソース -> アセンブリでのエラー。
        /// </summary>
        private List<CompilerError> _compileErrorList = new List<CompilerError>();

        #endregion

        /// <summary>
        /// デフォルト値での生成。
        /// </summary>
        public T4TemplateProcessor(ILogger logger = null)
            : this(new TextTemplatingEngineHost() {
                Session = new TextTemplatingSession()
            }, logger)
        { }

        /// <summary>
        /// 指定データでの生成。
        /// </summary>
        /// <param name="host">セッション定義済みのホスト環境。</param>
        /// <param name="logger"></param>
        public T4TemplateProcessor(TextTemplatingEngineHost host, ILogger logger = null)
        {
            //Session = host.Session;
            Host = host;
            Variable = host.Session as TextTemplatingSession;

            Logger = logger;

            Initialize();
        }

        #region property

        protected ILogger Logger { get; private set; }

        /// <summary>
        /// テンプレートソース。
        /// </summary>
        public string TemplateSource
        {
            get { return this._templateSource ?? string.Empty; }
            set
            {
                if(this._templateSource != value) {
                    DisposeTemplateSource(true);
                    this._templateSource = value;
                }
            }
        }
        /// <summary>
        /// テンプレートソースからプログラムソースへ変換済みか。
        /// </summary>
        public bool Generated { get; private set; }
        /// <summary>
        /// プログラムソースはコンパイル済みか。
        /// </summary>
        public bool Compiled { get; private set; }

        /// <summary>
        /// T4で使用されるプログラミング言語。
        /// </summary>
        public string ProgrammingLanguage
        {
            get { return this._programmingLanguage; }
        }
        /// <summary>
        /// T4から変換されたプログラムソース。
        /// </summary>
        public string GeneratedProgramSource { get; protected set; }

        /// <summary>
        /// ホスト。
        /// </summary>
        public ITextTemplatingEngineHost Host { get; private set; }
        /// <summary>
        /// セッションに乗せとくデータ。
        /// </summary>
        public IDictionary<string, object> Variable { get; private set; }

        /// <summary>
        /// 参照アセンブリ。
        /// </summary>
        public IReadOnlyList<string> References { get; private set; }

        /// <summary>
        /// テンプレートソース変換時のエラー。
        /// </summary>
        public IReadOnlyList<CompilerError> GeneratedErrorList { get { return this._generatedErrorList; } }

        /// <summary>
        /// 言語ソースの名前空間。
        /// </summary>
        public string NamespaceName
        {
            get { return this._namespaceName; }
            set
            {
                if(Generated) {
                    throw new InvalidOperationException("NamespaceName");
                }

                this._namespaceName = value;
            }
        }
        /// <summary>
        /// 言語ソースのクラス名。
        /// </summary>
        public string ClassName
        {
            get { return this._className; }
            set
            {
                if(Generated) {
                    throw new InvalidOperationException("ClassName");
                }

                this._className = value;
            }
        }

        public string FullyQualifiedClassName
        {
            get { return string.Format("{0}.{1}", NamespaceName, ClassName); }
        }

        /// <summary>
        /// テンプレート実行時に使用するアプリケーションドメイン名。
        /// </summary>
        public string TemplateAppDomainName
        {
            get { return this._templateAppDomainName; }
            set
            {
                if(Compiled) {
                    throw new InvalidOperationException("Compiled");
                }

                this._templateAppDomainName = value;
            }
        }


        /// <summary>
        /// プログラムソースコンパイルエラー。
        /// </summary>
        public IReadOnlyList<CompilerError> CompileErrorList { get { return this._compileErrorList; } }
        /// <summary>
        /// プログラムソースコンパイルメッセージ。
        /// </summary>
        public string CompileMessage { get; private set; }

        /// <summary>
        /// アセンブリの走るアプリケーションドメイン。
        /// </summary>
        protected AppDomain TemplateAppDomain { get; set; }
        protected bool IsOtherAppDomain
        {
            get
            {
                if(TemplateAppDomain == null) {
                    throw new InvalidOperationException("IsOtherAppDomain");
                }

                return AppDomain.CurrentDomain != TemplateAppDomain;
            }
        }
        /// <summary>
        /// アセンブリを走らせるためのプロクシ。
        /// </summary>
        public IRuntimeTextTemplate TemplateProxy { get; set; }
        /// <summary>
        /// 実行になんかあったエラー。
        /// </summary>
        public Exception Error { get; protected set; }

        public int FirstLineNumber { get; protected set; }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DisposeTemplateSource(disposing);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region function

        /// <summary>
        /// コンストラクタからの初期化。
        /// </summary>
        protected virtual void Initialize()
        {
            CompileMessage = string.Empty;
            FirstLineNumber = 1;
        }

        /// <summary>
        /// テンプレートソース破棄。
        /// </summary>
        protected virtual void DisposeTemplateSource(bool disposing)
        {
            DisposeProgramSource(disposing);

            //TemplateSource = string.Empty;
            this._generatedErrorList.Clear();
            Generated = false;
        }

        /// <summary>
        /// プログラムソース破棄。
        /// </summary>
        protected virtual void DisposeProgramSource(bool disposing)
        {
            DisposeAssembly(disposing);

            this._compileErrorList.Clear();
            Compiled = false;
            CompileMessage = string.Empty;
            //GeneratedProgramSource = string.Empty;
        }

        /// <summary>
        /// アセンブリ破棄。
        /// </summary>
        protected virtual void DisposeAssembly(bool disposing)
        {
            if(TemplateProxy != null) {
                try {
                    if(TemplateProxy != null) {
                        TemplateProxy.Dispose();
                    }
                } catch(RemotingException ex) {
                    Logger.SafeWarning(ex);
                }
                TemplateProxy = null;
            }
            if(TemplateAppDomain != null) {
                if(IsOtherAppDomain) {
                    try {
                        AppDomain.Unload(TemplateAppDomain);
                    } catch(CannotUnloadAppDomainException ex) {
                        Logger.SafeWarning(ex);
                    }
                }
                TemplateAppDomain = null;
            }
        }

        /// <summary>
        /// 実際に使用するテンプレートソースの作成。
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeTemplateSource()
        {
            return TemplateSource;
        }

        /// <summary>
        /// 実際に使用する言語ソースの作成。
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeProgramSource()
        {
            var source = new StringBuilder(GeneratedProgramSource.Length + 40);

            source.AppendLine("#pragma warning disable 1709");
            source.AppendLine(GeneratedProgramSource);
            source.AppendLine("#pragma warning restore 1709");

            return source.ToString();
        }

        /// <summary>
        /// T4を言語ソースに変換。
        /// </summary>
        public void GeneratProgramSource()
        {
            var eventHost = Host as TextTemplatingEngineHost;

            try {
                if(eventHost != null) {
                    eventHost.Error += WithEvent_Error;
                }

                GeneratProgramSource_Impl();
            } finally {
                if(eventHost != null) {
                    eventHost.Error -= WithEvent_Error;
                }
            }
        }

        /// <summary>
        /// T4を言語ソースに変換する実装。
        /// </summary>
        protected virtual void GeneratProgramSource_Impl()
        {
            if(string.IsNullOrWhiteSpace(NamespaceName)) {
                throw new InvalidOperationException("Namespace");
            }
            if(string.IsNullOrWhiteSpace(ClassName)) {
                throw new InvalidOperationException("ClassName");
            }

            var templateSource = MakeTemplateSource();

            if(string.IsNullOrEmpty(templateSource)) {
                throw new InvalidOperationException("TemplateSource");
            }

            this._generatedErrorList.Clear();

            string sourceCode = string.Empty;
            string programmingLanguage = string.Empty;
            string[] references = null;
            var engine = new Engine();
            try {
                sourceCode = engine.PreprocessTemplate(
                    templateSource,
                    Host,
                    ClassName,
                    NamespaceName,
                    out programmingLanguage,
                    out references
                );
            } catch(ParserException ex) {
                Error = ex;
                return;
            }
            Error = null;

            if(!GeneratedErrorList.Any()) {
                Generated = true;
                GeneratedProgramSource = sourceCode;
                this._programmingLanguage = programmingLanguage;
                References = references;
            }
        }

        /// <summary>
        /// 言語ソースをコンパイルする。
        /// </summary>
        public void CompileProgramSource()
        {
            CompileProgramSource(WarningLevel.Full, true);
        }
        /// <summary>
        /// 言語ソースをコンパイルする。
        /// </summary>
        /// <param name="warningLevel">警告レベル。</param>
        /// <param name="warningIsError">警告をエラーとして扱うか。</param>
        public void CompileProgramSource(WarningLevel warningLevel, bool warningIsError)
        {
            CompileProgramSource(
                warningLevel,
                warningIsError,
                new Dictionary<string, string>()
            );
        }
        /// <summary>
        /// 言語ソースをコンパイルする。
        /// </summary>
        /// <param name="warningLevel">警告レベル。</param>
        /// <param name="warningIsError">警告をエラーとして扱うか。</param>
        /// <param name="option">コンパイルオプション。</param>
        public void CompileProgramSource(WarningLevel warningLevel, bool warningIsError, IDictionary<string, string> option)
        {
            CompileProgramSource_Impl(
                warningLevel,
                warningIsError,
                option
            );
        }

        /// <summary>
        /// 言語ソースをコンパイルする実装。
        /// </summary>
        /// <param name="warningLevel">警告レベル。</param>
        /// <param name="warningIsError">警告をエラーとして扱うか。</param>
        /// <param name="option">コンパイルオプション。</param>
        protected virtual void CompileProgramSource_Impl(WarningLevel warningLevel, bool warningIsError, IDictionary<string, string> option)
        {
            if(!Generated) {
                throw new InvalidOperationException("Generated");
            }
            CheckUtility.DebugEnforceNotNullAndNotEmpty(ProgrammingLanguage);
            CheckUtility.DebugEnforceNotNullAndNotEmpty(GeneratedProgramSource);
            CheckUtility.DebugEnforceNotNullAndNotEmpty(NamespaceName);
            CheckUtility.DebugEnforceNotNullAndNotEmpty(ClassName);

            DisposeProgramSource(true);

            // コンパイル準備
            var codeDomProv = CodeDomProvider.CreateProvider(ProgrammingLanguage, option);
            var compilerParameters = new CompilerParameters(References.ToArray()) {
                //GenerateInMemory = true,
                WarningLevel = (int)warningLevel,
                TreatWarningsAsErrors = warningIsError,
            };

            // コンパイル
            // プリプロセッサ #line のファイル名対応
            var source = MakeProgramSource();
            var compileResult = codeDomProv.CompileAssemblyFromSource(compilerParameters, source);
            CompileMessage = string.Join(Environment.NewLine, compileResult.Output.Cast<string>());
            this._compileErrorList.AddRange(compileResult.Errors.Cast<CompilerError>());

            if(!this._compileErrorList.Any()) {
                Compiled = true;
                // アセンブリを読み込む
                var pathAssembly = compileResult.PathToAssembly;
                var binaryAssembly = File.ReadAllBytes(pathAssembly);
                try {
                    File.Delete(pathAssembly);
                } catch(Exception ex) {
                    Logger.SafeWarning(ex);
                }

                if(string.IsNullOrEmpty(TemplateAppDomainName)) {
                    TemplateAppDomain = AppDomain.CurrentDomain;
                    Debug.Assert(!IsOtherAppDomain);
                } else {
                    TemplateAppDomain = AppDomain.CreateDomain(
                        TemplateAppDomainName,
                        null,
                        new AppDomainSetup() {
                            ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                        }
                    );
                    //TemplateAppDomain = AppDomain.CreateDomain(TemplateAppDomainName);
                    Debug.Assert(IsOtherAppDomain);
                }

                var fullyQualifiedClassName = FullyQualifiedClassName;

                var templateProxy = (RuntimeTextTemplateProxy)TemplateAppDomain.CreateInstanceAndUnwrap(
                    typeof(RuntimeTextTemplateProxy).Assembly.FullName,
                    typeof(RuntimeTextTemplateProxy).FullName
                );

                templateProxy.LoadAssembly(binaryAssembly, fullyQualifiedClassName);
                TemplateProxy = templateProxy;
            }
        }

        /// <summary>
        /// コンパイル済みプログラムから出力。
        /// </summary>
        /// <returns></returns>
        public string TransformText()
        {
            try {
                Error = null;
                return TransformText_Impl();
            } catch(Exception ex) {
                Error = ex;
                return ex.ToString();
            }
        }

        /// <summary>
        /// コンパイル済みプログラムから出力する実装。
        /// </summary>
        /// <returns></returns>
        protected virtual string TransformText_Impl()
        {
            if(!Compiled) {
                throw new InvalidOperationException("Compiled");
            }

            //Debug.Assert(CompiledAssembly != null);
            Debug.Assert(TemplateProxy != null);

            TemplateProxy.Host = Host;

            //return (string)InstanceTemplateTransformText.Invoke(InstanceTemplate, null);
            //InstanceTemplate.TransformText();
            return TemplateProxy.TransformText();
        }

        /// <summary>
        /// T4 -> プログラムソース -> アセンブリまで一直線。
        /// </summary>
        public void AllProcess()
        {
            GeneratProgramSource();

            if(Generated) {
                CompileProgramSource();
            }
        }

        #endregion

        void WithEvent_Error(object sender, TextTemplatingErrorEventArgs e)
        {
            var list = e.CompilerErrorCollection.Cast<CompilerError>();
            this._generatedErrorList.AddRange(list);
        }
    }
}
