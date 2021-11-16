# Pe.Embedded

プラグイン構築処理プロジェクト。

このプロジェクトの各種ソースをリンクして使用する(共有プロジェクト使えりゃなぁ)。

参考実装: `<Pe>/Source/Pe.Plugins.*/*.sln`

## リンク方法

プロジェクトに対して以下を設定する。

この辺はドキュメントのプラグインテンプレートを参照のこと。

```
<ItemGroup>
  <Compile Include="..\..\Pe\Pe.Embedded\**\*.cs" Exclude="..\..\Pe\Pe.Embedded\obj\**\*.cs" />
</ItemGroup>
```



