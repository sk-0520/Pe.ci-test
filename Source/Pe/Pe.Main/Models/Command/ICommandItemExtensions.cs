using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    public static class ICommandItemExtensions
    {
        #region function

        static string Join(IEnumerable<HitValue> values) => string.Join(string.Empty, values.Select(i => i.Value));

        public static string GetHeaderText(this ICommandItem commandItem) => Join(commandItem.HeaderValues);
        public static string GetDescriptionText(this ICommandItem commandItem) => Join(commandItem.DescriptionValues);
        public static string GetExtendDescriptionText(this ICommandItem commandItem) => Join(commandItem.ExtendDescriptionValues);

        #endregion
    }
}
