using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public sealed class LauncherItemWithIconElement<TLauncherItemElement> : ElementBase, ILauncherItemId
        where TLauncherItemElement : ElementBase, ILauncherItemId
    {
        public LauncherItemWithIconElement(TLauncherItemElement element, LauncherIconElement icon, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            if(element.LauncherItemId != icon.LauncherItemId) {
                throw new ArgumentException(nameof(ILauncherItemId.LauncherItemId));
            }

            Element = element;
            Icon = icon;
        }

        #region property

        public TLauncherItemElement Element { get; }
        public LauncherIconElement Icon { get; }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Element.LauncherItemId;

        #endregion

    }

    public static class LauncherItemWithIconElement
    {
        #region function

        public static LauncherItemWithIconElement<TLauncherItemElement> Create<TLauncherItemElement>(TLauncherItemElement element, LauncherIconElement icon, ILoggerFactory loggerFactory)
            where TLauncherItemElement : ElementBase, ILauncherItemId
        {
            return new LauncherItemWithIconElement<TLauncherItemElement>(element, icon, loggerFactory);
        }

        #endregion
    }
}
