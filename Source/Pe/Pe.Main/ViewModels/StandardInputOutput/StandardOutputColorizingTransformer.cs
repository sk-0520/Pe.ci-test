using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardOutputColorizingTransformer: DocumentColorizingTransformer
    {
        readonly struct OffsetPair
        {
            public readonly int start;
            public readonly int end;

            public OffsetPair(int start, int end)
            {
                this.start = start;
                this.end = end;
            }
        }

        public StandardOutputColorizingTransformer(Brush foreground)
        {
            Foreground = foreground;
        }

        #region property

        Brush Foreground { get; }

        IList<OffsetPair> OffsetItems { get; } = new List<OffsetPair>(40);

        #endregion

        #region function

        public void Add(int start, int end)
        {
            OffsetItems.Add(new OffsetPair(start, end));
        }

        #endregion


        #region DocumentColorizingTransformer

        protected override void ColorizeLine(DocumentLine line)
        {
            var offset = OffsetItems.FirstOrDefault(i => i.start <= line.Offset && line.EndOffset <= i.end);
            if(offset.start != 0 && offset.end != 0) {
                ChangeLinePart(Math.Max(offset.start, line.Offset), Math.Min(offset.end, line.EndOffset), elm => {
                    elm.TextRunProperties.SetForegroundBrush(Foreground);
                });
            }
        }

        #endregion
    }
}
