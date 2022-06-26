// <auto-generated>
// [T4] build 2022-05-29 12:33:42Z(UTC)
// </auto-generated>
using System;
using System.Text.Json.Serialization;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    /// <summary>
    /// ランチャーアイテムID。
    /// </summary>
    public readonly record struct LauncherItemId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherItemId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherItemId Empty
        {
            get
            {
                return new LauncherItemId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="LauncherItemId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherItemId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherItemId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="LauncherItemId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static LauncherItemId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherItemId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static LauncherItemId NewId()
        {
            return new LauncherItemId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// 資格情報ID。
    /// </summary>
    public readonly record struct CredentialIdId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public CredentialIdId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static CredentialIdId Empty
        {
            get
            {
                return new CredentialIdId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="CredentialIdId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out CredentialIdId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new CredentialIdId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="CredentialIdId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static CredentialIdId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new CredentialIdId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static CredentialIdId NewId()
        {
            return new CredentialIdId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// ランチャーツールバーID。
    /// </summary>
    public readonly record struct LauncherToolbarId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherToolbarId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherToolbarId Empty
        {
            get
            {
                return new LauncherToolbarId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="LauncherToolbarId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherToolbarId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherToolbarId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="LauncherToolbarId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static LauncherToolbarId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherToolbarId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static LauncherToolbarId NewId()
        {
            return new LauncherToolbarId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// フォントID。
    /// </summary>
    public readonly record struct FontId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public FontId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static FontId Empty
        {
            get
            {
                return new FontId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="FontId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out FontId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new FontId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="FontId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static FontId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new FontId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static FontId NewId()
        {
            return new FontId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// ランチャーグループID。
    /// </summary>
    public readonly record struct LauncherGroupId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public LauncherGroupId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static LauncherGroupId Empty
        {
            get
            {
                return new LauncherGroupId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="LauncherGroupId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out LauncherGroupId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new LauncherGroupId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="LauncherGroupId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static LauncherGroupId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new LauncherGroupId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static LauncherGroupId NewId()
        {
            return new LauncherGroupId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// ノートID。
    /// </summary>
    public readonly record struct NoteId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public NoteId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static NoteId Empty
        {
            get
            {
                return new NoteId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="NoteId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out NoteId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new NoteId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="NoteId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static NoteId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new NoteId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static NoteId NewId()
        {
            return new NoteId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// ノートファイルID。
    /// </summary>
    public readonly record struct NoteFileId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public NoteFileId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static NoteFileId Empty
        {
            get
            {
                return new NoteFileId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="NoteFileId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out NoteFileId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new NoteFileId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="NoteFileId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static NoteFileId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new NoteFileId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static NoteFileId NewId()
        {
            return new NoteFileId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// キーアクションID。
    /// </summary>
    public readonly record struct KeyActionId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public KeyActionId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static KeyActionId Empty
        {
            get
            {
                return new KeyActionId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="KeyActionId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out KeyActionId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new KeyActionId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="KeyActionId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static KeyActionId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new KeyActionId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static KeyActionId NewId()
        {
            return new KeyActionId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// プラグインID。
    /// </summary>
    public readonly record struct PluginId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public PluginId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static PluginId Empty
        {
            get
            {
                return new PluginId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="PluginId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out PluginId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new PluginId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="PluginId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static PluginId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new PluginId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static PluginId NewId()
        {
            return new PluginId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// 通知ログID。
    /// </summary>
    public readonly record struct NotifyLogId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public NotifyLogId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static NotifyLogId Empty
        {
            get
            {
                return new NotifyLogId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="NotifyLogId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out NotifyLogId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new NotifyLogId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="NotifyLogId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static NotifyLogId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new NotifyLogId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static NotifyLogId NewId()
        {
            return new NotifyLogId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
    /// <summary>
    /// スケジュールジョブID。
    /// </summary>
    public readonly record struct ScheduleJobId
    {
        /// <summary>
        /// 生成。
        /// </summary>
        [JsonConstructor]
        public ScheduleJobId(System.Guid id)
        {
            Id = id;
        }

        #region property

        /// <summary>
        /// 生ID。
        /// </summary>
        public System.Guid Id { get; }

        /// <summary>
        /// 空ID。
        /// </summary>
        public static ScheduleJobId Empty
        {
            get
            {
                return new ScheduleJobId(default(System.Guid));
            }
        }

        #endregion

        #region function

        /// <summary>
        /// <see cref="ScheduleJobId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <param name="result">変更成功。</param>
        /// <returns></returns>
        public static  bool TryParse(string s, out ScheduleJobId result)
        {
            if(System.Guid.TryParse(s, out var val)) {
                result = new ScheduleJobId(val);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// <see cref="ScheduleJobId"/>に変換。
        /// </summary>
        /// <param name="s">入力文字列。</param>
        /// <returns></returns>
        public static ScheduleJobId Parse(string s)
        {
            var id = System.Guid.Parse(s);
            return new ScheduleJobId(id);
        }

        /// <summary>
        /// 新規IDの生成。
        /// </summary>
        /// <returns></returns>
        public static ScheduleJobId NewId()
        {
            return new ScheduleJobId(System.Guid.NewGuid());
        }

        #endregion

        #region object

        /// <summary>
        /// IDを文字列表現に変換。
        /// <para>ファイル名として使用可能な文字列表現にもなる。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id.ToString("D");
        }

        #endregion
    }
}


