namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Remoting;
	using System.Runtime.Serialization;
	using System.Security;
	using System.Text;
	using Microsoft.VisualStudio.TextTemplating;
	using Mono.TextTemplating;

	public enum WarningLevel
	{
		None = 0,
		Full = 4,
	}

	/// <summary>
	/// T4Templateの便利処理。
	/// </summary>
	public static class T4TemplateUtility
	{
		public static string TransformText(string templateContent)
		{
			var host = new TextTemplatingEngineHost();
			host.Session = new TextTemplatingSession();

			var engine = new Engine();
			return engine.ProcessTemplate(templateContent, host);
		}

		public static string TransformTextWidthVariable(string templateContent, IDictionary<string, object> variable)
		{
			var host = new TextTemplatingEngineHost();
			host.Session = new TextTemplatingSession();
			foreach(var pair in variable) {
				host.Session.Add(pair);
			}

			var engine = new Engine();
			return engine.ProcessTemplate(templateContent, host);
		}
	}

	/// <summary>
	/// 各種変換、実行をサポートする。
	/// 
	/// Bugs: VBはその、コンパイルできん。
	/// <para>これSerializableいらんと思うんだけどなぁ。</para>
	/// </summary>
	[Serializable]
	public class T4TemplateProcessor: IDisposable
	{
		private string _templateSource;
		private string _programmingLanguage;
		private string _templateAppDomainName;
		private string _namespaceName;
		private string _className;
		private List<CompilerError> _generatedErrorList = new List<CompilerError>();
		private List<CompilerError> _compileErrorList = new List<CompilerError>();

		public T4TemplateProcessor()
			: this(new TextTemplatingEngineHost() {
				Session = new TextTemplatingSession()
			})
		{ }

		public T4TemplateProcessor(TextTemplatingEngineHost host)
		{
			//Session = host.Session;
			Host = host;
			Variable = host.Session as TextTemplatingSession;

			var eventHost = Host as TextTemplatingEngineHost;
			if(eventHost != null) {
				eventHost.Error += WithEvent_Error;
			}

			Initialize();
		}

		~T4TemplateProcessor()
		{
			var eventHost = Host as TextTemplatingEngineHost;
			if(eventHost != null) {
				eventHost.Error -= WithEvent_Error;
			}
			Dispose(false);
		}

		/// <summary>
		/// テンプレートソース。
		/// </summary>
		public string TemplateSource 
		{
			get { return this._templateSource; }
			set {
				if(this._templateSource != value) {
					DisposeTemplateSource();
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
		/// セッション。
		/// </summary>
		//public ITextTemplatingSession Session { get; private set; }
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

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			DisposeTemplateSource();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		protected virtual void Initialize()
		{
			CompileMessage = string.Empty;
		}

		/// <summary>
		/// テンプレートソース破棄。
		/// </summary>
		protected virtual void DisposeTemplateSource()
		{
			DisposeProgramSource();

			//TemplateSource = string.Empty;
			this._generatedErrorList.Clear();
			Generated = false;
		}

		/// <summary>
		/// プログラムソース破棄。
		/// </summary>
		protected virtual void DisposeProgramSource()
		{
			DisposeAssembly();

			this._compileErrorList.Clear();
			Compiled = false;
			CompileMessage = string.Empty;
			//GeneratedProgramSource = string.Empty;
		}

		/// <summary>
		/// アセンブリ破棄。
		/// </summary>
		protected virtual void DisposeAssembly()
		{
			if(TemplateProxy != null) {
				TemplateProxy.ToDispose();
				TemplateProxy = null;
			}
			if(TemplateAppDomain != null) {
				if(IsOtherAppDomain) {
					AppDomain.Unload(TemplateAppDomain);
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
			GeneratProgramSource_Impl();
		}

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

			string programmingLanguage;
			string[] references;
			var engine = new Engine();
			string sourceCode = engine.PreprocessTemplate(
				templateSource,
				Host,
				ClassName,
				NamespaceName,
				out programmingLanguage,
				out references
			);

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

		protected virtual void CompileProgramSource_Impl(WarningLevel warningLevel, bool warningIsError, IDictionary<string, string> option)
		{
			if(!Generated) {
				throw new InvalidOperationException("Generated");
			}
			Debug.Assert(!string.IsNullOrEmpty(ProgrammingLanguage));
			Debug.Assert(!string.IsNullOrEmpty(GeneratedProgramSource));
			Debug.Assert(!string.IsNullOrWhiteSpace(NamespaceName));
			Debug.Assert(!string.IsNullOrWhiteSpace(ClassName));

			DisposeProgramSource();

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
					Debug.WriteLine(ex);
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

		public string TransformText()
		{
			return TransformText_Impl();
		}

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

		public void AllProcess()
		{
			GeneratProgramSource();

			if(Generated) {
				CompileProgramSource();
			}
		}

		void WithEvent_Error(object sender, TextTemplatingErrorEventArgs e)
		{
			var list = e.CompilerErrorCollection.Cast<CompilerError>();
			this._generatedErrorList.AddRange(list);
		}
	}


	/// <summary>
	/// T4変換時のエラー出力イベント。
	/// </summary>
	public class TextTemplatingErrorEventArgs: EventArgs
	{
		public TextTemplatingErrorEventArgs(CompilerErrorCollection errors)
		{
			CompilerErrorCollection = errors;
		}

		public CompilerErrorCollection CompilerErrorCollection { get; private set; }
	}

	/// <summary>
	/// http://d.hatena.ne.jp/seraphy/20140419
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

	/// <summary>
	/// セッションの実装.
	/// 単純にDictionaryと、IDを持っているだけのコレクションクラスである.
	/// </summary>
	[Serializable]
	public sealed class TextTemplatingSession: Dictionary<string, Object>, ITextTemplatingSession, ISerializable 
	{
		public TextTemplatingSession()
		{
			this.Id = Guid.NewGuid();
		}

		private TextTemplatingSession(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.Id = (Guid)info.GetValue("Id", typeof(Guid));
		}

		[SecurityCritical]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Id", this.Id);
		}

		public Guid Id
		{
			get;
			private set;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var o = obj as TextTemplatingSession;
			return o != null && o.Equals(this);
		}

		public bool Equals(Guid other)
		{
			return other.Equals(Id);
		}

		public bool Equals(ITextTemplatingSession other)
		{
			return other != null && other.Id == this.Id;
		}
	}

	/// <summary>
	/// 実行時テンプレートのインターフェイス
	/// </summary>
	public interface IRuntimeTextTemplate: IDisposable
	{
		ITextTemplatingEngineHost Host { get; set; }

		/// <summary>
		/// テンプレート変換を実施する.
		/// </summary>
		/// <returns></returns>
		string TransformText();
	}

	/// <summary>
	/// テンプレートクラスをAppDomain間で利用できるようするProxy
	/// </summary>
	public class RuntimeTextTemplateProxy: MarshalByRefObject, IRuntimeTextTemplate
	{
		/// <summary>
		/// 破棄済みフラグ
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// コンパイル後の生成オブジェクト。
		/// 
		/// ResetBindException初回例外がしんどいのでdynamic使わない。
		/// </summary>
		object InstanceTemplate { get; set; }

		PropertyInfo InstanceTemplateHost { get; set; }
		MethodInfo InstanceTemplateTransformText { get; set; }

		/// <summary>
		/// 初期化。アセンブリをロードする.
		/// </summary>
		/// <param name="assemblyBytes">ロードするアセンブリの内容</param>
		/// <param name="fullyQualifiedClassName">名前空間・クラス名</param>
		public void LoadAssembly(byte[] assemblyBytes, string fullyQualifiedClassName)
		{
			var assembly = Assembly.Load(assemblyBytes);
			InstanceTemplate = assembly.CreateInstance(fullyQualifiedClassName);

			var classType = assembly.GetType(fullyQualifiedClassName);
			InstanceTemplateHost = classType.GetProperty("Host");
			InstanceTemplateTransformText = classType.GetMethod("TransformText");

		}

		~RuntimeTextTemplateProxy()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!_disposed) {
				InstanceTemplate = null;

				RemotingServices.Disconnect(this);
				_disposed = true;
			}
		}

		public ITextTemplatingEngineHost Host
		{
			get
			{
				if(InstanceTemplateHost != null) {
					return InstanceTemplateHost.GetValue(InstanceTemplate) as ITextTemplatingEngineHost;
				}

				return null;
			}
			set
			{
				if(InstanceTemplateHost != null) {
					InstanceTemplateHost.SetValue(InstanceTemplate, value);
				}
			}
		}

		public string TransformText()
		{
			Debug.WriteLine("current appdomain=" + AppDomain.CurrentDomain.FriendlyName);
			return (string)InstanceTemplateTransformText.Invoke(InstanceTemplate, null);

		}

		public sealed override object InitializeLifetimeService()
		{
			// AppDomainを越えてアクセスするため、マーシャリングされているが
			// 使用期間は不明であるため無期限とする.
			// そのため、使い終わったらDisposeメソッドを呼び出し、Disconnectする必要がある.
			return null;
		}
	}
}
