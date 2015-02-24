namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Remoting;
	using System.Runtime.Serialization;
	using System.Security;
	using System.Text;
	using Microsoft.VisualStudio.TextTemplating;
	//using Microsoft.VisualStudio.TextTemplating;
	using Mono.TextTemplating;

	public static class TextTemplating
	{
		public static string test(string templateContent)
		{
			var host = new TextTemplatingEngineHostImpl();
			host.Session = new TextTemplatingSessionImpl();

			host.TemplateFile = "TemplateSample1.tt";
			host.Session["maxCount"] = 2;
	
			// T4 Engine
			var engine = new Engine();

			// テンプレートをソースコードに変換する.(実行時テンプレート)
			string className = "GeneratedClass";
			string namespaceName = "TemplateEngineExample";
			string lang;
			string[] references;
			string generatedSource = engine.PreprocessTemplate(
				templateContent, // テンプレート
				host, // ホスト
				className, // 生成するテンプレートクラス名
				namespaceName, // 生成するテンプレートクラスの名前空間
				out lang, // 生成するソースコードの種別が返される
				out references // 参照しているアセンブリの一覧が返される
				);



			// コンパイラを取得する.
			var codeDomProv = CodeDomProvider.CreateProvider(lang);

			// 参照するアセンブリの定義
			var compilerParameters = new CompilerParameters(references);

			// アセンブリはインメモリで作成する.
			compilerParameters.GenerateInMemory = true;

			// コンパイルする.
			var result = codeDomProv.CompileAssemblyFromSource(
				compilerParameters, generatedSource);

			// エラーがあれば例外を返す.
			if(result.Errors.Count > 0) {
				var msg = new StringBuilder();
				foreach(CompilerError error in result.Errors) {
					msg.Append(error.FileName).Append(": line ").Append(error.Line)
						.Append("(").Append(error.Column).Append(")[")
						.Append(error.ErrorNumber).Append("]")
						.Append(error.ErrorText).AppendLine();
				}
				throw new ApplicationException(msg.ToString());
			}

			// エラーがなければアセンブリを取得し、
			// テンプレートクラスのインスタンスを作成する.
			Assembly assembly = result.CompiledAssembly;
			var fqClassName = namespaceName + "." + className;
			//var instance = assembly.CreateInstance(fqClassName); // 名前空間.クラス名を指定してクラスを取得
			//Type type = instance.GetType();
			//type.InvokeMember("TransformText", BindingFlags.InvokeMethod, null, instance, null);
			//MethodInfo methid = type.GetMethod("TransformText");
			//var ret = methid.Invoke(instance, null);
			//return ret.ToString();
			Type type = assembly.GetType(fqClassName); // 名前空間.クラス名を指定してクラスを取得
			dynamic templateInstance = Activator.CreateInstance(type);
			templateInstance.Host = host;
			string output = templateInstance.TransformText();
			return output;
		}
	}

	/// <summary>
	/// TextTemplatingEngineHostの実装.
	/// 大まかな実装はMSDNの以下のページを参考とした.
	/// http://msdn.microsoft.com/ja-jp/library/bb126579.aspx
	/// </summary>
	[Serializable]
	class TextTemplatingEngineHostImpl
		: ITextTemplatingEngineHost
		, ITextTemplatingSessionHost
	{
		/// <summary>
		/// 現在処理中のテンプレートファイル
		/// </summary>
		public string TemplateFile { set; get; }

		/// <summary>
		/// T4エンジンがコードを生成する先のAppDomain
		/// </summary>
		public AppDomain AppDomain { set; get; }

		/// <summary>
		/// テンプレートに引き渡すデータを保持するセッション
		/// </summary>
		private ITextTemplatingSession session = null;

		public ITextTemplatingSession CreateSession()
		{
			return session;
		}

		public ITextTemplatingSession Session
		{
			get
			{
				return session;
			}
			set
			{
				session = value;
			}
		}

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

		public void LogErrors(CompilerErrorCollection errors)
		{
			foreach(CompilerError error in errors) {
				Console.Error.WriteLine(error.Line + ":" + error.Column +
					" #" + error.ErrorNumber + " " + error.ErrorText);
			}
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
				var ret = new string[]
                {
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
				return new string[]
                {
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
	public sealed class TextTemplatingSessionImpl
		: Dictionary<string, Object>
		, ITextTemplatingSession
		, ISerializable
	{
		public TextTemplatingSessionImpl()
		{
			this.Id = Guid.NewGuid();
		}

		private TextTemplatingSessionImpl(
		   SerializationInfo info,
		   StreamingContext context)
			: base(info, context)
		{
			this.Id = (Guid)info.GetValue("Id", typeof(Guid));
		}

		[SecurityCritical]
		void ISerializable.GetObjectData(SerializationInfo info,
		   StreamingContext context)
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
	
	///// <summary>
	///// 実行時テンプレートのインターフェイス
	///// </summary>
	//public interface IRuntimeTextTemplate: IDisposable
	//{
	//	/// <summary>
	//	/// ホスト
	//	/// </summary>
	//	ITextTemplatingEngineHost Host { set; get; }

	//	/// <summary>
	//	/// テンプレート変換を実施する.
	//	/// </summary>
	//	/// <returns></returns>
	//	string TransformText();
	//}

	///// <summary>
	///// テンプレートクラスをAppDomain間で利用できるようするProxy
	///// </summary>
	//class RuntimeTextTemplateProxyImpl
	//	: MarshalByRefObject
	//	, IRuntimeTextTemplate
	//{
	//	/// <summary>
	//	/// テンプレートクラスのインスタンス
	//	/// </summary>
	//	private dynamic templateInstance;

	//	/// <summary>
	//	/// 破棄済みフラグ
	//	/// </summary>
	//	private bool _disposed;

	//	/// <summary>
	//	/// 初期化。アセンブリをロードする.
	//	/// </summary>
	//	/// <param name="assemblyBytes">ロードするアセンブリの内容</param>
	//	/// <param name="fqClassName">名前空間・クラス名</param>
	//	public void LoadAssembly(byte[] assemblyBytes, string fqClassName)
	//	{
	//		var assembly = Assembly.Load(assemblyBytes);
	//		templateInstance = (dynamic)assembly.CreateInstance(fqClassName);
	//	}

	//	~RuntimeTextTemplateProxyImpl()
	//	{
	//		Dispose(false);
	//	}

	//	public void Dispose()
	//	{
	//		GC.SuppressFinalize(this);
	//		Dispose(true);
	//	}

	//	protected virtual void Dispose(bool disposing)
	//	{
	//		if(!_disposed) {
	//			templateInstance = null;

	//			RemotingServices.Disconnect(this);
	//			_disposed = true;
	//		}
	//	}

	//	public ITextTemplatingEngineHost Host
	//	{
	//		set
	//		{
	//			templateInstance.Host = value;
	//		}

	//		get
	//		{
	//			return templateInstance.Host;
	//		}
	//	}

	//	public string TransformText()
	//	{
	//		System.Diagnostics.Debug.WriteLine("current appdomain=" + AppDomain.CurrentDomain.FriendlyName);
	//		return templateInstance.TransformText();
	//	}

	//	public sealed override object InitializeLifetimeService()
	//	{
	//		// AppDomainを越えてアクセスするため、マーシャリングされているが
	//		// 使用期間は不明であるため無期限とする.
	//		// そのため、使い終わったらDisposeメソッドを呼び出し、Disconnectする必要がある.
	//		return null;
	//	}
	//}

	///// <summary>
	///// テンプレートクラスを構築するファクトリ
	///// </summary>
	//public class RuntimeTextTemplateFactory
	//{
	//	/// <summary>
	//	/// 生成したテンプレートクラスをロードするAppDomain
	//	/// </summary>
	//	public AppDomain TemplateAppDomain { get; set; }

	//	/// <summary>
	//	/// T4テンプレートエンジン
	//	/// </summary>
	//	private Engine engine;

	//	public RuntimeTextTemplateFactory()
	//	{
	//		this.engine = new Engine();
	//		this.TemplateAppDomain = AppDomain.CurrentDomain;
	//	}

	//	/// <summary>
	//	/// ファイルを指定してテンプレートを構築する.
	//	/// </summary>
	//	/// <param name="templateFile">テンプレートファイル</param>
	//	/// <returns>テンプレートクラスのインスタンス</returns>
	//	public IRuntimeTextTemplate Generate(string templateFile)
	//	{
	//		string templateContent = File.ReadAllText(templateFile);
	//		return Generate(templateContent, templateFile);
	//	}

	//	/// <summary>
	//	/// テンプレートとファイルの位置を指定してテンプレートを構築する.
	//	/// </summary>
	//	/// <param name="templateContent">テンプレートの内容</param>
	//	/// <param name="templateFile">テンプレートファイルの位置</param>
	//	/// <returns>テンプレートクラスのインスタンス</returns>
	//	public IRuntimeTextTemplate Generate(string templateContent, string templateFile)
	//	{
	//		TextTemplatingEngineHostImpl host = new TextTemplatingEngineHostImpl();
	//		host.TemplateFile = templateFile;

	//		// 生成するクラス名をランダムに作成する.
	//		// (アセンブリが毎回異なるので必須ではないが、一応。)
	//		Guid id = Guid.NewGuid();
	//		String className = "Generated" +
	//			BitConverter.ToString(id.ToByteArray()).Replace("-", "");

	//		// テンプレートをソースコードに変換する.(実行時テンプレート)
	//		string lang;
	//		string[] references;
	//		string generatedSource = engine.PreprocessTemplate(
	//			templateContent,
	//			host,
	//			className,
	//			"TemplateEngineExample",
	//			out lang,
	//			out references
	//			);
	//		string fqClassName = "TemplateEngineExample." + className;

	//		// アセンブリの位置が確定していないものは先に確定しておく
	//		var resolvedReferences = references.Select(host.ResolveAssemblyReference)
	//			.Where(x => !string.IsNullOrEmpty(x)).ToArray();


	//		// コンパイラを取得する.
	//		var codeDomProv = CodeDomProvider.CreateProvider(lang);

	//		// 参照するアセンブリの定義
	//		// アセンブリはテンポラリに作成する.
	//		var compilerParameters = new CompilerParameters(references);

	//		// コンパイルする.
	//		var result = codeDomProv.CompileAssemblyFromSource(
	//			compilerParameters, generatedSource);

	//		// エラーがあれば例外を返す.
	//		if(result.Errors.Count > 0) {
	//			var msg = new StringBuilder();
	//			foreach(CompilerError error in result.Errors) {
	//				msg.Append(error.FileName).Append(": line ").Append(error.Line)
	//					.Append("(").Append(error.Column).Append(")[")
	//					.Append(error.ErrorNumber).Append("]")
	//					.Append(error.ErrorText).AppendLine();
	//			}
	//			throw new ApplicationException(msg.ToString());
	//		}

	//		// エラーがなければ生成されたアセンブリを取得する.
	//		byte[] assemblyBytes = File.ReadAllBytes(result.PathToAssembly);

	//		try {
	//			// 生成されたアセンブリファイルは不要になるので削除する.
	//			File.Delete(result.PathToAssembly);
	//		} catch(Exception ex) {
	//			System.Diagnostics.Debug.WriteLine("Can't delete file: " + ex);
	//			// 削除失敗しても無視して継続する.
	//		}

	//		// ターゲットのAppDomain内でアセンブリをロードするためのプロキシを作成する.
	//		var proxy = (RuntimeTextTemplateProxyImpl)TemplateAppDomain.CreateInstanceAndUnwrap(
	//			typeof(RuntimeTextTemplateProxyImpl).Assembly.FullName,
	//			typeof(RuntimeTextTemplateProxyImpl).FullName);

	//		// アセンブリをロードさせる.
	//		proxy.LoadAssembly(assemblyBytes, fqClassName);

	//		return proxy;
	//	}
	//}
}
