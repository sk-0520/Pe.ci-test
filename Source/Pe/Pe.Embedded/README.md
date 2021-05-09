# Pe.Embedded

プラグイン構築処理プロジェクト。

このプロジェクトの各種ソースをリンクして使用する(共有プロジェクト使えりゃなぁ)。

参照実装: `<Pe>/Source/Plugins/Plugins.sln`

## リンク方法

プロジェクトに対して以下を設定する。

```
<ItemGroup>
  <Compile Include="..\..\Pe\Pe.Embedded\**\*.cs" Exclude="..\..\Pe\Pe.Embedded\obj\**\*.cs" />
</ItemGroup>
```



