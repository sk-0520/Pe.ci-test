declare function makeChangelogLink(): void;

const changelogs = [
	/*
						"class": "compatibility" "notice" "nuget" "myget" "plugin-compatibility"
						"comments": [
							""
						]
	---------------------------------------------
	*/
	/*
	{
		"date": "YYYY/MM/DD",
		"version": "0.xx.001",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"class": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			}
		]
	},
	*/
	/*--------RELEASE HEAD--------*/
	{
		"date": "YYYY/MM/DD",
		"version": "0.99.014",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"class": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "",
						"subject": "カラーパレット選択時のカーソル・視覚状態を変更"
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "",
						"subject": "#670: ランチャーアイテム自動登録で取り込みボタン連打すると死ぬ💀"
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "",
						"subject": "#668: SQLの実行ログにてスペース破棄とか行番号追加とか、いる？"
					},
					{
						"revision": "",
						"subject": ""
					},
					{
						"revision": "",
						"subject": ""
					}
				]
			}
		]
	},
	{
		"date": "2020/07/09",
		"version": "0.99.013",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "c0ea1fe0bc8eb35e9a44751275fc69c799b015f3",
						"subject": "#671: ノートが自動的に隠れなくなっている "
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "71368427c5f8fc92af3e058deacbb618260e371a",
						"subject": "#667: Visual Studio 2019 Image Library の使用を明記する"
					}
				]
			}
		]
	},
	{
		"date": "2020/07/07",
		"version": "0.99.010",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "2905eecdf7571684c20a7f3d02c987a7a98e7718",
						"subject": "#655: ランチャーアイテム自動登録にてアンインストールファイルと思しきファイル名は登録対象外とする"
					},
					{
						"revision": "f7bef111cad2bad7aa64e118b86f95b10402b9b0",
						"subject": "#662: ランチャーアイテム自動登録で登録時ではなくプレビュー時にショートカットを展開する"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "db840540ea24cc00404149cc3d68c0dfbc5bf23e",
						"subject": "#659: CI 処理時のコミットリビジョン埋め込み処理をスキップしている"
					},
					{
						"revision": "846999268fa3e8a6e774768e3bdf62c9e6ad035c",
						"subject": "#663: ランチャーアイテム自動登録でサブディレクトリが読み込めていない"
					},
					{
						"revision": "8527b589cb5e9d58e372ed2ab4b11d0452568230",
						"subject": "#661: アイコン取得時に基本サイズ以外にDPIスケールも考慮する",
						"comments": [
							"DPI が取れたり取れなかったりのヤケクソ DPI スケール反映",
							"環境によるけど 20px とか 24px とか 40px とかのアイコンをとってくるので対象が該当アイコンサイズを持っていなければ結局ぼけるっていうね"
						]
					},
					{
						"revision": "6bce56ab96d2c491ab4a3eebb3eb6152bec87366",
						"subject": "#634: 設定画面を開く際にやたらめったら時間がかかる",
						"comments": [
							"かなり手を入れたのでバグってたらめんご"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "27afe5cd24dfb4879f9559890ce8cbd2048b8a08",
						"class": "nuget",
						"subject": "CefSharp.Wpf 81.3.100 -> 83.4.20",
						"comments": [
							"WebView処理周りで透過効かなくなってるっぽいなー"
						]
					},
					{
						"revision": "034b6f79c849e418f75685a5757426cc7faaad02",
						"class": "nuget",
						"subject": "SonarAnalyzer.CSharp 8.8.0.18411 -> 8.9.0.19135"
					}
				]
			}
		]
	},
	{
		"date": "2020/07/03",
		"version": "0.99.001",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "60ed1438fab4d615d1f5ac25978eaf74ad3f2c51",
						"subject": "#658: ランチャーアイテム更新間隔にて分が毎分になっている"
					}
				]
			}
		]
	},
	{		"date": "2020/07/02",
		"version": "0.99.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "",
						"subject": "プラグインを実装した",
						"comments": [
							"今のところかなり限定的で気楽に試せるようなものではないけど一区切り",
							"プラグイン共通ライブラリ(Pe.Bridge)バージョンは Pe と連動しない",
							"将来的にはインストール・アンインストールを Pe 側からできるようにしたりする予定",
							"参照実装: <Pe.git>/Source/Plugins"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "b3e624137a3aadebf85cfb0399d35fe6f0578ab7",
						"subject": "#509: プラグイン機構の構築",
						"comments": [
							"まだまだ甘いし達成できてない目標もあるけど実運用しながら機能拡張できるようにしていきたいのでリリース",
							"ドキュメントもまだ全然かけてないのでソースが正。んで頻繁に互換性が失われる想定"
						]
					},
					{
						"revision": "fe77f8d8e95e42df01d492306492506f6b1c04ce",
						"subject": "#550: 定期的にアイコン情報を更新する"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "84d12a472783693eea5fb863bfb9a7ccef461126",
						"subject": "#623: アプリケーション内で使用する Pe アイコンをもうちときれいに表示する"
					},
					{
						"revision": "37f6d64b3b62b9a91d3ec762101a0fffb321896b",
						"subject": "#649: ランチャーアイテム再試行処理のキャンセルが効いていない",
						"comments": [
							"色々試したけどアクティブ→非アクティブを連続するとダメっぽいので初っ端から非アクティブにした",
							"副次的効果として #654 に対応"
						]
					},
					{
						"revision": "37f6d64b3b62b9a91d3ec762101a0fffb321896b",
						"subject": "#654: 通知ログがウィンドウアクティブ状態をまだまだ奪う"
					},
					{
						"revision": "410f71ec484e3bcc3a8de783cc1117c5968cd9e5",
						"subject": "#651: %PATH% から設定されたランチャーアイテムのコンテキストメニューの活性処理で %PATH% を考慮する"
					},
					{
						"revision": "e6ae31d10d40ac86f70f1f584d52c900422eeb08",
						"subject": "#652: バージョン情報表示中はコマンド表示できないようにする",
						"comments": [
							"スタート・設定・バージョン情報を表示した際にフック等の処理を停止するようにした"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "a3503a67c914f7b40e726e2810e29e8144a11022",
						"subject": "#650: 内蔵ブラウザのリソース取得をC#処理からCefSharpで直接行う"
					}
				]
			}
		]
	},
	{
		"date": "2020/06/21",
		"version": "0.98.001",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "パッケージ周りの更新",
						"comments": [
							"プラグイン周り実装を入れたいんだけどアセンブリ周りの解決処理がうまくいかないのでスキップ"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "56ff4b82e73a26ec247688f61583e06381fb89f0",
						"class": "nuget",
						"subject": "MS関係パッケージ更新",
						"comments": [
							"Microsoft.Extensions.Logging.Abstractions 3.1.4 -> 3.1.5",
							"Microsoft.Extensions.Logging 3.1.4 -> 3.1.5",
							"Microsoft.Extensions.Configuration.Json 3.1.4 -> 3.1.5",
							"MSTest.TestAdapter 2.1.1 -> 2.1.2",
							"MSTest.TestFramework 2.1.1 -> 2.1.2"
						]
					},
					{
						"revision": "d53e0ef0f070c5ad029a6780e5cfdda91b4aac9e",
						"class": "nuget",
						"subject": "SonarAnalyzer.CSharp 8.7.0.17535 -> 8.8.0.18411"
					},
					{
						"revision": "efa6c10a3cf595b834f48a69c030827d2c5816a2",
						"class": "nuget",
						"subject": "System.Data.SQLite.Core 1.0.112.2 -> 1.0.113.1"
					},
					{
						"revision": "7e69657f18a645ece0d3de14645ac3c1d812dffd",
						"class": "nuget",
						"subject": "CefSharp.Wpf 79.1.360 -> 81.3.100"
					}
				]
			}
		]
	},
	{
		"date": "2020/05/24",
		"version": "0.98.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "デスクトップPCがぶっ壊れたのでノートPCから意味もなくアップデート",
						"comments": [
							"データのバックアップ大事",
							"全部吹っ飛んだわ。全ドライブ死ぬとかどうなってんの"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "88fc9fcca83f0a88eff7a9ede26e9107870a7677",
						"subject": "#642: フィードバックより ->ランチャーツールバーへのファイルD&D処理の標準挙動",
						"comments": [
							"設定 -> 基本 の「ツールバー」の「ボタンへのD&D」により変更"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "b2cf4bd6c49695df4a83f0469437cab500e7c81c",
						"subject": "#645: 本体コマンド 再起動 死んでるやん！",
						"comments": [
							"#641, #644 との合わせ技で心折れたので #576 の優先度を一つ上げた"
						]
					},
					{
						"revision": "5aa21a3c074d622f689b99281d9b82e8ec3fcd0e",
						"subject": "AppStandardInputOutputSetting.IsTopmost の型が TEXT"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "c814ae8832590d86c1f548828a662b9255773192",
						"class": "nuget",
						"subject": "NLog.Extensions.Logging 1.6.3 -> 1.6.4"
					},
					{
						"revision": "31086ffcf54cc1f4128ef64e38d6e3390ca848be",
						"subject": "強制フル GC 時に LOH をコンパクションするようにした"
					}
				]
			}
		]
	},
	{
		"date": "2020/05/20",
		"version": "0.97.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"subject": "自動バージョンアップ処理不具合(#641)に対応しました",
						"comments": [
							"本バージョンを用いた次回バージョンアップ移行で有効になるため、今までダメだった場合は手動ダウンロードが必要です"
						]
					},
					{
						"revision": "",
						"class": "compatibility",
						"subject": "通常使用の場合影響はしませんがコマンドライン引数の不具合修正により一部挙動が変わる可能性があります",
						"comments": [
							"Pe.exe に対して半角スペースを含むコマンドライン引数を渡した際に、本バージョン以前では最後の一文字が破棄されていました",
							"(前バージョン) Pe.exe --user-data=\"dir path\" -> 'dir pat' と解釈されていた",
							"(本バージョン) Pe.exe --user-data=\"dir path\" -> 'dir path' と解釈される"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "47fa77de01d6bee8697164059e266f40ee0c4a67",
						"subject": "#640: 通知ログがウィンドウアクティブ状態を奪う "
					},
					{
						"revision": "7714338edaf8950ffa295e9d24eaff537d04e7a7",
						"subject": "#641: フィードバックより -> アップデート失敗",
						"comments": [
							"ディレクトリパスに半角スペースが存在する場合に PowerShell の引数・変数が上手く扱えず失敗していた",
							"本体配置ディレクトリのパスに半角スペースが存在する場合はアップデートスクリプトの処理中に異常終了",
							"データ配置ディレクトリのパスに半角スペースが存在する場合はアップデートスクリプトの起動に失敗",
							"コマンドライン引数に半角スペースが存在する場合はアップデートスクリプトの起動に失敗",
							"関連して Pe.exe 処理に半角スペースを含んだコマンドライン引数を渡した場合に Pe.Main.exe に最後の一文字が渡されたない不具合の修正"
						]
					},
					{
						"revision": "0b00c03a07e95f540725affd6b00b5d12acb66e2",
						"subject": "#644: 本体コマンドの再起動処理で本体配置パス・コマンドラインの各種データディレクトリにスペースがあると再起動できない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "128c9b206ae99d52b5679093417d884255a9658c",
						"subject": "#635: デバッグ用初回起動データ構築処理の実装"
					}
				]
			}
		]
	},
	{
		"date": "2020/05/17",
		"version": "0.96.000",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "978d63b4a9b030065d2caf822f57474baa84c59c",
						"subject": "#525: 環境変数編集機能の色付けを行う"
					},
					{
						"revision": "ce97b1eac0e12e2e60e3116c840059658c939dd5",
						"subject": "#627: コマンドで二種類に分かれるアプリケーションコマンドは拡張キーで切り替える"
					},
					{
						"revision": "0502f2ff851b33bf5bd93d49b0cfd16ab0610e7a",
						"subject": "#625: ノートを非表示にした際に元に戻すをサポートする",
						"comments": [
							"以下操作のみを対象とする",
							"Alt + F4",
							"×"
						]
					},
					{
						"revision": "e1e639d6fc5ef47f80a130fb8ea9af24bf1a7acf",
						"subject": "#624: ツールバーを提供UI以外から閉じたときに元に戻すをサポートする",
						"comments": [
							"以下操作のみを対象とする",
							"Alt + F4"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "04afcca9081dafc6d22cd04421f608134b937be0",
						"subject": "#622: 通知領域コンテキストメニューのフック状態の切り替えがチェック反映されていない"
					},
					{
						"revision": "3e32bc99b1aa15e51211b8653d6b669e148388e4",
						"subject": "#530: 通知領域右クリックが死んでるときがある。",
						"comments": [
							"たぶんね、たぶん",
							"ダメだったら起票します。。。"
						]
					},
					{
						"revision": "37351d1f96aa86376d04cf3eeb1082c50cc8dc41",
						"subject": "#617: 本体設定完了時にランチャーアイテムのアイコンキャッシュが全部削除される既知の問題",
						"comments": [
							"調査の結果ランチャーアイテム変更時にも発生していた模様"
						]
					},
					{
						"revision": "7af32665d4d73e54f5904325285142bf1f6b8293",
						"subject": "#626: ツールバーのハンバーガーメニュー表示をフェードさせる"
					},
					{
						"revision": "f1e459de74bf544605ba4661225e4f5c569476cd",
						"subject": "#633: ランチャーグループ名に _ が存在するとアクセスキー扱いとなっている"
					},
					{
						"revision": "3cb441c7b30a875e0e74730b34ea877d6e99b5b6",
						"subject": "#636: 通知ログがカーソル位置指定で通知ログウィンドウにクリック可能なアイテムがある場合は常時追従してはいけない"
					},
					{
						"revision": "b288e997badaa01455dc56e66e249b6e6f0cf9a3",
						"subject": "#628: 出来立てほやほやのノート位置情報が保存されていない"
					},
					{
						"revision": "47f291f81e56c95f4b513a9fe559925aa6981b80",
						"subject": "#638: コマンド検索時の0件ヒット文字列表記をまともにする"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "2aadad27e1afc6e0c14952f635fc9eb970a5540a",
						"class": "nuget",
						"subject": "NLog.Extensions.Logging 1.6.2 -> 1.6.3"
					},
					{
						"revision": "f38039737ddecb610979d34a85a622e8d20189c9",
						"class": "nuget",
						"subject": "MS関係パッケージ更新",
						"comments": [
							"Microsoft.Extensions.Logging 3.1.3 -> 3.1.4",
							"Microsoft.Extensions.Logging.Abstractions 3.1.3 -> 3.1.4",
							"Microsoft.Extensions.Configuration.Json 3.1.3 -> 3.1.4",
							"System.Text.Encoding.CodePages 4.7.0 -> 4.7.1"
						]
					},
					{
						"revision": "cd6e3c97ec8b68c26eafc8167deac3f4adfdd33f",
						"subject": "コマンドウィンドウにデバッグ・β版印を付与"
					},
					{
						"revision": "e23aa0c880f0699da0ed8ad5c56b37dea8da6443",
						"subject": "#620: Clr Heap Allocation Analyzer を VS 拡張機能から Nuget に移し替える"
					},
					{
						"revision": "b9035485416401d075b05fb5c82b5f154939ac89",
						"subject": "SonarAnalyzer.CSharp の導入"
					},
					{
						"revision": "01ee026a1a40917c8915a93c54da7ec155b4aa6a",
						"subject": "#637: 更新履歴の元ファイルがでかすぎるので分割したい"
					}
				]
			}
		]
	},
	{
		"date": "2020/05/09",
		"version": "0.95.000",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "6f2f4f6729441b57ce3e6fe5963bfd8e0bdd9d98",
						"subject": "#603: マウスクリックでキーボード入力待機を解除する"
					},
					{
						"revision": "93b3df1af98152dc7b87f8104ddbd2b156ef7ae0",
						"subject": "#531: 本体用特別コマンドの実装",
						"comments": [
							"コマンド入力時に先頭が「.」の場合に本体用コマンドとして扱うようにした"
						]
					},
					{
						"revision": "8a646840355b1d79f6957d6753809bc703a033c3",
						"subject": "#613: ノート内でタブを入力できるようにする"
					},
					{
						"revision": "d05797710abb575c8c141cc8a328a2716ae0e66e",
						"subject": "#602: キーボード設定をキー入力から行えるようにする"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "76d8addc5d42c0417694c18696836355f784433f",
						"subject": "#601: コマンド型ランチャーの横幅が保存されてない",
						"comments": [
							"正確には保存されてたんだけど保存値がちょっと頭おかしかった"
						]
					},
					{
						"revision": "3e03ec1a35538ed9b12f4a39a5891011f159d7f4",
						"subject": "#607: ヘルプのメニュースクロール位置がリンク遷移時にリセットされる"
					},
					{
						"revision": "9862776bd0a0c63cdce79cdd5f1ca0ad0f625694",
						"subject": "#610: アイコンのあるコントロール系UIが二重のタブ移動対象になっている"
					},
					{
						"revision": "fc09c9b8e1f0a6ef91a19b1aef17587d4ac73023",
						"subject": "#604: 文言をもうちっと分かり易くする"
					},
					{
						"revision": "b7e7e86052a226c9ce8c9fa22ebdb0438338fdb9",
						"subject": "#606: 毎月1日のクッソしょうもないアイコン切り替えが常時稼働状態だと切り替わらない"
					},
					{
						"revision": "c59fdf04ac39dd4000d04bc673df0a5538147f72",
						"subject": "#614: ランチャーアイコンが保存されていない疑惑"
					},
					{
						"revision": "86b7817b0f649fb5ecbdafb58c9a097a37405558",
						"subject": "#615: 本体ディレクトリ読み込み時に不要なディレクトリが作成される"
					},
					{
						"revision": "60119f359108eb67945ec06d68ff8929a618f50d",
						"subject": "#605: ランチャーアイテム修正時にコマンド型ランチャーに即時反映されない"
					},
					{
						"revision": "c2bedc4ae10af111a0b1e59e676ab2a98efba739",
						"subject": "アイコン制御処理SQLが上手くいってなかった"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "ea659d24ad4f329f09fc15b713e10a5aee0106fc",
						"subject": "フック処理の登録処理を初期化から若干ずらした"
					},
					{
						"revision": "ee01edf5db43db7d66f233f7361bbb42feda86a4",
						"subject": "#608: UserControl のバインド周りを調整"
					},
					{
						"revision": "8f0bc8a95e3cc8b57121c108f026a45cb897b81a",
						"class": "nuget",
						"subject": "System.Data.SQLite.Core: 1.0.112.1 -> 1.0.112.2"
					},
					{
						"revision": "4b11f25409c3a27f462e8895993cad8302e49340",
						"subject": "#584: 0.95.000 公開時時に 0.83.0-0.90.000 からのアップデートサポートを破棄"
					},
					{
						"revision": "60e254596d40fc626f698d8e0bbb9044c959d876",
						"subject": "#616: Dao と内部実装SQL読み込み処理に対する事故防止対策委員会"
					},
					{
						"revision": "518650078c359bfabe454b7db2e2cb32cb850b28",
						"subject": "過去バージョンはもう tag から適当に再現してくれ"
					}
				]
			}
		]
	}
];/*--------RELEASE TAIL--------*/

