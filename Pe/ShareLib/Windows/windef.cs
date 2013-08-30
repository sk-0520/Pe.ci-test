/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/30
 * 時刻: 23:49
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace ShareLib
{
[StructLayout(LayoutKind.Sequential)]
 public struct POINT {
   public int x;
   public int y;
 }

 [StructLayout(LayoutKind.Sequential)]
 public struct RECT {
   public int left;
   public int top;
   public int right;
   public int bottom;
 }
}
