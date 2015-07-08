namespace ContentTypeTextNet.Library.SharedLibrary.Logic.T4Template
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Remoting;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using Microsoft.VisualStudio.TextTemplating;

	/// <summary>
	/// テンプレートクラスをAppDomain間で利用できるようするProxy
	/// </summary>
	public class RuntimeTextTemplateProxy : MarshalByRefObject, IRuntimeTextTemplate
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
			if (!_disposed) {
				InstanceTemplateHost = null;
				InstanceTemplateTransformText = null;
				InstanceTemplate = null;

				RemotingServices.Disconnect(this);
				_disposed = true;
			}
		}

		public ITextTemplatingEngineHost Host
		{
			get
			{
				if (InstanceTemplateHost != null) {
					return InstanceTemplateHost.GetValue(InstanceTemplate) as ITextTemplatingEngineHost;
				}

				return null;
			}
			set
			{
				if (InstanceTemplateHost != null) {
					InstanceTemplateHost.SetValue(InstanceTemplate, value);
				}
			}
		}

		public string TransformText()
		{
			Debug.Assert(InstanceTemplateTransformText != null);
			//Debug.WriteLine("current appdomain=" + AppDomain.CurrentDomain.FriendlyName);
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