/*--------BUILD-EMBEDDED-JSON--------*/

window.addEventListener('load', () => {
	const changelogTypeMap: { [key: string]: string } = {
		'features': '機能',
		'fixes': '修正',
		'developer': '開発',
		'note': 'メモ'
	};

	const root = document.getElementById('changelogs')!;
	for (const changelog of changelogs) {
		const versionSection = document.createElement('section');

		const versionHeader = document.createElement('h2');
		versionHeader.textContent = `${changelog['date']}, ${changelog['version']}`;
		versionSection.appendChild(versionHeader);

		for (const content of changelog['contents']) {
			const contentSection = document.createElement('section');

			const contentHeader = document.createElement('h3');
			contentHeader.className = content['type'];
			contentHeader.textContent = changelogTypeMap[content['type']];

			contentSection.appendChild(contentHeader);

			const logs = document.createElement('ul');
			for (const log of content['logs']) {
				const item = document.createElement('li');
				if ('class' in log) {
					item.className = log['class']!;
				}

				const header = document.createElement('span');
				header.className = 'header';

				const subject = document.createElement('span');
				subject.textContent = log['subject'];
				header.appendChild(subject);


				if ('revision' in log) {
					if (log['revision']) {
						const revision = document.createElement('a');
						revision.className = 'revision';
						revision.textContent = log['revision']!;
						header.appendChild(revision);
					}
				}


				item.appendChild(header)

				if ('comments' in log) {
					const comments = document.createElement('ul');
					for (const comment of log['comments']!) {
						const li = document.createElement('li');
						li.textContent = comment;
						comments.appendChild(li);
					}
					item.appendChild(comments);
				}

				logs.appendChild(item);
			}
			contentSection.appendChild(logs);

			versionSection.appendChild(contentSection);
		}

		root.appendChild(versionSection);
	}

	makeChangelogLink();
});
