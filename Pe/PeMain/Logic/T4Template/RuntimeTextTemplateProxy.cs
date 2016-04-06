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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.PeMain.IF;
using Microsoft.VisualStudio.TextTemplating;

namespace ContentTypeTextNet.Pe.PeMain.Logic.T4Template
{
    /// <summary>
    /// テンプレートクラスをAppDomain間で利用できるようするProxy
    /// </summary>
    public class RuntimeTextTemplateProxy: MarshalByRefObject, IRuntimeTextTemplate, IIsDisposed
    {
        #region IIsDisposed

        [field: NonSerialized]
        public event EventHandler Disposing;

        /// <summary>
        /// 破棄済みフラグ
        /// </summary>
        public bool IsDisposed { get; private set; }

        #endregion

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
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                InstanceTemplateHost = null;
                InstanceTemplateTransformText = null;
                InstanceTemplate = null;

                if(Disposing != null) {
                    Disposing(this, EventArgs.Empty);
                }

                RemotingServices.Disconnect(this);
                GC.SuppressFinalize(this);
                IsDisposed = true;
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
