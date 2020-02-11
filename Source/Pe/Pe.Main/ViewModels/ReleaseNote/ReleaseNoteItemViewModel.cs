using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ReleaseNote
{
    public class ReleaseNoteTextViewModel : ViewModelBase
    {
        public ReleaseNoteTextViewModel(ReleaseNoteText text, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Text = text;
        }

        #region property

        ReleaseNoteText Text { get; }

        public string Value => Text.Value;
        public ReleaseNoteTextKind Kind => Text.Kind;

        #endregion
    }

    public class ReleaseNoteWordViewModel : ViewModelBase
    {
        public ReleaseNoteWordViewModel(IReadOnlyList<ReleaseNoteTextViewModel> items, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Items = items;
        }

        #region property

        public IReadOnlyList<ReleaseNoteTextViewModel> Items { get; }

        #endregion
    }

    public class ReleaseNoteLogItemViewModel : ViewModelBase
    {
        public ReleaseNoteLogItemViewModel(ReleaseNoteLogItemData log, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Log = log;

            Subject = CreateWord(Log.Subject);
            if(Log.Comments != null && 0 < Log.Comments.Length) {
                Comments = Log.Comments.Select(i => CreateWord(i)).ToList();
            }
        }

        #region property

        ReleaseNoteLogItemData Log { get; }

        public string Revision => Log.Revision;

        public ReleaseNoteLogKind Kind => Log.Kind;

        public ReleaseNoteWordViewModel Subject { get; }
        public IReadOnlyList<ReleaseNoteWordViewModel> Comments { get; } = new List<ReleaseNoteWordViewModel>();

        static Regex WrodRegex = new Regex(@"
(?<HIT_HTTP>
    https?://[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)
)
|
(?<HIT_ISSUE>
    \#\d+
)
        ", RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
        #endregion

        #region function

        ReleaseNoteWordViewModel CreateWord(string s)
        {
            var matches = WrodRegex.Matches(s);
            if(matches.Any()) {
                /*
                foreach(var match in matches.Cast<Match>()) {
                    var matchValue = s.Substring(match.Index, match.Length)
                    new ReleaseNoteTextViewModel(new ReleaseNoteText(, ReleaseNoteTextKind.Plain), LoggerFactory),
                }
                */
            }

            return new ReleaseNoteWordViewModel(new List<ReleaseNoteTextViewModel>() {
                new ReleaseNoteTextViewModel(new ReleaseNoteText(s, ReleaseNoteTextKind.Plain), LoggerFactory),
            }, LoggerFactory);
        }

        #endregion
    }

    public class ReleaseNoteContentViewModel : ViewModelBase
    {
        public ReleaseNoteContentViewModel(ReleaseNoteContentData content, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            Content = content;
            Logs = Content.Logs
                .Select(i => new ReleaseNoteLogItemViewModel(i, LoggerFactory))
                .ToList()
            ;
        }

        #region property

        ReleaseNoteContentData Content { get; }

        public ReleaseNoteContentKind Kind => Content.Kind;
        public IReadOnlyList<ReleaseNoteLogItemViewModel> Logs { get; }

        #endregion
    }

    public class ReleaseNoteItemViewModel : ViewModelBase
    {
        public ReleaseNoteItemViewModel(ReleaseNoteItemData releaseNoteItem, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            Item = releaseNoteItem;
            Contents = Item.Contents
                .Select(i => new ReleaseNoteContentViewModel(i, LoggerFactory))
                .ToList()
            ;
        }

        #region property

        ReleaseNoteItemData Item { get; }

        public DateTime Date => Item.Date;
        public Version Version => Item.Version;

        public IReadOnlyList<ReleaseNoteContentViewModel> Contents { get; }

        #endregion
    }
}
