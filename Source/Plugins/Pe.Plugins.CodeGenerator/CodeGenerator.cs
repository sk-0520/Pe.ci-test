using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.CodeGenerator.Addon;

namespace ContentTypeTextNet.Pe.Plugins.CodeGenerator
{
    public class CodeGenerator: PluginBase, IAddon
    {
        #region variable

        CodeGeneratorAddonImpl _addon;

        #endregion

        public CodeGenerator(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new CodeGeneratorAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        //protected override IPreferences CreatePreferences() => new ClockPreferences(this);

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion

    }
}
