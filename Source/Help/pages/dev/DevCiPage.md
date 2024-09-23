リリース処理等は CI 環境にて実施する。

CI/CD の一括処理試験は ci-test ブランチにて実施すること(Pe リポジトリでは ci-test ブランチの読み書きを禁止している)。

# GitHub/GitHub Actions

## GitHub

特に設定する必要なし

## GitHub Actions

特に設定する必要なし

> [!WARNING]
> 特に設定なしって書いてるけど書き込み権限が必要
>
> 将来的に何とかする

# GitHub での CI 試験

ブランチ ci-test で処理。

リリースタグ系が面倒なことになるので新たなリポジトリを作った方が安全。
