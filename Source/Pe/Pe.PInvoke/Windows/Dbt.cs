namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum DBT
    {
        /// <summary>
        /// デバイスが使用可能
        /// </summary>
        DBT_DEVICEARRIVAL = 0x8000,
        /// <summary>
        /// 設定変更要求キャンセル
        /// </summary>
        DBT_CONFIGCHANGECANCELED = 0x0019,
        /// <summary>
        /// 設定が変更された
        /// </summary>
        DBT_CONFIGCHANGED = 0x0018,
        /// <summary>
        /// ドライバー定義のカスタムイベント
        /// </summary>
        DBT_CUSTOMEVENT = 0x8006,
        /// <summary>
        /// デバイス停止要求
        /// </summary>
        DBT_DEVICEQUERYREMOVE = 0x8001,
        /// <summary>
        /// デバイス停止要求失敗
        /// </summary>
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002,
        /// <summary>
        /// デバイスが停止
        /// </summary>
        DBT_DEVICEREMOVECOMPLETE = 0x8004,
        /// <summary>
        /// デバイス停止中
        /// </summary>
        DBT_DEVICEREMOVEPENDING = 0x8003,
        /// <summary>
        /// 独自イベント発行
        /// </summary>
        DBT_DEVICETYPESPECIFIC = 0x8005,
        /// <summary>
        /// デバイス状態変更
        /// </summary>
        DBT_DEVNODES_CHANGED = 0x0007,
        /// <summary>
        /// 設定変更要求
        /// </summary>
        DBT_QUERYCHANGECONFIG = 0x0017,
        /// <summary>
        /// なんだろう。ユーザー定義？
        /// </summary>
        DBT_USERDEFINED = 0xFFFF,
    }
}
