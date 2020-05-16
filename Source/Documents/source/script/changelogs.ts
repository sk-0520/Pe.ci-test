declare function makeChangelogLink(): void;

const changelogs = [
	/*
						"class": "compatibility" "notice" "nuget" "myget"
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
		"version": "0.95.001",
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
						"subject": "#525: 環境変数編集機能の色付けを行う"
					},
					{
						"revision": "",
						"subject": "#627: コマンドで二種類に分かれるアプリケーションコマンドは拡張キーで切り替える"
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
						"subject": "#622: 通知領域コンテキストメニューのフック状態の切り替えがチェック反映されていない"
					},
					{
						"revision": "",
						"subject": "#530: 通知領域右クリックが死んでるときがある。",
						"comments": [
							"たぶんね、たぶん",
							"ダメだったら起票します。。。"
						]
					},
					{
						"revision": "",
						"subject": "#617: 本体設定完了時にランチャーアイテムのアイコンキャッシュが全部削除される既知の問題",
						"comments": [
							"調査の結果ランチャーアイテム変更時にも発生していた模様"
						]
					},
					{
						"revision": "",
						"subject": "#626: ツールバーのハンバーガーメニュー表示をフェードさせる"
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
						"class": "nuget",
						"subject": "NLog.Extensions.Logging 1.6.2 -> 1.6.3"
					},
					{
						"revision": "",
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
						"revision": "",
						"subject": "コマンドウィンドウにデバッグ・β版印を付与"
					},
					{
						"revision": "",
						"subject": "#620: Clr Heap Allocation Analyzer を VS 拡張機能から Nuget に移し替える"
					},
					{
						"revision": "",
						"subject": "SonarAnalyzer.CSharp の導入"
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
	},
	{
		"date": "2020/04/26",
		"version": "0.94.000",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "cd37c118ca03deac74f2d1209b34e2f9fc8d6373",
						"subject": "#593: 通知用UIの作成"
					},
					{
						"revision": "ca412643b2736bd7516fd54e74d62ee9a396a42b",
						"subject": "#592: 起動失敗アイテムを頑張って起動させる"
					},
					{
						"revision": "257ffd2248c278c8b20213174213c3220e9c5105",
						"subject": "#591: ノートの内容を時間経過で非表示にするとか視認性を悪くする"
					},
					{
						"revision": "770ddd944acf0b890984182408fd86b96e71be60",
						"subject": "#507: キーボード入力待ちの通知を行う"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "596bd618c738a3d4e72d0fb0064748066472fe1d",
						"subject": "#566: 設定でランチャーアイテムからアイテムを削除した場合にグループ内に該当ランチャーアイテムが残ってる"
					},
					{
						"revision": "55720ff987110c7ffc911a3adc546ca51d4ebf99",
						"subject": "#594: 初回仕様バージョンの記録が 0.84.0 固定になってる"
					},
					{
						"revision": "",
						"subject": "#595: クラッシュレポートの云々ってなんやねん"
					},
					{
						"revision": "af523ee7a9269ea3e926116b65731e9015d08aef",
						"subject": "型名設定できてなかった",
						"comments": [
							"型変更に table 作り直ししか手がなさそうなので一応初期構築には正しい型を設定したうえで、既存は無視する",
							"出来んことはなさそうだけど手間がかかるので気が向いたら何とかしてみる"
						]
					},
					{
						"revision": "e5a16c180de56c3d56c4787e616ac7791b36af5d",
						"subject": "#596: 実行回数記録されてなくない？"
					},
					{
						"revision": "47d0cc577b7406ce4d7e6399f3ec8e5d0fe1d992",
						"subject": "#598: ツールバーで登録したてのアイテムを編集したら死ぬ疑惑"
					},
					{
						"revision": "717833705217355c2cfd79647ec8382ef5b54194",
						"subject": "#600: 初回起動時に作成される表のうち型指定していないものがある"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "112bcd3a2badbc13afc0f1583fc9c9d4bf87b523",
						"subject": "#569: @appsettings.debug.json 消していいでしょ"
					},
					{
						"revision": "e544ddd1b040286eaf0dab17fa5ceb1667ad09a4",
						"class": "nuget",
						"subject": "Dapper 2.0.30 -> 2.0.35"
					},
					{
						"revision": "e544ddd1b040286eaf0dab17fa5ceb1667ad09a4",
						"class": "nuget",
						"subject": "System.Data.SQLite.Core 1.0.112 -> 1.0.112.1"
					},
					{
						"revision": "350e11430b8e0c7665d630fce7957e057210b5ed",
						"subject": "#597: CURRENT_TIMESTAMP を使わずにアプリ側から時刻を設定する"
					}
				]
			}
		]
	},
	{
		"date": "2020/03/29",
		"version": "0.93.000",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "a82b25c30e9cd2a702590408b71d9dfb3da5c195",
						"subject": "標準入出力死んでるやん！"
					},
					{
						"revision": "ec1b8821431a945b0b9eb9da894e5ced9aec0580",
						"subject": "#589: ヘルプドキュメントのメニュー部が使いにくい"
					},
					{
						"revision": "fc9cb7ba7427b6569c3274ee4a6fa1b75c0c1e94",
						"subject": "#590: フィードバックのプレビューでインターネット世界に旅立てる"
					},
					{
						"revision": "52aa2db9d89786a503100b46d4caaf85c8baa156",
						"subject": "#587: 非実行可能アイテムを指定して実行で標準入出力を受け取ると死ぬ"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "d2118258ba9baadc7d3130b7a8a98b95728b3eb4",
						"subject": "#493: DI に名前付きがほしい",
						"comments": [
							"恐らく使うことはない"
						]
					},
					{
						"revision": "02306f920cce8f2357205f1202fd97651eb2e2bb",
						"class": "nuget",
						"subject": "Microsoft.Extensions.* 更新",
						"comments": [
							"Microsoft.Extensions.Configuration.Json 3.1.2 -> 3.1.3",
							"Microsoft.Extensions.Logging 3.1.2 -> 3.1.3",
							"Microsoft.Extensions.Logging.Abstractions 3.1.2 -> 3.1.3"
						]
					}
				]
			}
		]
	},
	{
		"date": "2020/03/22",
		"version": "0.92.000",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "0832fd551d021da3c16ad13b5aa5d7cc2cb9199c",
						"subject": "#503: 無言で死ぬのを何とかする",
						"comments": [
							"アプリケーション側で検知可能な未処理例外に対してクラッシュレポートを表示",
							"MnMn でやってたようなことを整理して再実装",
							"クラッシュ時に生データを出力してクラッシュレポート側で再調整みたいな感じ"
						]
					},
					{
						"revision": "1672c0723532c8f36b677d077fd0bd390bf3d0dd",
						"subject": "#506: フィードバック機能の再実装"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "8ad089ea862928c798376538b918e88d3a1d5f71",
						"subject": "#588: ディスプレイ設定変更後に強制終了する時がある"
					},
					{
						"revision": "2591d4ae145b4ed6250aaa68a380cdb4039a2a74",
						"subject": "ヘルプのコマンドライン引数 app-log-limit の説明書きが値無しになっていたのを修正"
					},
					{
						"revision": "072b8b618f454e7f280d53146ef878215eb61da0",
						"subject": "#586: ログが二重に出力されている"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "5fecb71fefb2f389dfee63943765cf5dc1e5901d",
						"subject": "#574: 0.92.0 公開時に 0.91.0 以上をアップデート可能対象にする"
					},
					{
						"revision": "dd8c64fdf09c3d03121a2b27ba1d9e520a598444",
						"class": "nuget",
						"subject": "NLog.Extensions.Logging 1.6.1 -> 1.6.2"
					}
				]
			}
		]
	},
	{
		"date": "2020/03/13",
		"version": "0.91.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "バージョン 0.83.0 以下のデータコンバート処理を破棄しました"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "1e6f0af99d9a25d6eaa7faeba1b5c32f5fcaeaa3",
						"subject": "#579: ノートの書式付き操作ツールバーを操作メニュー側に一元化する"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "2b1005f6965629112d504a353dc1c7711b3eab80",
						"subject": "#572: アップデートスクリプトのアップデート前後処理スクリプトが既存スクリプトを使用している",
						"comments": [
							"関連: #574: 0.92.0 公開時に 0.91.0 以上をアップデート可能対象にする"
						]
					},
					{
						"revision": "21348cd12d35fcde3634a7431eb91ef22c8047b8",
						"subject": "ノートの種別変更で同一種別に変更しようとすると内部的に例外が飛んでいた事象の修正"
					},
					{
						"revision": "de0be7a47b4e1feb602a01fceb7c2507354e6e56",
						"subject": "#551: もしかしてだけど Pe から Pe.Main に --wait が飛んでる？"
					},
					{
						"revision": "4dffd989953d6b9ab889def88c42011e5e1c15b0",
						"subject": "#554: アクセントカラーがなーんかまだ透明っぽいときがある"
					},
					{
						"revision": "4dffd989953d6b9ab889def88c42011e5e1c15b0",
						"subject": "#575: テキスト <-> 書式付きの変換処理で改行が取り払われる"
					},
					{
						"revision": "7b56a24a5a74e5b1878257bf1f5b9fcd7a5786d5",
						"subject": "エクスプローラ補正のキャッシュ数指定が 0 になってる不具合修正"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "a201feb9f96bb41fb6925155e11673ea304b77fa",
						"subject": "#573: 自前でプロパティ変更通知している処理の高速化",
						"comments": [
							"これまた遅くなった気がしないでもないけど気にしない"
						]
					},
					{
						"revision": "b947c6b31734dd1b4031db3c8f27c290bb0fca8b",
						"subject": "#570: Dispatcher.Invoke を滅ぼしましょう"
					},
					{
						"revision": "a0823b0101df750fc0b2c8424c2352368f0f6d0e",
						"subject": "#543: 0.83.0 からのデータ変換処理を破棄する"
					},
					{
						"revision": "03bde067f136480e03031e4583bc3b64a27c2526",
						"subject": "#582: 本体内部にログの一部を保持する"
					},
					{
						"revision": "e401a6afd0ab769e319ac60cf6d210bdab852e0b",
						"subject": "#578 ノートの「書式付き」をリッチテキストに変更する "
					}
				]
			}
		]
	},
	{
		"date": "2020/03/08",
		"version": "0.90.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "notice",
						"subject": "バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "ded9c606ccc1394846f2b813ddfdd455a9ffa2f9",
						"subject": "#562: コマンド型ランチャーを ESC で閉じる"
					},
					{
						"revision": "6e771610a6d0f5f2188615424c8cd1100996dc16",
						"subject": "#487: デプロイ時にSQLファイルを一つにまとめる",
						"comments": [
							"思ってたより意味がないというか若干遅くなったけど気にしない"
						]
					},
					{
						"revision": "6449c91f3235d08b042dd58fe197bd3f563991b6",
						"subject": "#508: ノートの書式付きを操作できるようにする",
						"comments": [
							"本課題の副産物としてカラーピッカーに既定の色を追加"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "d582c63e3b89505e729af9113de102e6c9b7c1a0",
						"subject": "#563: コマンドランチャーで 1 文字だけの入力だと初期選択が行われない場合がある"
					},
					{
						"revision": "3f959aa37dc4d384ab9e0f183033a5318260ce63",
						"subject": "#564: RDP 復帰後に落ちることがある"
					},
					{
						"revision": "f9bb81f4a6590cf1087acfed9448162e40797079",
						"subject": "書式付きノートを最小化/元に戻すをガチャガチャした場合に落ちるの対応"
					},
					{
						"revision": "35a1221d36edfd22f4f6617f85e10742d6cbe56a",
						"subject": "#558: 標準入出力のタイトルバーがアイテム名じゃなくてパスになってる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "29a41075eb122c509d9823f78e08d657d6ae868c",
						"subject": "#542: ログ出力周りの整理"
					},
					{
						"revision": "ab435c7f58ea01c50b3c8d065996debf118c3360",
						"subject": "#557: 絶対に動的じゃないとダメでない SQL はファイルとして外に出す"
					},
					{
						"revision": "7135ae9fcc8cdc451a7615fb8f591ed25ce30263",
						"subject": "#567: アプリケーション構成ファイルをバージョン専用で使用できるようにする"
					},
					{
						"revision": "a5926acff7ddca674dc5321387c13b5a134683dd",
						"subject": "#568: デバッグ時のノートがデバッグ用として見てわかるようにする"
					},
					{
						"revision": "9f3def2684720169010fe6a637eb57520ac95fc6",
						"subject": "#565: コマンドライン引数のドキュメントを作成する"
					}
				]
			}
		]
	},
	{
		"date": "2020/03/04",
		"version": "0.89.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "notice",
						"subject": "バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "1c3d38a7864df733511166519d1b1653b2138e3e",
						"subject": "#524: コマンド入力のスコア評価を改善する"
					},
					{
						"revision": "9f8bf74cc6115c185605976e0db6df247f9e9a1e",
						"subject": "#502: コマンドランチャーで待機時間は不要",
						"comments": [
							"待機時間をなくすとともに列挙したアイテムの表示処理を改善"
						]
					},
					{
						"revision": "3abcddddb765cc3518a782b9605d5fac8e711b83",
						"subject": "#556: マウスの戻る・進むボタンでグループの切り替えを行う"
					},
					{
						"revision": "b009fecc49f444949c526768a353c3f292a1fbb4",
						"subject": "#548: 自動的に隠すツールバーを強制的に隠した場合の表示条件該当に制限を入れる"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "5b5f2cd74188eb8eb78807d201cd547e8c8514e5",
						"subject": "#544: コマンド型ランチャー入力時の色設定が完全にデバッグ用なので適当にいい感じにする"
					},
					{
						"revision": "75a38b0cb9ff06646627c162a8013bd8743e04e6",
						"subject": "#522: ツールバーの初期グループ選択設定が未選択の場合に選択されているものとして扱われる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "81ea3b586a491bb96bdab6d328de08ebca402980",
						"subject": "#518: 配布形式を 7z にする"
					},
					{
						"revision": "851425c7aacab4dce068424b636da3df09b9c95c",
						"subject": "#545: リリースビルド処理で node_modules のキャッシュは外す"
					},
					{
						"revision": "b5cfd2810b064bbd244684fe2e834643b87c8d28",
						"subject": "#555: Pe の所有しているディレクトリに対しては安全にアクセスできる仕組みを作る"
					},
					{
						"revision": "bcfcb271b5eb08179f5c8e03dab9d598e86eeac2",
						"subject": "#552: メインアイコンを 3 つも持つ必要なくない？",
						"comments": [
							"CI ビルド時に切り替えるようにして *.ico 自体はリポジトリ管理にした"
						]
					},
					{
						"revision": "dcf88dd4ef9a72558923cbc03e948dfad93907e3",
						"subject": "#547: デプロイ処理の対象サービスでアーカイブ配布先とタグ付けが一緒になってる"
					}
				]
			}
		]
	},
	{
		"date": "2020/03/01",
		"version": "0.88.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "notice",
						"subject": "バージョン 0.91.000 で 0.83.0 のデータコンバート処理を破棄します"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "a5d5c96f6a86b1c31364c783e738111d4879eaf1",
						"subject": "#534: ネットワークドライブのコードが取得できない"
					},
					{
						"revision": "05bb0111d70b6ad17b69c0fecbaf0e70f5cb2013",
						"subject": "#540: 特に指定のないアイテムの並び順が謎極まる"
					},
					{
						"revision": "1c73b4f1fd4f649d5db4ce8ffcd64d36c36b098b",
						"subject": "#541: アイコン表示に失敗すると連鎖的に全部失敗してる感"
					},
					{
						"revision": "89ed09c7567367b7559dc1697e87b3dc1193cc00",
						"subject": "#538: ネットワーク接続時のキャッシュ暫定回避をきちんと対応する"
					}
				]
			}
		]
	},
	{
		"date": "2020/02/29",
		"version": "0.87.001",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"class": "",
						"subject": "起動条件によりアップデートに失敗することが分かったので緊急リリースです",
						"comments": [
							"コマンドライン引数なしで起動した場合にアップデートスクリプトが実行できないのでダミーで引数を付けるか、<Pe>\\bat\\safe.bat で一時的に起動するか、手動でアップデートが必要です"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "4f34892038d792af8c3b114a35861455d93541d3",
						"subject": "#539: コマンドライン引数無しで実行した EXE のアップデートが行えない",
						"comments": [
							"ずーっと --log 付きで実行してたから全然検知できていなかった"
						]
					}
				]
			}
		]
	},
	{
		"date": "2020/02/29",
		"version": "0.87.000",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "140fdfe6e90055d9e4101d2455555d5a11a3dac9",
						"subject": "#532: 設定画面で落ちる"
					},
					{
						"revision": "430861596a61385a4ea68ca3d337d5fa50dc6358",
						"subject": "#535: RDP接続で落ちる",
						"comments": [
							"解像度変更が主に死んでる",
							"ある程度は改善できたと思う。思う。。。"
						]
					},
					{
						"revision": "81813b0243611a62d8d1b33a71fd0d66874bf8d5",
						"subject": "#519: システムのアクセントカラーが透明なときがある"
					},
					{
						"revision": "aa339950fbef98ea0bbc9203956ae2f299a9cf6c",
						"subject": "#523: 内臓ブラウザでリンクを標準ブラウザで開く制限ページにも関わらずホイールクリックによりインターネット世界に羽ばたいていく"
					},
					{
						"revision": "c3ab878b1a8c9ed4510483d1d9459e7558b22016",
						"subject": "#537: 設定画面の「ユーザーIDと使用統計情報」のリンクが完全に置物"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "31ed7edf55b90ffa90b9c2c11b029e7f061c4c44",
						"subject": "#517: CLR って v4.0.30319 でいいの？",
						"comments": [
							"修正ついでに長い情報に RuntimeInformation を追加"
						]
					},
					{
						"revision": "b6fd37ea44b7e024225c0a279cbff1627a7fca51",
						"subject": "#533: Microsoft Visual C++ 再頒布可能パッケージは Pe.Main プロジェクトから除外する"
					},
					{
						"revision": "a7e58267cb21809f5ec906907d1850d257e3a5bf",
						"subject": "#501: 過去バージョンのダウンロード先を明記する"
					}
				]
			}
		]
	},
	{
		"date": "2020/02/26",
		"version": "0.86.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "🙇 32bit 版は手動でアップデートしてください🙇",
						"comments": [
							"大きめの不具合だし早めにリリース"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "f19f850b0adfab1627789468bb3b12c701257543",
						"subject": "#512: スタートアップ登録時に引数も登録できるようにする"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "626553bfb5727d6f2e31b3e67a9fb02fad052867",
						"subject": "#526: アップデート時に PowerShell が実行できない",
						"comments": [
							"32bit 版で実行できなかった",
							"x86,x64 のみを受け付けるようにしていたところを x32,x64 を受け付けるようにしていて x86 を渡していたから死んだ",
							"x32 て。。。"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "77c3761e3a307b6c82d55b27a9b7849c45b3795d",
						"subject": "クッソしょうもないお絵かきが楽しい年ごろ"
					},
					{
						"revision": "260ccf574072d8e539ca509a18078ebfb31a6051",
						"subject": "VMからテーマUI要素をごにょごにょするところはなんも考えなくていいはず"
					}
				]
			}
		]
	},
	{
		"date": "2020/02/26",
		"version": "0.85.000",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "98575402e5956db442cff82752bd3d344ca0e1f3",
						"subject": "#504: ヘルプファイルの再作成 "
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "af68ed61adea4e411c79df3d666c7d92fa9d7715",
						"subject": "#514: 初回起動時に ArgumentNullException で落ちる"
					},
					{
						"revision": "90ab38bbefc98a8285cf00e6c83f9bcf623e8a10",
						"subject": "#516: Microsoft Visual C++ 再頒布可能パッケージ のインストールを不要にする",
						"comments": [
							"対応として再頒布可能パッケージを同梱し、 Pe.exe (PeMain.exe) 起動時に PATH に <Pe>\\bin\\lib\\Redist.MSVC.CRT\\<CPU> を追加するようにした",
							"インストールされてればそれを使用するし、インストールされてなければ同梱版が使われるのでたぶん大丈夫",
							"たぶん Windows 10 なら問題ないと思うんだけどクリーン環境で試してなくて、未サポートの Windows 7 環境で試したから根本的に何か間違ってるかも"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "b35a7e67468c60f30d5650487180c1f47e83856a",
						"subject": "CIのバッジが開発用向いてた"
					},
					{
						"revision": "af68ed61adea4e411c79df3d666c7d92fa9d7715",
						"subject": "#515: CefSharp を使用するために必要な要件をきちんと調べる"
					}
				]
			}
		]
	},
	{
		"date": "2020/02/24",
		"version": "0.84.000",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "78ce8c309c5efc1e586fc209560063734094b792",
						"subject": "#484: 設計練り直して作り直し",
						"comments": [
							"今回の主要アップデートで他のは付随してきただけの課題です",
							"色々足りないけど自動更新機能が動けば何とかなる思い"
						]
					},
					{
						"subject": "根っこからめっちゃくちゃ実装を変えました",
						"comments": [
							".NET Framework から .NET Core に移行したので環境依存に関する制限がある程度なくなりました",
							"内蔵ブラウザを .NET Core(Forms, WPF) のシステム依存から Chromium 実装の CefSharp に切り替えました",
							"ただしこれらの対応によりファイルサイズがすっごい大きくなっています(100MB超)"
						]
					},
					{
						"class": "compatibility",
						"subject": "実装変更に伴い互換性が結構なくなります",
						"comments": [
							"Windows 10 を主軸にしたことで隠しファイル・拡張子切り替え機能の廃止(Explorerですぐに実施できるため)",
							"-> Windows7 では動きません(CefSharp が死ぬ)",
							"ノートの「書式付き」は現状操作できません",
							"グループ選択とランチャーアイテムコンテキストメニューを統合(ここに至るまで色々あったが全部忘れたものとする)",
							"標準入出力ウィンドウの機能を削りました。時間あるときに戻します",
							"コマンドラインオプションの互換性を破棄しました",
							"ランチャーアイテムの種別 コマンド を破棄しました",
							"実行形式の配置場所・名称等の関係の互換性を破棄(.NET Core 移行に伴う bin ディレクトリのわちゃわちゃ感)",
							"ユーザー情報送信機能は手が足りなかったので確認はしますが機能しません",
							"テンプレート機能を廃止しました",
							"Windows がクリップボード頑張っているのでクリップボード機能を廃止しました",
							"ランチャーウィンドウのフロート表示を破棄しました",
							"ログウィンドウを破棄しました",
							"ヘルプドキュメントは作ってる途中です"
						]
					},
					{
						"class": "compatibility",
						"subject": "0.83.4 からライセンスを GPL 3 から WTFPL 2 に変更します",
						"comments": [
							"0.83.0 以下は GPL 3 扱いでリポジトリから取得可能です"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "#459: アップデート処理の実装周りを整理"
					},
					{
						"subject": "#485: 高 DPI 対応"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#472: ノートの斜め方向リサイズ領域を広げる"
					},
					{
						"subject": "#469: 自動的に隠す状態のツールバーが云々"
					},
					{
						"subject": "#452: ツールバーが自動的に隠れる設定でアイコンが非表示になる"
					},
					{
						"subject": "#439: グループ名変更時に変更用入力UIの位置が変"
					},
					{
						"subject": "#425: 一意に識別される設定項目はその一意な値を表示する"
					},
					{
						"subject": "#417: 列挙体の保存値を数値から名称にする"
					},
					{
						"subject": "#380: ランチャーアイテムがネットワーク越しのファイルだとアホみたいに遅い"
					},
					{
						"subject": "#369: ノートのタイトルバーについてるボタンをもうちょっと見栄え良くする"
					},
					{
						"subject": "#313: 四辺に配置したツールバーをシステムメニューから移動すると大変なことになる"
					},
					{
						"subject": "#300: メッセージボックスがダサい"
					},
					{
						"subject": "#112: HTMLレンダリングコンポーネントを変えたい",
						"comments": [
							"CefSharp に全権委任"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "dfed410e2ec4a9bcc637bdefa5f4cf94ba482287",
						"subject": "#480: myget: SharedLibrary から Pe 限定の処理を抜き出し ",
						"comments": [
							"更新履歴には一応乗せるけど完全に死に項目"
						]
					}
				]
			}
		]
	},
	{
		"date": "2017/06/11",
		"version": "0.83.0",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "8dea8d185cd900776b76aaa37b68d2905aa8f75b",
						"subject": "#482: 完全透明状態は設定できないようにする"
					},
					{
						"revision": "99ad70bbff987819e7a185004915229d5f745f58",
						"subject": "#483: コマンドのパスが不正な際にツールバーからアイテムメニューを開くと落ちる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "",
						"subject": "nuget, myget 周りアップデート"
					}
				]
			}
		]
	},
	{
		"date": "2016/12/31",
		"version": "0.82.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"subject": "0.82.0 は 0.82.1 に統合",
						"comments": [
							"ミス大杉"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "26d84b3286f16713a3b2e2c70a50a8e5c55755f6",
						"subject": "#481: セッション終了時にデスクトップサイズをシステムに返却する",
						"comments": [
							"セッション終了時にツールバーを一旦破棄するようにした"
						]
					},
					{
						"revision": "2cc7141f145df536031a3923e0e6f37251d5be1b",
						"subject": "#437: windows10でツールバーの色をwindowsの設定に合わせる",
						"comments": [
							"レジストリ調べきってないので追従できてない",
							"とりあえず Windows10 でツールバーが透明になる問題に対応が主、追々別課題でまた対応する"
						]
					},
					{
						"revision": "5fca089afbc10a5953ff96bf09404d881440c180",
						"subject": "#479: クリップボード取り込み時に落ちる",
						"comments": [
							"再現できず。とりあえず lock で逃げる"
						]
					},
					{
						"revision": "7e6ebc583dd7e4736c30f19f0dfd79cf23d30598",
						"subject": "64bit版プロセスでアクセントカラーが自動取得の場合に OverflowException が発生する"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "2ae0c328694f629ac955f4a81fa11f5191627980",
						"class": "nuget",
						"subject": "Extended.Wpf.Toolkit 2.9 -> 3.0"
					}
				]
			}
		]
	},
	{
		"date": "2016/08/17",
		"version": "0.81.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "",
						"subject": "[PR] ニコニコ見るツール作った",
						"comments": [
							"https://bitbucket.org/sk_0520/mnmn"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "4bb40c7cdd12417bd68891f5f256a72746ad64ca",
						"subject": "ヘルプページの先頭が general でなかったリンクミスの修正"
					},
					{
						"revision": "0f65ea0d88fb786c6bf6b87e5d4d1fc82e036037",
						"subject": "#475: ランチャーアイテムの履歴が保存されない"
					},
					{
						"revision": "09763b1274ae206e1728ace9816d22cf55e98703",
						"subject": "クリップボードのフィルタ入力部分の位置が変だった",
						"comments": [
							"全然ダメだったので 0.81.0 -> 0.81.1 への急遽リリース"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "230151dd075a83391cdbd05762a7e70449bff231",
						"class": "nuget",
						"subject": "Network.Json を 9.0.1 に更新"
					},
					{
						"revision": "230151dd075a83391cdbd05762a7e70449bff231",
						"class": "nuget",
						"subject": "Extended WPF Toolkit™ Community Edition を 2.9.0 に更新"
					},
					{
						"revision": "e98a5275da66a3862acc064e20e588f7975483a5",
						"subject": "#464: 設定モデル複製処理の自動化",
						"comments": [
							"本対応で #475 修正が無意味になった"
						]
					},
					{
						"revision": "b0f5c995b9a7f74d849ead2523bd18dea0d9d311",
						"subject": "開発環境を Visual Studio 2015 Update 3 に変更"
					},
					{
						"revision": "230151dd075a83391cdbd05762a7e70449bff231",
						"class": "nuget",
						"subject": "NUnit3TestAdapter を 3.4.1 に更新"
					},
					{
						"revision": "36ccdfb47bd04db39695687a76386adceb3ec470",
						"subject": "ソースディレクトリの変更",
						"comments": [
							"/Pe -> /Source"
						]
					}
				]
			}
		]
	},
	{
		"date": "2016/06/12",
		"version": "0.80.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "938d3a6a63b0825b1200ead75097be3b07c35d2d",
						"subject": "#448: ノートに書式を持たせる",
						"comments": [
							"RichTextそのままだと直感的でなくなるためワードパッドとは扱いが若干異なります",
							"主に段落関連を除外しています",
							"あと書式付きの場合は背景色を細かく設定しないほうがいいです"
						]
					},
					{
						"revision": "11bc0bfe11a97edbd8187760c1bdcef90516dd57",
						"subject": "#413: ヘルプファイルに更新履歴を表示する",
						"comments": [
							"今バージョンから更新履歴を確認するにはヘルプ(通知領域メニュー→情報→ヘルプ→更新履歴)から確認してください",
							"情報→更新履歴は廃止です",
							"IE依存してたしアップデート用の出力処理と確認用の表示処理が二重で地味に負担だったのですよ"
						]
					},
					{
						"revision": "f667c0d30400278b9539180badf86cf83234d32c",
						"subject": "#440: クリップボード・テンプレートのリストダブルクリック操作"
					},
					{
						"revision": "f85ff85c9426d058572cffbedf050611aa416860",
						"subject": "#470: 設定のバックアップにバージョン情報を付与する"
					},
					{
						"revision": "70d8b04addf6e9a8fa89eac3960a28e01e5d8340",
						"subject": "#465: ウィンドウを強制的に隠す操作にマウスも追加する",
						"comments": [
							"ツールバーの設定[自動的に隠す]が有効な場合にツールバーの空いている領域をダブルクリックするとツールバーを隠れた状態にします",
							"ボタン上でも出来ちゃうけどそこはまぁ運用回避で"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "b6ded0bf968b8f02ac0244d89cfd351521cae3e0",
						"subject": "#461: Windows8, 8.1, 10 でのツールバーがきもい",
						"comments": [
							"妥協の産物"
						]
					},
					{
						"revision": "c52547879b58eb7aaa8bee0c674dd75191970d35",
						"subject": "#462: GridHelpersのリンク先が間違っている",
						"comments": [
							"修正はしたんだけど別ライブラリに移動させたので記述から消えた"
						]
					},
					{
						"revision": "e1eabcb733eba9a79f50b541fcafcf2544d0f78e",
						"subject": "#458: クリップボード取り込み待機時間の設定UIが直感的でない"
					},
					{
						"revision": "b09ce6688e129db95d41b752f1f259858609f361",
						"subject": "#451: 設定項目のUIが直観的ない部分を調整する"
					},
					{
						"revision": "450e1c5d1aa119e3234c1122e5965fae7405d0de",
						"subject": "#471: 構成ファイル backup-archive が使用されていない"
					},
					{
						"revision": "4c74f93ef4e7ff4cc311f34df67ab3c114796cca",
						"subject": "#468: ノート最前面表示をホットキーから実施するとできたりできなかったりする",
						"comments": [
							"非アクティブ縛りで変に泥臭いことになってしまったけど多分動くよ"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "8c5ba7fa84f50a3e9f69b4a8a4c9e26b5e491e4b",
						"class": "nuget",
						"subject": "ICSharpCode.AvalonEdit を 5.0.3 に更新"
					},
					{
						"revision": "016e48184a6f3e3bd872bc18aa79f7deb2c13578",
						"subject": "データ補正処理を統一"
					},
					{
						"revision": "11bc0bfe11a97edbd8187760c1bdcef90516dd57",
						"subject": "#284: 更新履歴の空白データ要素を表示しない",
						"comments": [
							"#413実装時に同時解消"
						]
					},
					{
						"revision": "400b969282d589e992a512eed6c0ec10ff469085",
						"subject": "XAMLの名前空間を整備"
					}
				]
			}
		]
	},
	{
		"date": "2016/05/18",
		"version": "0.79.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "17f2f45d998dbf266ad0c5c40a6771d4f9ec1afe",
						"subject": "#350: ツールバーのメニューボタン「▼」の今後の身の振り方について",
						"comments": [
							"ツールバー設定からランチャーのメニューボタンを表示するかどうか設定可能にしました",
							"メニューボタン非表示状態でメニューを表示するには「マウス中央ボタン」、「Shift + 右クリック」のどちらかの操作が必要です"
						]
					},
					{
						"revision": "3cb7beb228d348d0557e2c33d35e03255fd10931",
						"subject": "#449: ランチャーアイテムの並び順を保存時にソートする"
					},
					{
						"revision": "018e9be333d09bb4732be0c3195e465bbb76f115",
						"subject": "#455: ツールバーを強制的に隠した状態にする",
						"comments": [
							"ツールバーの「自動的に隠す」が設定されている場合に ESC キーを二回入力すると表示中のツールバーが隠れた状態になります"
						]
					},
					{
						"revision": "ab7dcc91053c6c622c76f550e633bce6649a31c0",
						"subject": "#454: F1抑制機能を付ける",
						"comments": [
							"F1キーの誤入力を防ぐ機能です",
							"本機能が有効な時にF1を入力したい場合は F1 を二回入力してください",
							"設定箇所: 設定→本体設定→システム環境→F1キーを抑制"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "e60fdbe111507ab6f0be5bdf0f968710f93d0d58",
						"subject": "#383: ツールバーに表示しきれずはみ出したアイテムに救いの手を差し伸べる",
						"comments": [
							"ツールバーのコンテキストメニューからはみ出したアイテムを選択できます",
							"ただしこの場合は通常実行の挙動に限定されます"
						]
					},
					{
						"revision": "46f4de86ff4829edd6d637e04331b49dc8062751",
						"subject": "#447: グループ内ランチャーアイテムを上下矢印UIで移動させると落ちる",
						"comments": [
							"ランチャーアイテム側で削除したアイテムがグループ配下側で内部的に保持されたまま不可視だったことが原因なのでこれを可視化した",
							"ついでに #456 対応"
						]
					},
					{
						"revision": "bea456a776693cc4331dbd639fe8d730a448927c",
						"subject": "#456: グループ設定でノード操作時のちらつきを抑制"
					},
					{
						"revision": "dfcdc936844ae25d8c94d7bdc9acb2f1519349bd",
						"subject": "#450: ノートのサイズ変更枠ダブルクリックでもサイズ変更出来てしまう"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "#446: CIビルド時にpdbファイルは削除しないようにする",
						"comments": [
							"なんか急にしたくなっただけ"
						]
					},
					{
						"revision": "b5446023f209716ec684fd0f91ac34d1ca4773e9",
						"subject": "[nuget] Extended.Wpf.Toolkit を 2.8.0 に更新"
					},
					{
						"revision": "17e9a097b60c769ef71c5660ff5b3771be43d9c5",
						"subject": "#457: グローバルフック実装",
						"comments": [
							"Forms 版で使用していた Gma.System.MouseKeyHook を使用"
						]
					}
				]
			}
		]
	},
	{
		"date": "2016/05/08",
		"version": "0.78.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "46e2b6b4383e448e2d232d201ded0a636dad9615",
						"subject": "#436: コマンド入力で列挙するかどうか条件設定を追加する",
						"comments": [
							"コマンド入力保管対象を設定することで切り替えます(デフォルトで有効)",
							"入力補完時に有効設定アイテムのみ列挙し、入力が完全一致する場合は設定が無効でも列挙されます",
							"0.77.0 以下からのアップデートは全アイテムがデフォルト値に強制されます"
						]
					},
					{
						"revision": "11fa3c838fdd8da9712267c7580c3413f929f380",
						"subject": "#339: グループにアイコンを設定する",
						"comments": [
							"使用可能なアイコンは Pe 組み込みのアイコンのみに制限されます(ネットワーク上のアイコンとか使うと遅いので)"
						]
					},
					{
						"revision": "f0c5f9fedd871003676c76c2d4df369694139569",
						"subject": "#443: ノートのキャプションバーをダブルクリックでタイトル入力を行う"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "0fedad90fe8fc77ff45396a40d2e3192fce134cc",
						"subject": "#438: ノートの移動ができない"
					},
					{
						"revision": "2f91c0c618d69d6fb1cf3f921c9a079656605f86",
						"subject": "#317: 数値の範囲指定入力欄のテキストボックスをアップダウンコントロールにする"
					},
					{
						"revision": "df865b16d902ac5d0bbb28facd73aa2f62dde466",
						"subject": "#318: 時間の範囲指定入力欄のテキストボックスをアップダウンコントロールにする"
					},
					{
						"revision": "94e9872eec5a070a8fd8baf178824b3592eb8565",
						"subject": "#444: ノート設定の標準スタイルに折り返しを追加する",
						"comments": [
							"折り返しと最前面を追加した"
						]
					},
					{
						"revision": "feb09bba3193d9eb109353449c1ba3e32451551b",
						"subject": "#445: 指定して実行ウィンドウの初期フォーカスをオプションに設定すべき"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "de0e98d3692a86165feceb89dbefce7eeca545df",
						"subject": "開発環境を Visual Studio 2015 Update 2 に変更"
					},
					{
						"revision": "8f28de02237d714724c8f7e4619e13f22f71d441",
						"subject": "[nuget] Extended.Wpf.Toolkit を 2.7.0 に更新"
					},
					{
						"revision": "a4620d2ee9c5a0f5214da04d4c75a5cd841b307f",
						"subject": "[nuget] Hardcodet.Wpf.TaskbarNotification を 1.0.8 に更新"
					}
				]
			}
		]
	},
	{
		"date": "2016/04/07",
		"version": "0.77.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "ひさびさリリース。ランチャーと関係ないツール作ったりダークソウル3したりで忙しいのです"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "91bbd100ea168425f2dd48fdb4b01f548bd10535",
						"subject": "#428: 各種データのアーカイブ化タイミングを再検討",
						"comments": [
							"端末が一定時間アイドル状態であれば各データをアーカイブするよう変更",
							"設定値はApp.config(PeMain.exe.config)",
							"変更に伴いクリップボードのアーカイブ間隔を変更",
							"アイドル監視時間: 8分",
							"アイドル判定時間: 70分",
							"クリップボード閾値: 3時間"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "95a0ed24751b74615f10cb768b1c8302f35ee447",
						"subject": "#426: ヘルプファイルのファイルとディレクトリアイコンが出力ディレクトリに出力されていない"
					},
					{
						"revision": "762ceb36f86045633c129a8cedb44ae98526f8df",
						"subject": "#432: ノートの標準フォント設定が反映されない"
					},
					{
						"revision": "105dee68a5329f80b4febf68c6d63da48ec48cd7",
						"subject": "#429: フィルタリング中のクリップボードコピーで落ちる",
						"comments": [
							"例外捕まえただけの暫定対応",
							"原因調査してないので今後やっていく"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "84cde1568d8eb67a2aa6860e2dc7d9e995209103",
						"subject": "名前空間とかライセンス表記とかがさっと修正"
					},
					{
						"revision": "f3add1826547df139e487c4e9446ed71d89bd196",
						"subject": "[myget/NuGet] ShaerdLibrary更新に伴い関連ライブラリの更新",
						"comments": [
							"#431: PeからSharedLibraryへ統合した処理に委譲"
						]
					}
				]
			}
		]
	},
	{
		"date": "2016/01/18",
		"version": "0.76.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "[緊急] 0.76.0 で設定ウィンドウの保存実行後に Pe が落ちる問題に対応したため 0.76.0 と 0.76.1 は統合"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "#408: GUID重複に備える",
						"comments": [
							"Peの開発が続いている間にこの処理が日の目を見ることは100%ないだろなぁ"
						]
					},
					{
						"revision": "22b018037285e9837520b463ef1a450ee8d8a27a",
						"subject": "#420: Extended WPF Toolkit™ Community Editionのバージョンアップ",
						"comments": [
							"2.5 → 2.6"
						]
					},
					{
						"revision": "9863916ffe321091f3ba2f75a7fcbce67591aa7d",
						"subject": "#421: Json.NETのバージョンアップ",
						"comments": [
							"8.01 → 8.02"
						]
					},
					{
						"revision": "3d58546d546d42157abe6401ab303fa2567c58db",
						"subject": "#364: App.configの設定値をキャッシュする"
					},
					{
						"revision": "4366619ab4277922fd8ff5acdd11c66caa8ef2d7",
						"subject": "#423: HTMLクリップボードのURIを規定プログラムで開く"
					},
					{
						"revision": "c925ad58ff0c3a344fe892ab9f53e0781e31c6ea",
						"subject": "#362: App.config(PeMain)の説明"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "ea0a8ccaf4ca30ac4335ee094fd53dd18c8ee9d7",
						"subject": "#353: Windowsの終了・ユーザーのログオフを妨げる",
						"comments": [
							"調査結果としては設定ファイルのバックアップ、特にクリップボード全件保存の場合に各データファイルのバックアップに時間がかかっていた",
							"クリップボード・ノート・テンプレートの実データが閾値に該当するものをアーカイブした状態で保持するように変更",
							"クリップボード閾値: 更新が 3 日前で 256KB 以下",
							"ノート閾値: 更新が 7 日前で 1KB 以下",
							"テンプレート閾値: 更新が 10 日前で 4KB 以下",
							"閾値は App.config(PeMain.config) で定義されてるので内容については #364 を参照してください",
							"Pe 終了時にバックアップ→本処理の流れで実施されるためアップデート後の二回目終了時に効力が現れます",
							"デバッグ版ではうまくいったよ！デバッグ版ではね！"
						]
					},
					{
						"revision": "73c2e58b179dbf215f632dd69591134ab80c68fb",
						"subject": "#375: 起動時に各UI制御を行う"
					},
					{
						"revision": "84ba9542fc5fb57e573b62065c33eb8880cdf820",
						"subject": "#427: 設定保存時に死ぬ",
						"comments": [
							"内部的に掴んでいるファイルをさらに掴もうとしていました",
							"一部ややこしい問題もありました",
							"根本的に処理変えたところもありました",
							"おちこんだりもしたけれど、私はげんきです"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "a0afdd9051b074b019d90e8c0b53a87d3db1517d",
						"subject": "#418: 独立可能なライブラリを独立させる"
					},
					{
						"revision": "678005664f3a275d022d1b094720eb142c097a8d",
						"subject": "#419: 開発に関する諸々をヘルプファイルに記載する"
					}
				]
			}
		]
	},
	{
		"date": "2016/01/04",
		"version": "0.75.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "あけおめ"
					},
					{
						"class": "compatibility",
						"subject": "#415の影響によりユーザー設定ディレクトリ(標準だと %APPDATA%Pesettings あたり)の *.tmp ファイルが削除対象となりました",
						"comments": [
							"Pe の設定ファイルが配置されるディレクトリなのでユーザー側でどうこうしてるとは思えないけど一応周知"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "8fd925990917f64b7324b01740b61de81ac02a67",
						"subject": "#397: 言語ファイル読み込みにはdefault.xmlを親とする",
						"comments": [
							"そもそも条件的にdefault.xmlしか読めてなかったぜ！"
						]
					},
					{
						"revision": "b3ba79bd33e51aabc0cc80864d72a03beb049d91",
						"subject": "#237: テンプレート入力エディタを高機能にする",
						"comments": [
							"AvalonEditを使用",
							"今回実装分では単純に色設定のみ"
						]
					},
					{
						"revision": "65717cf3e0b63971a383c92cb40d2f0117af6d54",
						"subject": "#415: 設定ファイルへの書き込みはデータ出力後にファイルを置き換える"
					},
					{
						"revision": "dd26b9ee23c5cc45794741be9af2bffbd49d7d11",
						"subject": "#411: Json.NETを 7.0.1 → 8.0.1 にする",
						"comments": [
							"おっきな対応は#412で実施"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "1662115601ef2c0fc4f0c4d63f5ede8d8867f598",
						"subject": "#410: ログのファイル出力うまくいってない？",
						"comments": [
							"#393, #355でどうにもうまくいかなかった原因",
							"なんかロジック的には正しかったけど内部使用しているパラメータの扱いがミスってた"
						]
					},
					{
						"revision": "48ba67dbea1926477bb85fa9ff3763511b7ef84d",
						"subject": "#402: ウィンドウの背景色をシステムに合わせる",
						"comments": [
							"コントロールの色に合わせた"
						]
					},
					{
						"revision": "ac36b67564191a961462fdf93420a1c9d9f93d36",
						"subject": "#412: HashItemModel.Code の保存形式を変換する",
						"comments": [
							"今回リリースで一番の不安処理"
						]
					},
					{
						"revision": "6c53ec47572fc29a78dda532bbc278544957a335",
						"subject": "#414: パース出来ない設定ファイルの読込で落ちる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "bdca07370be06b8919af45c5b5703622a82fc3b1",
						"subject": "#416: シリアライズ処理に使用した元ストリームは呼び出し側で面倒を見る"
					}
				]
			}
		]
	},
	{
		"date": "2015/12/23",
		"version": "0.74.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "e7fe3b78cd68c8ec7b41b8c6f2966ed18a9a3488",
						"subject": "#25: ヘルプファイルの記述",
						"comments": [
							"通知領域コンテキストメニュー→情報→ヘルプ",
							"ひっさしぶりに生のHTML/CSS書いた",
							"読み込み時の細かい処理は追々調整する"
						]
					},
					{
						"revision": "82221341f524f7ac80bdf75935f3bbf5349c07b5",
						"subject": "#392: ホットキー処理を実施した際のトースト(バルーン)表示を選択制にする",
						"comments": [
							"設定→本体設定→操作通知",
							"Windows 10 で出まくるの鬱陶しいので初期値は「なし」に設定"
						]
					},
					{
						"revision": "a383696689fde08f647937b5361a60a3a3901c5c",
						"subject": "#370: クリップボードHTMLデータからクリップボード名を算出できない場合はテキストから取得する"
					},
					{
						"revision": "a3819ed98e56e3f45ff44cea38c267d3145b5bde",
						"subject": "#393: ログ出力をコマンドライン指定でなくGUI側でいつでも出力できるようにする",
						"comments": [
							"#355の逆襲",
							"本実装に伴いファイルへのログ出力実装を変更したけ通常使用には無関係"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "a2ecce5d3cadfeffae6e0f33f059b1dc75563cd7",
						"subject": "#406: ファイルのローテート処理で最新ファイルを削除している"
					},
					{
						"revision": "db117745015f1f5f9c672b9a19bf4c42242a5e41",
						"subject": "0.73.0の変更履歴にリビジョン記入してなかった"
					},
					{
						"revision": "d82438892c577157aeb4df9f53a9cdb3164d3696",
						"subject": "#405: ホットトラックの色算出に黒・白・灰色は計算に含まないように変更する",
						"comments": [
							"色の勉強しないと難しいなぁ"
						]
					},
					{
						"revision": "9c649e1814181584f6ae510ccf22cc6071efcf57",
						"subject": "#387: ランチャーアイテム登録中にアイコンの反映が行われない",
						"comments": [
							"コマンド項目修正は毎回ディスク見に行くのアホっぽいから500msの遅延更新"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "開発環境を Microsoft Visual Studio Community 2015 Update 1 に変更"
					}
				]
			}
		]
	},
	{
		"date": "2015/12/06",
		"version": "0.73.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "de74c412a997761664c9b76cb5c113fea0e694a9",
						"class": "compatibility",
						"subject": "#335: .NET Frameworkのバージョンを4.6に変更",
						"comments": [
							"本バージョンから .NET Framework 4.6 が必要になります",
							".NET Framework 4.6 は https://www.microsoft.com/ja-jp/download/details.aspx?id=48130 からダウンロードできます"
						]
					},
					{
						"subject": "本バージョンからアップデートチェックに使用するアドレスが変更となります",
						"comments": [
							"XML -> https://bitbucket.org/sk_0520/pe/downloads/update.xml",
							"HTML(Release) -> https://bitbucket.org/sk_0520/pe/downloads/update-release.html",
							"HTML(RC) -> https://bitbucket.org/sk_0520/pe/downloads/update-rc.html",
							"bitbucketのダウンローダーはレスポンスに`Content-Disposition: attachment;`があるけど大丈夫だろ"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "138734edf8e7cbc14169022e95de481ef3251a6c",
						"subject": "#367: バージョンチェック用XMLと更新履歴用XMLのURI変更",
						"comments": [
							"数世代はcontent-type-text.net側も保守するつもり"
						]
					},
					{
						"revision": "fe883254caa678c861f3444be15d405d514354b0",
						"subject": "#395: ログウィンドウに個別の出力・コピー・削除機能を設ける"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "7e111254355b9c9c88a9bcd74a3d9bfb2d745cc5",
						"subject": "#398: 自動アップデート時の最終調整スクリプトが実行されない",
						"comments": [
							"いつからおかしくなっていたかは未調査だけどパス間違ってた"
						]
					},
					{
						"revision": "d57c692a9c443570e941bfae5900c134ae1adb66",
						"subject": "#401: クリップボードの取込対象・制限のON/OFFが効いていない",
						"comments": [
							"1. 設定UIでの制御ができていなかった",
							"2. 設定補正時に強制ONになっていた"
						]
					},
					{
						"revision": "05af853ddf4ad923f967d75f25857ff47cbf4028",
						"subject": "#368: 環境によりツールバー設定の項目がはみ出る",
						"comments": [
							"どの環境でもはみ出てた"
						]
					},
					{
						"revision": "637e4bc45dec70801710630344d22161eaf320d7",
						"subject": "#403: 情報ダイアログに旧フィードバックリンクが残ってる"
					},
					{
						"revision": "a30390e605e378c4d9d1a1211d1b6ed0f5beaca8",
						"subject": "#399: ネットワーク接続ができない状態でユーザー情報送信を許可した場合に落ちる",
						"comments": [
							"アップデート確認と同じ処理方法だと思ってたら全然違ってた",
							"みんな大好き try ... catch(Exception) で対応"
						]
					},
					{
						"revision": "3464fde3b34c700868429b87c06ad76c5000f3aa",
						"subject": "#400: フィードバック入力ウィンドウをモードレスにする"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "de74c412a997761664c9b76cb5c113fea0e694a9",
						"subject": "#335: .NET Frameworkのバージョンを4.6に変更",
						"comments": [
							"やっとこさ nameof が使えるようになったので目についた範囲を修正",
							"実装の移行はのんびりやっていく"
						]
					},
					{
						"subject": "#25: ヘルプファイルの記述",
						"comments": [
							"次回バージョンで記載しますん"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/11/30",
		"version": "0.72.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "[事前通知]: #335: .NET Frameworkのバージョンを4.6に変更",
						"comments": [
							"0.73.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
							"nameof! nameof! nameof!",
							"#355の影響で0.72.0→0.73.0に先延ばし"
						]
					},
					{
						"subject": "[プライバシー]: #179, #297実装で設定によりインターネット通信の発生する可能性があるためアップデート後に使用許諾が表示されます",
						"comments": [
							"送信データを破棄したい場合は DATA-ID をお伝えください"
						]
					},
					{
						"subject": "[悩み中] #381: 匿名で課題作成を行えるようにする",
						"comments": [
							"課題への記入を匿名でも行えるようにするか悩み中です",
							"フィードバック機能も実装したし賛成・反対意見をもらえるとありがたいです"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "6f51246421c002bc4111f89be8a0e11acc9d0a7d",
						"subject": "#378: ユーザー識別子を作成・保持する",
						"comments": [
							"フィードバック等のユーザ情報収集時に使用されます",
							"設定画面の本体設定タブで再設定が可能です",
							"規定値ではユーザー環境のユーザー名・OS・CPU・メモリをコネコネしてMD5を算出します",
							"ランダム生成した時間は現在時間からMD5を算出します",
							"UI追加に伴いランチャーD&D設定はランチャータブへ移動しました"
						]
					},
					{
						"revision": "e12fb70455ac9c9b6cd68bc4892c219b5f3782d2",
						"subject": "#297: フィードバックをPe内で気楽に入力",
						"comments": [
							"通知領域Peアイコンコンテキストメニュー 情報→フィードバックで入力できます",
							"入力データの送信にはインターネット接続が必要になります",
							"データに関しては追々ヘルプ書きます",
							"入力データを破棄したい場合は DATA-ID をお伝えください",
							"本対応によりレジストリ情報の一部に書き込みが行われます。Pe起動時に書き込まれ終了時に破棄されます。キーは下記になります",
							"HKEY_CURRENT_USERSoftwareMicrosoftInternet ExplorerMainFeatureControlFEATURE_BROWSER_EMULATION",
							"HKEY_CURRENT_USERSoftwareMicrosoftInternet ExplorerMainFeatureControlFEATURE_DOCUMENT_COMPATIBLE_MODE",
							"過去に記入して頂いたフィードバックは破棄します",
							"ていうか余裕なくてあんまり見れてませんでした。ごめんちゃい"
						]
					},
					{
						"revision": "69dde0aac6b6c9337963bfce803becb7905d5575",
						"subject": "#373: 初回起動時の情報を保持する",
						"comments": [
							"本バージョンからの設定項目追加なので古いバージョン情報は持てません"
						]
					},
					{
						"revision": "d92cc38c8e24c965659efc20b857b32195941107",
						"subject": "#390: コマンドランチャーでディレクトリパスを入力した際に親ディレクトリも表示する"
					},
					{
						"revision": "3db332ff1e05dc55cbe81a94b47fdedb50dd731f",
						"subject": "#389: ランチャーアイテム選択リストのフィルタリング機能の一致方法を改善する",
						"comments": [
							"コマンドランチャーとランチャーアイテム一覧で実装が分かれていたのを統合しました",
							"入力文字列の先頭1文字で検索方法が変わります",
							"大文字: 前方一致 + 大文字小文字を区別する",
							"小文字: 部分一致 + 大文字小文字を区別しない"
						]
					},
					{
						"revision": "645424bc282f00ec958fe56b12011b1b528d81e0",
						"subject": "#179: 使用ユーザー情報の収集",
						"comments": [
							"#297での実装と環境を流用してユーザー情報を収集します",
							"今のところ実行タイミングは起動時・セッション開始時になります(自動アップデート確認と同じタイミング)",
							"設定→プライバシー から「ユーザー情報送信を許可」が有効になっている場合のみ送信処理が行われます",
							"送信内容はヘルプに記載する予定ですがまだヘルプが書けていません",
							"内容を確認したい場合はログを確認してください。要求・応答メッセージが出力されています",
							"送信データを破棄したい場合は DATA-ID をお伝えください",
							"本機能実装により本バージョンアップデートには使用許諾が表示されます"
						]
					},
					{
						"revision": "8c4546823c1ea43d24e95df3991340af95246ad7",
						"subject": "アイテム起動時のログ内容をまともにした"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "2ce7e8c9f6f7869980752736b0b9f6632b68b9b7",
						"subject": "#384: 短い報告用情報のビルド種別の項目名をTypeからBuildTypeに変更する",
						"comments": [
							"CLIもCLRに変更"
						]
					},
					{
						"revision": "ec6efd3d9c19ed1619421b36181ecfc87d130e73",
						"subject": "#372: 情報ダイアログは最前面表示にする必要ない"
					},
					{
						"revision": "86031a0fbf868d2734ee229f5fa2c7399b38f4c0",
						"subject": "#376: クリップボード・テンプレレートの転送にクリップボードを経由した場合にクリップボードオープンのエラーが発生する可能性あり",
						"comments": [
							"本改修に伴いクリップボードが空であれば転送後も空にするように修正"
						]
					},
					{
						"revision": "b4ef13c1afe0ec2e8f37bda937043db37a5d9b02",
						"subject": "#377: テンプレートの置換処理(文字列orT4)でクリップボードを使用した場合にクリップボードオープンのエラーが発生する可能性あり",
						"comments": [
							"本改修に伴いコピー操作の再試行を実装(全処理に影響)"
						]
					},
					{
						"subject": "#394: システム環境情報取得時の取得エラーの例外キャッチをやめる"
					},
					{
						"revision": "cbe3ae2273912e7337104e55cfa5b023ed517305",
						"subject": "#385: コマンド型アイテムのツールバーメニューに作業ディレクトリがない"
					},
					{
						"revision": "a6b6c4b54d6e8aae0736e7ff177f887e7d135333",
						"subject": "#386: ランチャー登録時に新規作成したアイテムを選択状態にする"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "b1f2262d22cd9f350df4818832e06c173ceb578b",
						"subject": "#355: 終了時にログを強制出力する",
						"comments": [
							"うまくいかんし#355自体は対応やめます",
							"#393で頑張りますん"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/11/15",
		"version": "0.71.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "[事前通知]: #335: .NET Frameworkのバージョンを4.6に変更",
						"comments": [
							"0.73.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
							"nameof! nameof! nameof!",
							"#355の影響で0.72.0→0.73.0に先延ばし"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "48e14eea7bd107e1ff299d29973074940bd3f4b6",
						"subject": "#360: キャンセルボタンとESCキーをリンクさせる"
					},
					{
						"revision": "43183959f9c4e3e127ed9272117770b0095e9091",
						"subject": "#303: 設定ファイル更新を頻繁に行わない",
						"comments": [
							"ノート・テンプレート・クリップボードの一覧データ保存時に一定時間待機するように改修しました"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "54f075926e318317e9b4617c485bd63f137e77fd",
						"subject": "0.70.0修正内容の各Revision記載漏れ"
					},
					{
						"revision": "771b804cc396463d68c84945d5007ba4213a7b82",
						"subject": "#355: 終了時にログを強制出力する",
						"comments": [
							"0.70.0での実装は色々残念だった",
							">ログ保存位置を指定していない場合(標準動作)は %APPDATA%logs に強制出力ログファイル(session-ending.log)が作成されていました",
							">>%APPDATA%直下に汎用的な名前でディレクトリを作っちゃったのでPe側では削除できません",
							">>>気になるのであれば削除しといてください",
							"まぁそもそも出力失敗してるから0byteファイルなんですけどね！"
						]
					},
					{
						"revision": "041d0f3e46a5ee30e925035925c2598b0a6d14ea",
						"subject": "#320: クリップボードの一覧アイテムの横幅とリストの横幅を合わせる",
						"comments": [
							"修正の簡易さからアイコンとタイムスタンプを左寄せにした",
							"ついでにテンプレートも同じスタイルに設定"
						]
					},
					{
						"revision": "a2498f3b825a320d3aa53e7405675cd558323ab0",
						"subject": "#352: アイコン+文字列のスタイル整理"
					},
					{
						"revision": "c30b577b7ccd0d1974f0c63d190b48cca9a92d1e",
						"subject": "#357: クリップボードの最上位移動アイテムの作成タイムスタンプを更新するのおかしいっすよね",
						"comments": [
							"今までは作成日時を元に並び替えてたけどソート用の項目で並び替えるようにしたので作成日のタイムスタンプは保たれるようになりました"
						]
					},
					{
						"revision": "3dfa48c8831e4f3434d89dde92dd09545041b038",
						"subject": "#361: クリップボード重複判定で範囲指定した場合になんか変",
						"comments": [
							"実装見ると変ではなかったけど直観的ではなかったので動作変更",
							"範囲指定した場合、今までは一番古いものを基準としたが本バージョンから新しいものを基準とするように変更",
							"でもまぁ#363に食われるだろうけど"
						]
					},
					{
						"revision": "43183959f9c4e3e127ed9272117770b0095e9091",
						"subject": "#363: クリップボード重複判定の初期値を全件対象にする",
						"comments": [
							"本バージョンへアップデートした際に重複判定件数が 50(0.70.0以下の規定値) であれば -1(全件) に変換されます",
							"0.71.0から -1 が規定値になります"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "ad28f66eeea13df30319465b6dff233c36340067",
						"subject": "アップデートコンソールの最後に少しだけ待ち時間(5秒)を設定",
						"comments": [
							"有効になるのは次回アップデート時です",
							"これと言ってユーザー側に意味はありません"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/11/12",
		"version": "0.70.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "#346: Forms版→WPF版用データコンバーターの廃止",
						"comments": [
							"本バージョンを持ってForms版からのデータ引き継ぎサポートを終了します"
						]
					},
					{
						"class": "compatibility",
						"subject": "#104による0.39.0 未満のアップデートチェック用URI互換を破棄",
						"comments": [
							"事前通知なしに消しても誰も困らんだろ……"
						]
					},
					{
						"subject": "[事前通知]: 0.72.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
						"comments": [
							"nameof! nameof! nameof!"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "2d414ef51fb09e3346d1f775da19a7f712bff648",
						"subject": "#322: ノート本文の自動改行を設定可能にする"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#349: HTMLクリップボード内のスクリプトエラーを無視する"
					},
					{
						"revision": "0c18841b28e87d85a510c0501328ada6b74c31a0",
						"subject": "#305: メモリ消費を抑える",
						"comments": [
							"到達不能な破棄処理を有効にした"
						]
					},
					{
						"revision": "82289f6d51a41a3746ef859e3464f6da347017b1",
						"subject": "#348: 情報ダイアログの「短い情報」に不要な'_'が存在する"
					},
					{
						"revision": "825eb2b7bd5229ed1758727c1b3031bd5ea5fbf0",
						"subject": "#356: クリップボード取込失敗→再取込で失敗しなかった場合はエラーを表示しない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "0df522de1a43b6f562dfca085d629b7343810e57",
						"subject": "#351: データ複製の事故防止",
						"comments": [
							"基本ロジックと一部データには適用",
							"全データに設定するのは労力的にしんどいので追々適用していく"
						]
					},
					{
						"revision": "cc4defd86c3103b3aea443cbaeb186d59c716776",
						"subject": "IDE0001の抑制",
						"comments": [
							"usingする名前空間をVS2013スタイルで留める",
							"親以降の名前空間が同じ名称結構多くて完全修飾の方が分かりやすいのですよ"
						]
					},
					{
						"revision": "008f6c8ba23142fecd4830c635f67f837761bf57",
						"subject": "#347: 一旦外していた使用許諾のユーザー操作再設定を復帰する"
					},
					{
						"revision": "c16e57c6f551b4106ed59f2ca1dfe9cda6fb99d0",
						"subject": "#354: ログ出力用ストリームをごにょごにょ"
					},
					{
						"revision": "36cf532b7ad735efc7009bf777bc0239f21d80c7",
						"subject": "#355: 終了時にログを強制出力する",
						"comments": [
							"#353のため#354の下準備から#355まで実装",
							"一番親元の作業オブジェクトに追加したのでView側で固まってたら再調査が必要なので保留とする",
							"ステータスは課題を参照のこと"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/11/03",
		"version": "0.69.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "[事前通知]: 0.70.0 で Forms 版データ引き継ぎ処理を廃止します",
						"comments": [
							"実装は残しててもいいんだけど名前空間被っててコーディングしんどいのですよ"
						]
					},
					{
						"subject": "[事前通知]: 0.72.0 で .NET Framework の対応バージョンを を 4.5.1 から 4.6 に変更します",
						"comments": [
							"nameof! nameof! nameof!"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "cc1b9e1b348cbfd8f016b181a62da6de722f0e7e",
						"subject": "#341: メインボタンのアイコンがなんかずれてる"
					},
					{
						"subject": "#305: メモリ消費を抑える",
						"comments": [
							"使いまわせる ViewModel は再生成を抑える",
							"意味あんのか知らんけど一部バインドを初回のみに変更"
						]
					},
					{
						"revision": "a5c46b69fd1872f0c69325f2604c193f6d0ea86c",
						"subject": "#342: クリップボードやテンプレートの項目を一定数選択していくと古い選択アイテムが表示されなくなる",
						"comments": [
							"#311と#305で死んでしもうてた"
						]
					},
					{
						"revision": "87b43b7580856087ea3dfb25a73bfe2331e678db",
						"subject": "#298: メニュー・ラベルにショートカットキーを表示する",
						"comments": [
							"WPF版作成時に未実装だった"
						]
					},
					{
						"revision": "59b025d28ae3026b9eb8a944b01c2f4ee273a79b",
						"subject": "#343: ランチャー自動登録ボタンのアイコンが環境に表示できない",
						"comments": [
							"旗マークも単色なんでふちどりしておいた"
						]
					},
					{
						"revision": "b20ae7d9a4d6e759b264f5355cd1cf45c2e3b2e1",
						"subject": "#345: グループ名変更UIが邪魔",
						"comments": [
							"左側に出すように変更",
							"OSの利き手設定によって表示方向が右だったりするけど気にしない"
						]
					},
					{
						"revision": "81ff02dcbe710f93077f3e09901d2ee787643d18",
						"subject": "#344: ホットキーコントロールをWindows提供(HOTKEY_CLASS)の挙動に合わせる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "c2b66c7720199c68b4bb9538203534cd51763f19",
						"subject": "#340: masterからdevelopmentマージはFFする"
					}
				]
			}
		]
	},
	{
		"date": "2015/10/18",
		"version": "0.68.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "[事前通知]: 0.70.0 で Forms 版データ引き継ぎ処理を廃止します",
						"comments": [
							"実装は残しててもいいんだけど名前空間被っててコーディングしんどいのですよ"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "1b17cbf88727190b88d7d0dd45868c1651fc2039",
						"subject": "#289: ランチャーアイテムを設定画面へ遷移せずに削除する",
						"comments": [
							"削除用ボタンのUI実装によりノート側の削除ボタンも変更"
						]
					},
					{
						"subject": "#305: メモリ消費を抑える",
						"comments": [
							"焼け石に水かもだけど ViewModel を破棄した時に Model の参照を外す",
							"ツールバーのGUI構築方法を改善"
						]
					},
					{
						"revision": "220788077df6490e37381ace0db75aecf537882a",
						"subject": "0.68.0が動かない",
						"comments": [
							"依存プロパティ実装修正の確認漏れ"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "42390940f9be507dba8087a5645d34500109f3e5",
						"subject": "#337: コマンド入力後に再度コマンド入力すると前回入力値が残っている"
					},
					{
						"revision": "c5171876096b8c063d0a0fb4df4e8173a2bd0089",
						"subject": "ツールバーの設定「メニューボタンを調整する」は有効を規定値にした",
						"comments": [
							"旧バージョンからのバージョンアップには影響しません"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "0a9554ea3fd284d790ec6f0877efc54ef7b430b0",
						"subject": "#334: 開発環境をVS2013からVS2015に変更"
					},
					{
						"revision": "f142bc9b0cc232c77d3f0b41ee9f15df84713473",
						"subject": "#274: 各ソースファイルにライセンス情報を記載する"
					},
					{
						"revision": "f142bc9b0cc232c77d3f0b41ee9f15df84713473",
						"subject": "#336: コーディング規約変更: TAB -> SPACE"
					}
				]
			}
		]
	},
	{
		"date": "2015/10/12",
		"version": "0.67.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "出張おわったー、ちまちま修正できるー"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "3cf6990dd4c4ebdda3cd73a0beefc469f5dd924e",
						"subject": "#329: クリップボード設定「重複アイテムをリストの最上部に移動する」が保存されない"
					},
					{
						"revision": "5a07c456577c1eadb2a50c87c51ec70f51602ece",
						"subject": "設定ウィンドウのツールバー項目において「表示時間」とそれ以降の項目が重なっていた"
					},
					{
						"revision": "aeedf9d364628f7a0c7af9377aff6e03ee9c8391",
						"subject": "#330: リスト最上部へ移動したクリップボードアイテムを選択状態にする"
					},
					{
						"revision": "30dcf8f94ed52a6aa802ff1925e20e324239c8bd",
						"subject": "#332: ホットキーからノートの前面移動を行っても前面移動しない",
						"comments": [
							"出来たりできなかったり。。"
						]
					},
					{
						"revision": "39c8f9cdea51d327c9a6aea405264d3f73b4a6c4",
						"subject": "#331: 自動的に隠す状態のツールバーが表示された際にアクティブウィンドウ云々",
						"comments": [
							"たぶんなおった、もう勘弁してください"
						]
					},
					{
						"revision": "53998305f8990f54f00ad054016af46f78e76e94",
						"subject": "#321: クリップボードの取り込み処理で失敗すれば再試行する",
						"comments": [
							"☆ 突 貫 工 事 ☆"
						]
					},
					{
						"revision": "bdf5bfe81e2d912e077ba72db00789e348ce75d6",
						"subject": "#312: ツールバー設定画面のランチャーアイテムが選択されている状態でスクロールバーがスクロールできない"
					},
					{
						"revision": "f6ca1c3a1345c6d895300ccee5823c6fa4bee05e",
						"subject": "#305: メモリ消費を抑える",
						"comments": [
							"継続課題のため終了はしない"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "1ece2f0d33addf2ffd2cce1e16581733322563f6",
						"subject": "ログの保持上限数を持たせた"
					},
					{
						"revision": "9a1153b4dd0a5beab92c295e2c213859ce78974a",
						"subject": "#325: 可能な限り標準提供されているConverterを使用する",
						"comments": [
							"BooleanToVisibilityConverter くらいしかなかった",
							"今後見つけ次第修正していく"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/09/28",
		"version": "0.66.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "出張が終わらない。おうち帰らせて"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "230f76f39afee3fa64c673af2df71390834d269d",
						"subject": "#327: リサイズ可能なウィンドウでデザインに問題なければリサイズグリップをつける"
					},
					{
						"revision": "2d9bec4652ef1457356d1bd95c2095f9c982c5c2",
						"subject": "#319: 重複したクリップボードをリストの上位に移動させる",
						"comments": [
							"0.65.0以下からアップデートした場合、本機能は規定値(有効)に設定されます"
						]
					},
					{
						"revision": "8e97ff6e76e8326e721dbf5045b2d22a94f97c4c",
						"subject": "#314: 各ウィンドウのタイトルバーにそれっぽい値を設定する",
						"comments": [
							"ユーザー視点的に何の影響もない"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "9b1fec690e71e599aa505495d5a6ecf59fe6caef",
						"subject": "#315: 隠しファイルを表示するホットキー通知のタイトルがローカライズされていない"
					},
					{
						"revision": "b481cf35c57dc0494e13944e7abe8be9d9bb1657",
						"subject": "#307: ログ追加時にログウィンドウが表示された場合に項目が選択されていない"
					},
					{
						"revision": "20af45367aabb78aa58dfa0ee41385707636d307",
						"subject": "#326: 起動時にクリップボード取込処理を実施する"
					},
					{
						"revision": "14612accee7d5d837eb32e5a5d70fec6ff389b52",
						"subject": "#302: 各アイテムの更新日時等をきちんと更新する",
						"comments": [
							"見える範囲で実装",
							"正直ハンドリングしてない部分までは無理"
						]
					},
					{
						"revision": "6fdafabb76e13fdd4f57ab7c5feff5744e7bec0c",
						"subject": "#311: インデックスデータ統括クラスのデータ破棄処理を政治家的に有耶無耶にしたい",
						"comments": [
							"ヘッダ部とデータ部で管理されているノート・クリップボード・テンプレートのメモリ管理方法が改善されました",
							"特にクリップボードの画像データによるメモリ圧迫が改善された気がします"
						]
					},
					{
						"revision": "710cc8fc398faf32245604488ce8d60897ebba63",
						"subject": "#316: 自動的に隠す状態のツールバーが表示されたときにアクティブウィンドウがツールバーになる",
						"comments": [
							"わっけわかんねぇわ"
						]
					},
					{
						"revision": "d22cdf872ce1686e2f28c485d234a1b571dc6692",
						"subject": "テンプレートアイテムの置き換え方法変更時にリスト表示部分が追従していないかった"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "487fe490847d6c987ad9e5eaf4e82348a569243b",
						"subject": "#309: ログデータ保持に生データを持たないようにする"
					}
				]
			}
		]
	},
	{
		"date": "2015/09/19",
		"version": "0.65.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "b561d8baa4e4433b021f9e90b2c12bd7d319b19a",
						"subject": "#287: ツールバーアイコンの開始位置を変更可能にする",
						"comments": [
							"フロート状態以外で最上位(上 or 左, デフォルト)・中央・最下位(下 or 右)にツールバーのボタンを寄せます"
						]
					},
					{
						"revision": "51e07c2071b7f6cd841aab500be6a4ceb0856c9a",
						"subject": "#290: ツールバーのアイコン上にファイルをD&Dした際の挙動を変更可能にする"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "cd73f95fbea85bb3feb8b7759bdedf4c8ff7b3ea",
						"subject": "#294: アップデート確認用文言が重複してる"
					},
					{
						"revision": "91539fe4e31c3b453e0f11e6ac84464d811dfe59",
						"subject": "#306: テンプレートウィンドウ表示切替時にアクティブ化されない",
						"comments": [
							"初回表示時の対応をクリップボード, ノート, コマンドウィンドウにも適用"
						]
					},
					{
						"revision": "a1ed556bb986f9a720d1203fee5876a8ea3490cc",
						"subject": "コマンドウィンドウのリスト項目描画方法を他のリストに合わせた"
					},
					{
						"revision": "c19fca46eb915a09ea17d35638686eca40393860",
						"subject": "ツールバーへファイルD&Dを行い、指定して実行ウィンドウを表示すると前面に表示されない不具合の修正"
					},
					{
						"revision": "c6b8719644cb947355bca808dfd75e348ffc15f0",
						"subject": "#310: 自動的に隠す状態のツールバーを表示した際にZ位置が下位に存在する"
					},
					{
						"revision": "ef80cfa3e9677d507e6dca32e4fdcc4d75dc9506",
						"subject": "#301: 自動的に隠す状態のツールバーがシステム的に復帰したとき描画されていない"
					},
					{
						"revision": "363bd30fcdf946bb2cb1748476bc984dbdfad37d",
						"subject": "#308: 設定ダイアログのランチャー項目にファイルのD&Dでアイテム登録できない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "fa987ecba4c62c0fe4c35b06be72adb1c2bfcbaf",
						"subject": "ソース管理を git 1 から git 2 に変更"
					},
					{
						"revision": "70a855acb0d0a980b0506a4ccff1028f3c42ca05",
						"subject": "#295: 未補足の例外を受け取る"
					},
					{
						"revision": "d9f9204da73a4bb6244cefc93d641ef129647f51",
						"subject": "#304: beta版実行用バッチファイルがWPF版の設定データ構成に未対応"
					}
				]
			}
		]
	},
	{
		"date": "2015/09/14",
		"version": "0.64.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "簡単だけど放置するのもなんだかなぁ課題を早めに解消"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "d6fca24face6fd17b58fa3ca145d958953cbc283",
						"subject": "#291: 各ノートの設定が反映されない",
						"comments": [
							"データ補正時に固定・最小化を無効にしてた",
							"多分初期化でやりたかった内容が補正側に入ってた"
						]
					},
					{
						"revision": "23ccf7333b12b872df02a94f9ad5712bd188dbb5",
						"subject": "#292: アップデート更新内容表示ウィンドウのデザインが適当",
						"comments": [
							"XAMLだけ修正したので次回更新内容表示時に反映されてるはず"
						]
					},
					{
						"revision": "fa0e1bbfeca222aa184fde19d6709ec452797fff",
						"subject": "#293: 個人設定テーマ変更時に落ちる"
					}
				]
			}
		]
	},
	{
		"date": "2015/09/13",
		"version": "0.63.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "本バージョンからWPF版になります",
						"comments": [
							"基本的な機能はForms版の踏襲ですがあくまで似ているだけです",
							"今後の機能追加・保守はWPF版のみになります",
							"本バージョンはあくまでForms→WPFへの移植で溜まっていた課題への対応は次回バージョンから頑張る",
							"実装期間長かったー！",
							"出張先からのリリースなのでWPF版でのアップデート試験してないけどいけるさ、大丈夫さ、気にするな"
						]
					},
					{
						"class": "compatibility",
						"subject": "Forms版とWPF版の設定データに互換性はありませんが一部設定のみ引き継がれます",
						"comments": [
							"!注意! バグバグしてそうな本バージョンへのアップデートを見送るユーザーもいそうなので、下記引き継ぎ機能は未来バージョン数世代はサポートします",
							"引き継ぎ処理はWPF版本体設定が存在せずForms版本体設定が存在する場合に実施されます",
							"引き継がれる設定: 基本設定、ランチャーアイテム、グループ",
							"引き継がれない設定: 各ノートデータ、各クリップボードデータ、各テンプレートデータ",
							"ランチャーアイテム互換性: ディレクトリアイテムはファイルアイテムに変換されます(内部実装として地味に予約しているので要望があれば検討します)",
							"ランチャーアイテム互換性: 組み込みアイテムは引き継ぎ対象外になります(将来的にはまたサポートしますが今は休止)"
						]
					},
					{
						"subject": "ツールバー",
						"comments": [
							"Aero Glass を使用しなくなりました",
							"「自動的に隠す」設定時に隠れる際のアニメーションを廃止しました(実装時に設計をミスった)",
							"ランチャーアイテムのメニューからファイル一覧メニューがなくなりました",
							"ランチャーアイテムのメニューからアイテム編集が行えるようになりました",
							"コマンドアイテムもファイルアイテムのように実行できるようにしました",
							"ALTキー押下によるアイテム並び替え機能が廃止されました(現実装だとちっと難しそうなので後回し)",
							"ESCキー二回押下でツールバーを隠す機能は一旦廃止しました(実装忘れてた)"
						]
					},
					{
						"subject": "ノート",
						"comments": [
							"各種設定編集をメニューからでなく一元的に操作できるようになりました",
							"通知領域からの一覧表示メニューが表示中・非表示に分離されました"
						]
					},
					{
						"subject": "コマンド",
						"comments": [
							"URL入力機能を廃止しました(実装忘れてた)",
							"候補一覧にアイコンを表示しました"
						]
					},
					{
						"subject": "クリップボード",
						"comments": [
							"データ保持方法を変更。Forms版で大きな画像ばかり取り込んだ際に落ちる不具合が解消されたと思います。思うだけで試してません",
							"クリップボードの各種データ形式に対して取込制限設定を追加しました",
							"取込種類・保存種類を統合しました"
						]
					},
					{
						"subject": "テンプレート",
						"comments": [
							"クリップボードウィンドウから独立しました"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "ウィンドウ位置保存機能に最大化・最小化をサポート"
					},
					{
						"subject": "ウィンドウ位置保存設定をUI上から変更できるようにしました"
					},
					{
						"subject": "ツールバーのメニューボタン位置を特定条件で変更する機能の追加",
						"comments": [
							"ツールバーを右側表示した時にメニューボタンを左に表示します"
						]
					},
					{
						"subject": "ツールバーの自動的に隠すまでの時間をUI上から変更できるようにしました"
					},
					{
						"subject": "使用許諾再表示バッチ機能の一時廃止"
					},
					{
						"subject": "あとなんか色々"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "Forms版のソリューションは [Pe]/Pe-Forms に配置されます"
					},
					{
						"subject": "以下IssuesはForms版で上がっていてWPF版から解決されたであろう課題。詳しく見てないけどなんとなく解決できたんじゃねってやつです"
					},
					{
						"subject": "#245: テンプレートって「システム環境」と違う気がする"
					},
					{
						"subject": "#243: ツールバーの自動的に隠す状態への遷移時間"
					},
					{
						"subject": "#248: 高DPI環境での表示不具合"
					},
					{
						"subject": "#275: クリップボード取込時にサイズ制限を行う"
					},
					{
						"subject": "#286: Aero AutoColorを適用させる"
					},
					{
						"subject": "#137: 大きなファイル読み込みで死ぬ"
					},
					{
						"subject": "#210: クリップボードアイテム名を変更する"
					}
				]
			}
		]
	},
	{
		"date": "2015/08/30",
		"version": "0.62.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "WPF版の開発に注力していたのでForms版ひさびさのリリースです"
					},
					{
						"subject": "本バージョンを持ってForms版としては最終リリースになります",
						"comments": [
							"今後はWPF版としてリリースされる予定です",
							"未決定ですがWPF版とForms版での設定データに互換性はありません",
							">変換処理実装に割く時間が無いかもなのです",
							">>互換性を持たせるための処理・検証よりWPF版リリースを優先してそこから発生した不具合の修正を優先したいのです",
							"よっぽどおかしな処理があればForms版でも修正入れますが、ほぼほぼ無いです"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ショートカット処理でリソースリーク"
					},
					{
						"subject": "コマンド入力時のTAB, Shift + TABでの次候補選択順序が逆"
					},
					{
						"subject": "#285: クリップボード/テンプレートのアイテムリストの件数が更新されない"
					}
				]
			}
		]
	},
	{
		"date": "2015/05/31",
		"version": "0.61.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "#282実装によりランチャーアイテム起動時の作業ディレクトリが変更されました",
						"comments": [
							"作業ディレクトリが設定されている場合の挙動は0.60.0以前と変わりありません",
							"作業ディレクトリが設定されていない場合、実行パスの親ディレクトリが作業ディレクトリとして使用されます"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "2b8f3d0a2fd249f00ddcf38e193025d3bcf10be9",
						"subject": "#276: コマンド入力の補完を行う",
						"comments": [
							"入力中に[TAB]キーを押下することにより補完を行います",
							"ノリと勢いだけで実装したので細かい挙動は気にしないでいただきたいなぁと思ってる、と書いとけばいいって予防線の張り方"
						]
					},
					{
						"revision": "08d4b77a59e81f3fe2687351e05418f334033c65",
						"subject": "#281: データ保存を任意タイミングで行う'タスクトレイコンテキストメニューの拡張メニュー表示(Shift + 右クリック)で項目が表示されます"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "bceb10e9d6ba6462082a245989a9b2f515bdc427",
						"subject": "#272: 設定項目にある「ディスプレイ」"
					},
					{
						"revision": "1324613256ac33438437366fdf3ad81ad3018c53",
						"subject": "#279: 起動時に例外"
					},
					{
						"revision": "859c4366ebcec32053c4cbb704d7bafa726d6224",
						"subject": "#277: コマンド入力でファイルパスの場合に\"\"を入力した場合、それが最終ファイルだと警告が出力される"
					},
					{
						"revision": "37acab0a0780d719087b21d8ac62b2edef9c49b0",
						"subject": "#280: コマンド入力で開けないファイルを実行した際に例外が発生する"
					},
					{
						"revision": "8e251f966f002d38d3436adcdcbca42c18474b02",
						"subject": "#282: ランチャーアイテム起動時に作業ディレクトリが指定されていない場合、起動パスの親ディレクトリとする"
					},
					{
						"revision": "a2b672e40ae768d7890d6f52715d964377a6f85e",
						"subject": "#283: Peがフルスクリーンになるのを邪魔する",
						"comments": [
							"ゲームしようとフルスクリーンにするとツールバー状態のPeがそれを解除しようとしてゲーム進行が止まるのですよ",
							"Crysis2の時は大丈夫だったんだけど昨日買ったCrysis3だと止まるんだよ！ だれだよこんなソース書いたやつは！"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/05/24",
		"version": "0.60.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "コマンド型ランチャー機能を実装しました",
						"comments": [
							"一応メニューから表示できますが実装としてはホットキーから表示する思想です"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "bbe615b060f72f00de83788fc2282bca98b726cc",
						"subject": "#244: 長らくほったらかしのコマンド型ランチャー作ろうべさ",
						"comments": [
							"過去バージョンから設定を引き継ぐ場合、タグ・ファイル検索機能は無効になってます",
							">設定補正だるかったし否定形の設定項目作るのに気が引けたのよ",
							"アイコン設定は実装してるけど下記事情によりリスト上のアイコン描画処理は将来実装",
							">>描画そのものは出来るんすよ",
							">>>出来るけど ComboBox の TextBox 部分のサイズ(高さ)がかなり残念なことになる",
							">>>>色々やってはみたけどこれだけのためにリリース伸ばすのもかったるかった",
							"はよWPFに移りたい、しんどい"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "6715bc48465c7df23851ac911275391b8678e761",
						"subject": "#232: 標準入出力で出力系の改行を待たない",
						"comments": [
							"それっぽくは動くけど勇んで走り出したTaskの行方は誰も知らない",
							"正直なところ白旗"
						]
					},
					{
						"revision": "ff9fd7f27ddaefd8e3f8b98bb9c8777739d3a334",
						"subject": "#265: ノート一覧のプレビューをもうちっとうまいこと表示する"
					},
					{
						"revision": "4ad88a0b04ad08f9f468a3515c577c4230b1c064",
						"subject": "ツールバーのツールチップ描画処理をちょっと改善"
					},
					{
						"revision": "953c589f3348053f37c819c28d42a10623c0ae9f",
						"subject": "アイコンパスがファイルとして存在するが無効パスと判定された場合に動作が不安定になる不具合の修正"
					},
					{
						"revision": "096c4d0d2d399d2e384b3febef41664320e0d86d",
						"subject": "#271: 設定ダイアログ保存時に例外発生",
						"comments": [
							"設定ダイアログ保存時にタスクトレイ右クリック連打したら再現できた"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/05/12",
		"version": "0.59.3",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "0.59.0, 0.59.1, 0.59.2 を潜り抜けた #239 が生んだ奇跡の#270",
						"comments": [
							"同じようなのが次発覚しても0.60.0と統合する"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "fb37c851c9a83b053f71391027b9888abc8f6048",
						"subject": "#270: クリップボード履歴のアイテム名が保存されない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "developmentブランチ作らんと急なリリースしんどいなぁ"
					}
				]
			}
		]
	},
	{
		"date": "2015/05/12",
		"version": "0.59.2",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "0.59.0, 0.59.1 を経てなお #239 が死んでた",
						"comments": [
							"ごめりんこ☆（ゝω・）vｷｬﾋﾟ"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "5c609c1ebf0473cf194349f7c70b5f971889a050",
						"subject": "#269: テキストテンプレート名が保存されない"
					}
				]
			}
		]
	},
	{
		"date": "2015/05/11",
		"version": "0.59.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "666ba18350db2b4f3cba71c96e24e1dbb6fe0e47",
						"subject": "ごっめん！ 0.59.0 で #239 死んでた！！"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#268: ランチャーアイテム保存できない"
					}
				]
			}
		]
	},
	{
		"date": "2015/05/10",
		"version": "0.59.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "#239が超心配！"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "5fc6d90edeaf354cdbf1df0cf5f2148b6090294a",
						"subject": "#264: クリップボードのファイル一覧で選択ファイルにコンテキストメニューをつける"
					},
					{
						"revision": "184425d4782c094f88123867a72e2b7acd023db9",
						"subject": "ツールバーのメインボタンに表示するツールバー位置選択項目を視覚的にした"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "d39fcc84fc87d9607737668520476b34a907e860",
						"subject": "#266: ノートの選択したフォントが反映されない場合がある",
						"comments": [
							"ちゃんとできてるのかちと不安"
						]
					},
					{
						"revision": "7e0e3e51da5d34c4875ac304ca69723f770e7ee3",
						"subject": "#251: イメージアイテムチェック時にAccessViolationException",
						"comments": [
							"あっかん、再現できん",
							"例外ガン無視する",
							"Exception 捕まえずに try { ... } catch(AccessViolationException) { ... } で自重した私をほめてください"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "a4233e81ee1e413f596ec24b6016678129a0f779",
						"subject": "#239: 設定ウィンドウ構築処理が初期実装継ぎ接ぎで開発側泣きそう"
					}
				]
			}
		]
	},
	{
		"date": "2015/05/05",
		"version": "0.58.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "10c9f48b0860aa8df504800534065fb8c116e7ad",
						"subject": "#256: クリップボード・テンプレートのテキスト転送方法を常時切り替え可能にする"
					},
					{
						"revision": "71c380a0ea3b5abbec19015af67598858b688c70",
						"subject": "#236: クリップボード/テンプレートウィンドウの分割領域を保持する"
					},
					{
						"revision": "ebe3c60521acee7d864fd8345bcc8070bf51ea8c",
						"subject": "#255: ノート一覧から該当ノートアイテムをプレビュー"
					},
					{
						"revision": "cab4083d7fd19413655d9f5e8743e77bd8e48286",
						"subject": "#238: T4エラー時に行番号も出力する"
					},
					{
						"revision": "8dcab3775ddaf286e7653004c7ef4bacc5161b46",
						"subject": "#263: クリップボード重複判定でファイルの場合はソートする",
						"comments": [
							"さすがに過去分までは補正しないので本バージョンから取り込んだものが対象となります"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "813d9ca878215c9c3c422cd86b6cabf0229150f5",
						"subject": "クリップボード/テンプレートの保存・削除ボタンをアイコンのみに変更"
					},
					{
						"revision": "58f43289bb7c9a96844e317c1c64c97b47ad1679",
						"subject": "#257: ファイルアイテムに上位ディレクトリが存在しない場合にもファイルメニューを表示させる"
					},
					{
						"revision": "80598bf089add78653fb6489ec0edaa4297a88e3",
						"subject": "#261: 起動時に出力されるログがUIスレッドに影響する"
					},
					{
						"revision": "333dab558438827c361fc3ce7e3451667b677416",
						"subject": "#260: ノートのタイトル入力後、前回の入力内容が微妙に残って汚い",
						"comments": [
							"あっれぇ再現しないぞぉ",
							"でも実装したから完璧っすよ"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/04/12",
		"version": "0.57.0",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "77aaf9dc0e54c9198d66651e9af52871dd035b9c",
						"subject": "#254: テキストテンプレート置き換えプレビューがRTFじゃない"
					},
					{
						"revision": "0c8bb809d3d6c810417278d4c7d3782b197ca7c6",
						"subject": "置き換え処理を行わないテンプレートアイテムが選択状態でテンプレートウィンドウ初回表示時に不要なリスト部分が表示されていた"
					},
					{
						"revision": "40541a787b9500f4a15a86168c67d5e9d980693d",
						"subject": "#253: ノートの本文が保存されないことがある"
					},
					{
						"revision": "e48147467c97155015f64733953635dd99102cc5",
						"subject": "ツールバーのツールチップ表示時にツールバーを全面に移動する"
					}
				]
			}
		]
	},
	{
		"date": "2015/03/29",
		"version": "0.56.0",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "275e831cc4243da25eef41f2f1739e02fbfd5f35",
						"subject": "#249: 情報ウィンドウでスクロールバーが表示される"
					},
					{
						"revision": "c7e85157d5250cbe5dcbd83213e3cab706265aa2",
						"subject": "#252: ノートのタイトル入力時にフォーカスが外れる"
					},
					{
						"revision": "40f47369124402ec7304bdbcd5b4b4f4aa76af71",
						"subject": "#250: イメージアイテム削除時に例外"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "ツールバー「自動的に隠す」実行でシステム的に成功したか否かに関わらず隠すように変更",
						"comments": [
							"Windows8で隠れないらしいので暫定的に対処",
							"再現環境がないので何とも言えない",
							"#182に干渉するけどまぁ、うん"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/03/07",
		"version": "0.55.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "フィードバック用ページを作りました",
						"comments": [
							"タスクトレイコンテキストメニュー → 情報 → フィードバック から遷移できます"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "ac5959d98883d26dad53ca20a942c3e0a3b99839",
						"subject": "#168: DBの論理削除後始末"
					},
					{
						"revision": "98e707adb5d3e12c9758e45b6e1c71911c703412",
						"subject": "#169: DBのアナライズ",
						"comments": [
							"一定タイミングで REINDEX, ANALYZEを指定なし実行"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "91a535b1b963dc92e4ec535f454700aada3988dd",
						"subject": "#247: 画面解像度を二回以上切り替えるとツールバー・ノートの位置・サイズがおかしい",
						"comments": [
							"ディスプレイ関係は切り替えやすいラップトップで作業してて分かったけど、tpscrex.exe(ThinkPadの解像度変更用ユーティリティ)を使用した場合にのみ強制的にリサイズされノートのサイズがおかしくなる",
							"ツールバーも同じようにサイズ・位置が強制的に変更されていたがデスクトップツールバーとしてPe側でさらに強制的にリサイズしていたので表面化しなかった模様",
							"なので発生し得る環境がかなり限定されるが修正箇所自体は全環境に恩恵があると思うのでマージした"
						]
					},
					{
						"revision": "a0108537a5ec63f3a52eb9d1e5da24700ec33920",
						"subject": "#246: クリップボード/テンプレートのアイテムリスト上コマンドボタンが消えない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "#242: 古いブランチいらねーんじゃねーの？",
						"comments": [
							"タグは全履歴あるしrcブランチ全部消してもいいけど過去3世代くらい残しとく"
						]
					},
					{
						"revision": "ff1b9793ff8d78bd188f57a58ce44c61d515075f",
						"subject": "#240: フォーラムへの書き込み",
						"comments": [
							"フォーラムへの書き込みはメールアドレスが必須になってくるのでGoogle フォームを用いた方式にした"
						]
					},
					{
						"revision": "34e3066e6e913d5c42681b81c25191f2da1807cd",
						"subject": "SQLite を 1.0.94.0 から 1.0.96.0 にバージョンアップ"
					},
					{
						"revision": "756dc38de309638bac4c04755b4ea14e89981f9e",
						"subject": "ショートカット登録処理でツールバーと設定画面の重複部分を統一"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/28",
		"version": "0.54.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "使わない人は一生使わないであろう機能を頑張って実装"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "0d41712756053148048e0b0bfab350b44b180918",
						"subject": "#222: クリップボード/テンプレートアイテム一覧にフィルタor検索機能追加",
						"comments": [
							"フィルタリング機能を追加しました",
							"フィルタリング中はテンプレートアイテムの追加・移動が抑制されます"
						]
					},
					{
						"revision": "89cde990d7b393ecdb2de95dd496a99c76818f50",
						"subject": "#233: テキストテンプレート拡張",
						"comments": [
							"置き換え処理が有効であればさらにT4テンプレートエンジンを使用した置き換えを行えます",
							"プログラム書ける人でかつ大規模なテンプレートを書かない人が対象です",
							"T4はMono.TextTemplatingを使用しているためMS製T4と動作が違うかもです",
							"暗黙的に <#@ template language=\"C#\" hostspecific=\"true\" culture=\"使用言語の言語コード\" #> が先頭行に挿入されます",
							"Pe側で __host(内部使用), app(IReadOnlyDictionary<string,object>) を予約します。Peの提供するデータには app[string] でアクセスしてください",
							"将来的にはもうちっと頑張ろうと思いますがとりあえず#233実装はここまで"
						]
					},
					{
						"revision": "a68f08edf4921eff8b339778ad30e19be0d11168",
						"subject": "#235: β版をとりあえずすぐ試せるようにする",
						"comments": [
							"<Pe>/bat/beta.bat を実行すると現行バージョンに影響することなくβバージョンを実行することができます",
							"beta.bat 実行時に デスクトップ/Pe-beta ディレクトリが存在しなければ現行バージョンの設定データ(デフォルトパス)を デスクトップ/Pe-beta ディレクトリにコピーします",
							"あくまでβバージョンとして動作させるための機能ですのでリリース版で実行する意味はありません"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "9b1d3dc746847260a64425985c4a038b7b199c8f",
						"subject": "設定データのバックアップファイル拡張子が ..zip となっていた不具合の修正"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/21",
		"version": "0.53.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "開発中のリリース構成アーカイブをCIに追加しました",
						"comments": [
							"詳細はプロジェクトページを参照してください"
						]
					},
					{
						"class": "compatibility",
						"subject": "アップデートに限り標準入出力のフォント設定が本バージョン初回起動時にリセットされます",
						"comments": [
							"標準入出力関係の設定データを内部的に独立させました",
							"元の設定項目が一つだけでロジックに影響せずUIだけが影響されるものであるため下位互換を維持するだけの価値がないのでバッサリ切りました"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "00618396318a5197cdd977dc1df08614b048076e",
						"subject": "#219: 画像データのクリップボード重複判定"
					},
					{
						"revision": "4ad62e273c4477e5d944c0032270e375b4f2be45",
						"subject": "#228: 標準入出力画面に色を設定する"
					},
					{
						"revision": "9619719ab7e2c2d70e3a7c11257c04e9b12cc711",
						"subject": "#229: スピンコントロールにデフォルト値を示すようにする",
						"comments": [
							"コンテキストメニューでデフォルト値に戻します"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "4983d06b4b23773fdc8b698a970f1db2f368e2d8",
						"subject": "#230: ログウィンドウが地味に身長伸びてね？"
					},
					{
						"revision": "0ae956498965c3e1e550a1690614af0f5a753d0a",
						"subject": "#231: 言語ファイルに clipboard/wait/message が未定義"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "d8d9bfec547304962930674e67e035c56c004180",
						"subject": "β版出力をCIに追加"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/18",
		"version": "0.52.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "本バージョン(Pe 0.52.0)未満でのアップデート処理は詳しく調査すると笑顔で地雷原を走り回っている状態でした",
						"comments": [
							"バージョンアップに失敗し、プログラムが強制終了した場合はPeを再起動してタスクトレイコンテキストメニュー → Pe情報 → アップデートを実行してみてくださいよ！",
							"どうしようもなくアップデートできない場合は https://bitbucket.org/sk_0520/pe/downloads からダウンロードしてください。。。Vectorは公開依頼してから公開までが遅いのですよ！",
							"もう大丈夫だ、大丈夫、これで落ちない、大丈夫大丈夫。大丈夫だから忘れよう"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "69800b89e653f7d52952c922175dff628b3bc5b2",
						"subject": "#225: クリップボード/テンプレートのアイテムをクリップボード経由でテキスト転送した後クリップボードの履歴が取り込めない",
						"comments": [
							"対応に伴い設定 → クリップボード/テンプレート → Pe操作猶予時間 を廃止しました"
						]
					},
					{
						"revision": "f60c25a3be357ccb77773b50f0ee0dbe52f9da69",
						"subject": "#223: 標準入出力をファイルに保存した時改行がLFになる"
					},
					{
						"revision": "3085e5cf2f5377e9865b1f4bf9ed011ce18f13a9",
						"subject": "#226: 標準入出力の出力クリア後に標準入力が行えない"
					},
					{
						"revision": "c54593bf8850c44340097f7cabfd6f7355a47e9c",
						"subject": "#224: タスクトレイコンテキストメニューのツールバーアイコンがWin7以下とWin8以上で意味合いが異なる",
						"comments": [
							"Win8以上のアイコンに合わせる"
						]
					},
					{
						"revision": "5b73080306b3b600efcafea820a31056ada21e6a",
						"subject": "#227: アップデートチェック時に死ぬ、再び",
						"comments": [
							"たまに報告いただいてたなーんもしてないのに落ちたってのは恐らくこれが原因かと思われます"
						]
					}
				]
			}
		]
	},
	{
		"date": "2014/02/17",
		"version": "0.51.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "スリープだとかロックだとか休止状態だとかでアップデートチェック用のタイミングがボロボロだったのでログオンのみに限定しました"
					},
					{
						"subject": "Pe 0.44.0-0.50.0でアップデートチェックからの自動アップデートで死ぬかもなのでご注意を",
						"comments": [
							"本バージョンで対応したつもりですよ",
							"アップデート用スレッドとか無関係そうなツールバーのリソース処理とか色々あったぽいのですよ",
							"バージョンアップに失敗し、プログラムが強制終了した場合はPeを再起動して(Windowsセッション接続維持中に)タスクトレイコンテキストメニュー → Pe情報 → アップデートを実行してみてください",
							"どうしようもなくアップデートできない場合は https://bitbucket.org/sk_0520/pe/downloads からダウンロードしてください。。。Vectorは公開依頼してから公開までが遅いのです"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "60b5f56f14fe02c3574a1393aa87e765aeaf2258",
						"subject": "#220: クリップボード重複判定に範囲を含める",
						"comments": [
							"履歴数やその内容によって負荷が異なりますのでユーザー設定で対応してください",
							"Pe 0.50.0の挙動と同じにするには値を 1 に設定してください"
						]
					},
					{
						"revision": "62eb5eede80f08c3ede1305fa70cceb17d883a4d",
						"subject": "#221: クリップボード/テンプレートウィンドウからの選択データを前回フォーカスウィンドウに転送する"
					},
					{
						"revision": "a2e6f85f50724a8df49acdf9d61bbd61d094bde7",
						"subject": "#181: 標準入出力ウィンドウをもうちっとこう……"
					},
					{
						"revision": "5b7e5f5a73e30c7d9fcf737aa223b69bfcd1e29f",
						"subject": "ランチャーアイテム種類がコマンドの場合に種類がファイルの時と同じような動作を行う",
						"comments": [
							"コマンドアイテムでも標準入出力を操作できるようになりました",
							"管理者として実行するにはきちんとファイルアイテムで登録してください",
							"指定コマンドが実行可能か、設定パラメーターが伝搬するかはコマンドに左右されるため注意して下さい",
							"コマンドアイテムはパラメーターが設定済みであることを期待します。その都度パラメーターを変更する用途にはファイルアイテムが適切です"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "e5cf1ba9246ae0051d0f6fc68069493b69361339",
						"subject": "#101: DPIが開発環境と異なる場合に色々と残念",
						"comments": [
							"とりあえず、とりあえず動くようにした",
							"本対応に伴い情報ウィンドウのレイアウトが変更となりました"
						]
					},
					{
						"revision": "b95b2c0a88c2fa65a9e27280947a2861d746cd25",
						"subject": "#218: アップデートチェック時に死ぬ",
						"comments": [
							"セッションを引き金とするアップデートチェックをログオン時に限定",
							"ツールバー破棄後のウィンドウハンドルアクセスも同時に修正"
						]
					},
					{
						"revision": "a4effdd845104b71dcc4b44377beafd6f0bda8c3",
						"subject": "クリップボード/メニューウィンドウの切り替えメニューにチェックをつける"
					},
					{
						"revision": "e3455b380051480a5e34dd5456c8ae5ec2c3b20f",
						"subject": "クリップボード/テンプレートのアイテム一覧をマウスホイールでアイテムに紐付くコントロールが再描画されない不具合の修正",
						"comments": [
							"これ前に対応した気がする"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "78e314c6cfd0c13c95e697ac39fd17df186dc7e1",
						"subject": "別スレッドからのクリップボート・テンプレートのリスト変更を安全にする",
						"comments": [
							"今のところ呼び出し自体はUIスレッドだから不具合にはなってないはず",
							"……はず"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/02/15",
		"version": "0.50.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "#216対応によりランチャーアイテム名の重複を許容しなくなりました。",
						"comments": [
							"旧バージョンや手動設定でランチャーアイテム名を重複させた場合に動作が不安定になる可能性があります"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "0062f3516b199b5219ac5ff1f6aefc6e1c5ad49f",
						"subject": "#217: 取得したクリップボードが直近のクリップボードアイテムと同じであれば履歴に追加しない",
						"comments": [
							"画像の判定はちょっと厳しいので後回し",
							"画像は暇な時にビット深度が固定・変動するのか調べて実装する"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "9211586bc2b66d7a20ce61e3ca85dd00d86551a5",
						"subject": "#215: ランチャーアイテム設定画面のコントロールのアイコンが笑ってる"
					},
					{
						"revision": "1e4d5b862b9817d1caf528c7edef475546ac88bb",
						"subject": "#214: 設定→クリップボード/テンプレートのタブインデックスが狂ってる"
					},
					{
						"revision": "10af64c6dc453a942136a7da9997e8822d00f509",
						"subject": "#216: ランチャーアイテム名が重複していても登録できる"
					},
					{
						"revision": "7972ea041fbef24aff4cf090b204d829c9fafba5",
						"subject": "ランチャーアイテム変更時にグループ内アイテムも追従させる"
					},
					{
						"revision": "ca580dc9c26948838934067e5a049453fb84fe32",
						"subject": "AppbarForm.Dispose中にInvalidOperationException"
					},
					{
						"revision": "2c9c6d75b07090a3d32dee8491e1976acda09987",
						"subject": "#213: 画像を含むクリップボードデータの取得に失敗する"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/13",
		"version": "0.49.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "クリップボード周りが癌細胞化してる"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "166f3b7a796f7517b13525ddd1023c869a75ccba",
						"subject": "#184: クリップボード履歴の保存",
						"comments": [
							"圧縮してはいるものの、データよっては保存ファイルのサイズが大きくなるので保存機能はデフォルトでは無効になっています",
							"保存機能を有能しても保存種別が未チェックであればデータは保存されません",
							"保存種別(特に画像)によっては保存ファイルサイズが肥大化しますので注意してください"
						]
					},
					{
						"revision": "8771dac472311aae4f1a1f2fee692b44205023ae",
						"subject": "#209: 自動的に隠す状態でのD&D"
					},
					{
						"revision": "e8a5c4791cc5e7100e44aba19e949f19998f5a9b",
						"subject": "#206: クリップボードプレビュー画面の機能改善",
						"comments": [
							"HTMLクリップボードはクリップボード取得時のURIの表示・コピーを追加しました",
							"画像は初期状態でウィンドウサイズに合わせて表示されます",
							"画像を原寸表示している場合に左クリックのD&Dでスクロールします"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "f7ee22c7b013c085540afe8abb54787da97b835b",
						"subject": "#205: ツールバーのグループ選択コンテキストメニューに対するチェックマーク"
					},
					{
						"revision": "e199016d9f61204f65accf0c10dc3530f535099e",
						"class": "compatibility",
						"subject": "#144: UpdateScriptをPe/etc/scriptに移動する",
						"comments": [
							"多分大丈夫なんでPe 0.44.0からの下位互換維持を打ち切り"
						]
					},
					{
						"revision": "3cc54f61292f9b88f4229c7cb9cefe6c370c30e7",
						"subject": "#207: ツールバーメインボタンのホイールクリックでメニューが表示されない"
					},
					{
						"revision": "7b1c94a748eeffaed50f84a703666482de8d6d8a",
						"subject": "#208: クリップボードが空で保持クリップボードも空の場合にタブ移動で表明違反"
					},
					{
						"revision": "a3e3842468a1e2e5e5f20d082c6c0f2be9640f85",
						"subject": "#211: 使用許諾ウィンドウのボタンがなんか変"
					},
					{
						"revision": "ce755ce057ebfbad3003f47a65d5c5dfd61d196c",
						"subject": "使用許諾でキャンセルするとNullReferenceExceptionが投げられる嫌がらせみたいな不具合の修正"
					},
					{
						"revision": "5029100f2ef16deeb7629ab897f4ee78c658060e",
						"subject": "クリップボード/テンプレートアイテムのリストアイテム選択変更時におけるちらつきを抑制"
					},
					{
						"revision": "f8e255c719d4638adbe3b53f8532dc7c3732cd99",
						"subject": "#212: クリップボード/テンプレートウィンドウで画像を含むデータを個別で二回以上破棄すると例外"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "fcaddc12f59f9de232ea59e2dc4315ed11c57c35",
						"subject": "#204: Control.Tagの絶滅",
						"comments": [
							"ToolStripUtility.AttachmentOpeningMenuInScreen(Control)だけは諸事情により無理"
						]
					},
					{
						"revision": "df2f5fa6f0eab035dfa15370193f5b5ab8204530",
						"subject": "System.TimeSpanのシリアライズ・デシリアライズ統一"
					},
					{
						"revision": "800662f938031704d7bbda79f75a77796c1bce50",
						"subject": "スキン置き換え画像はPeMainで保持しない"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/11",
		"version": "0.48.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "c2586268379c8eda8592699b18caa332afcd4952",
						"subject": "#203: 設定画面のツールバーグループ設定を直観的にする",
						"comments": [
							"グループノード選択時にランチャーアイテム一覧をダブルクリックすると選択中ランチャーアイテムが追加されます",
							"各ノードをD&Dで移動出来るようになりました"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "733fb123f24d29a6c2fa4a7763c0a4bce5c3991b",
						"subject": "#159: イベントに割り当てたラムダ式のメモリ解放",
						"comments": [
							"とりあえず見える範囲でキャプチャ切ったから勘弁して"
						]
					},
					{
						"revision": "49286e2c2acd9587f56e5bdc02e41fee7da13e65",
						"subject": "#202: グループ作成で名前に重複がある場合に例外が発生する"
					},
					{
						"revision": "71940970976125a393c0a186e1cfcf462dd6a7c3",
						"subject": "ファイルメニュー展開後に読み込み終了したアイコンを待機中イメージから置き換える",
						"comments": [
							"全項目に適用するとアホみたいに遅くなるし現在表示項目数が取得できないので上位項目に適用する",
							"ファイル数が多いとクッソ怪しい動作",
							"system32なんて誰も表示しないだろうという一握の望み"
						]
					},
					{
						"revision": "e24fa1d27cb81f2e7e39bb8920a2d51e421082b0",
						"subject": "ログウィンドウで一部のログが表示できない不具合の修正"
					},
					{
						"revision": "589affc8d5d144791fda6d4f80e3e0859a326617",
						"subject": "大きなアイコン取得時にIImageListが生成できずInvalidCastExceptionがブン投げられる",
						"comments": [
							"再現性皆無でデバッグ時しか発生を確認できていないのでとりあえず空catch"
						]
					},
					{
						"revision": "0182cf25e210342ef2dbf72a80b275827a36678c",
						"subject": "クリップボード/テンプレートのタブ一覧が設定ウィンドウ確定後一時的に無効になる不具合の修正"
					},
					{
						"revision": "f95bb3b6afa6f859db0f0d254d64b1dfa2b0e2a8",
						"subject": "タスクトレイコンテキストメニューに表示されるクリップボード/テキストテンプレート項目表示文字列をウィンドウ名に合わせる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "5fdaf73ea51d2a1e9b638833655f14a8a0f3eb04",
						"subject": "ホームダイアログのボタンがなんかちっさくなってたんで考えることを放棄してDock.Fillした"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/08",
		"version": "0.47.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "バグ修正！"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "5b5bc48e8ca1acfe43b61a05f48a747d58dc44e6",
						"subject": "#99: オーバーフロー時のツールチップ表示"
					},
					{
						"revision": "c98046857eae6e790a48ac2845f94711b8a4d8b7",
						"subject": "オーバーフロー表示されたランチャーアイテムの描画をスーパークラスに委譲する"
					},
					{
						"revision": "a10fef85a324e360368e513758e672ec5b5c5715",
						"subject": "#182: ツールバーの自動的に隠す状態はタスクバーに干渉するべきでない"
					},
					{
						"revision": "664ebe458e7a5ac24afdc4cb220a73e567881efc",
						"subject": "#201: 自動的に隠す非フロート状態からフロート状態にすると表明に引っ掛かる"
					},
					{
						"revision": "2819d8a092d3aedbc809ee623ca3111c6db1a4e1",
						"subject": "#200: 指定して実行ウィンドウのアイコンが汚い"
					},
					{
						"revision": "f0bc4c1e0bd489e86cf2156602a2ec6989260a35",
						"subject": "複数のツールバーを表示での各不具合を修正",
						"comments": [
							"ツールバーへファイルをD&Dで登録した際に対象ツールバー以外のグループがクリアされる",
							"ランチャーアイテムをD&Dして並べ替えた際に他のツールバーのアイテム順序が追従しない"
						]
					},
					{
						"revision": "8cbd6f6c7cfeec80b19fe8a4846c9d725f2e05d2",
						"subject": "ツールバーのランチャーアイテムをホイールクリックしてもランチャーアイテムメニューが表示できなくなっていた"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/06",
		"version": "0.46.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "テンプレートの置き換え書式 VER-* が#198対応で書式変更となりました。下位互換は#199により保たれます"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "8419d4e468725416f96435de485b62f6d7900b58",
						"subject": "#197: テンプレートにクリップボード置き換えを追加",
						"comments": [
							"CLIP",
							"CLIP:NOBREAK",
							"CLIP:HEAD",
							"CLIP:TAIL"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "c7499cc0c72f442a61faee79447d3d346ffc38dd",
						"subject": "#196: クリップボード/テンプレートウィンドウがリサイズできない"
					},
					{
						"revision": "0e92137eaf4d0c9722584c5e712e3da3fab3d2d5",
						"subject": "#195: 非インターネット接続環境だと毎回アップデートチェックに失敗するのにログが出てウザい"
					},
					{
						"revision": "7aa5eb0dec14806a6a82d51ed83d9ea1b1d4287e",
						"subject": "#198: テンプレートのバージョン書式を他の項目に合わせる",
						"comments": [
							"VER-FULL -> VER",
							"VER-NUMBER -> VER:NUMBER",
							"VER-HASH -> VER:HASH"
						]
					},
					{
						"revision": "1dea15676f0727295106dfa429ac8a6f8f4aed4d",
						"subject": "#193: クリップボード取得が重い",
						"comments": [
							"難しい、開発機だと再現できん。ロジックは少し変えたけど再発したらまた考える"
						]
					},
					{
						"revision": "379ef83c9b645ce8e9e017f5a4d087fa3115d533",
						"subject": "#171: SystemSkinのでっかいリソース",
						"comments": [
							"PeMainに組み込んでいたSystemSkinをリソースと供にDLL化した"
						]
					},
					{
						"revision": "db81154964d2105023190f87132b556ad43b3c67",
						"subject": "設定ダイアログ内でノート各行の高さを本文に合わせる"
					},
					{
						"revision": "2a81a822af45849a87fd1f0ea9aed78413d867c1",
						"subject": "#191: タブインデックス整理"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "e389103cb2f69e10df08106585d103fe7756fdbc",
						"subject": "標準CSSに開発時のゴミが混入していたので消しといた"
					}
				]
			}
		]
	},
	{
		"date": "2015/02/04",
		"version": "0.45.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "クリップボードウィンドウにテキストテンプレート(#154)機能が追加されました",
						"comments": [
							"定型文をアイテムとして保存、それをクリップボードへコピーします",
							"置き換え処理を使用すれば実行時の年月日などを設定できます"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "35b28628c3c076867594504a5d50e2490be7ac6b",
						"subject": "#154: 定型文のテンプレート"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "77a41f42b8272e1960557bd509ac123b14f97125",
						"subject": "設定データのXML出力時に改行のCRLFがLFになっていた不具合の修正"
					},
					{
						"subject": "#154対応により「クリップボード」ウィンドウの表示文言を「クリップボード/テンプレート」に変更"
					},
					{
						"revision": "34f1db6c15679c757ae7b1206dd41cfc177fe726",
						"subject": "クリップボードリストの最終アイテムを削除した際にArgumentOutOfRangeException初回例外が発生する",
						"comments": [
							"触った感じデバッガ噛ませた場合だけだと信じてる"
						]
					},
					{
						"revision": "9fe970b2f13eaf5c8bba43a02bd08556e0a2483d",
						"subject": "#188: ツールバーへのD&Dでメインメニューへカーソルを合わせると落ちる"
					},
					{
						"revision": "b8a17da5257ea78ea1921a5eb8aeb82cefc08437",
						"subject": "ツールバーへのD&D時にカーソル下のランチャーアイテムが組み込み・ディレクトリの場合もD&D不可とする"
					},
					{
						"revision": "0feb176f9729f2f0d74250f9930334aa30b8de40",
						"subject": "ツールバーへのD&D終了後にメッセージボックスを表示した際、メッセージボックスが背面に表示されるUIを改善"
					},
					{
						"revision": "3152fe5638ec901a11e05f052f57c4e4afe3aebd",
						"subject": "#190: clean.bat消してない"
					},
					{
						"revision": "040e6b47d01b61339c534d5d5a46249df9363dba",
						"subject": "クリップボードウィンドウ周りの文言を変更"
					},
					{
						"revision": "0b06e1f09a492ea92921ecf4d98418d76ddb0e0d",
						"subject": "#185: ファイルメニューに表示するアイコンがシステムのものでスキン所属ではない"
					},
					{
						"revision": "b2396cb9ff97a1c03b8acf980e73fb0f93cacb7d",
						"subject": "#185対応により一部アイコン使用部分まで伝搬"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "9681d49628e58b861729ac73ae46e08c235296d8",
						"subject": "COMオブジェクトをラップする"
					},
					{
						"subject": "0.44.0の更新履歴#186にリビジョンが設定されていなかったので追加"
					},
					{
						"revision": "c20c62c8192240e53348ba97be2c432b5932a248",
						"subject": "#194: 非リリース版のリビジョンバージョン変更"
					},
					{
						"revision": "28c638381428009492ee9cc5680378a2337b2861",
						"subject": "あんまり関係ないどうでもいい変更",
						"comments": [
							"sbin/Updaterが戻り値を返す",
							"CLIでのキー押せ催促を統一"
						]
					},
					{
						"revision": "4a8935ba3110993533457cbac259820bb3387c97",
						"subject": "#189: UpdaterScript.cs実行時にmscorlibを読み込むか",
						"comments": [
							"大丈夫、いけるいける信じろって"
						]
					}
				]
			}
		]
	},
	{
		"date": "2015/01/31",
		"version": "0.44.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "大量のリソースリークが発生していたため一生懸命修正したのです",
						"comments": [
							"(´◔౪◔) 反省してまーす"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "da4da48ea40e16fbeb1a6321820025a352bc07bb",
						"subject": "#75: アップデート確認を定期的に行う",
						"comments": [
							"とりあえず現状では以下のタイミングで処理する",
							"起動",
							"設定保存",
							"ホームダイアログ終了",
							"セッション接続接続",
							"ロック解除",
							"システム再開"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "447134f2e1da7933d3e286474705acfabaab012c",
						"subject": "#180: メニューの罫線クリックでメニューが閉じる"
					},
					{
						"revision": "40af874b72280096acc55f0dedf358e97021ac71",
						"subject": "#144: UpdateScriptをPe/etc/scriptに移動する",
						"comments": [
							"スクリプト自体は移動したが下位互換等のため/sbin/Updater, /etc/script/Updater を同時配布して整合性を保つ"
						]
					},
					{
						"revision": "1aa8fa57442132abe9e08f5e6a4deaf3b967f54b",
						"subject": "48px以上のアイコンを読み込み時にリソースを持つファイルでリソース境界範囲外にアクセスする不具合の修正"
					},
					{
						"revision": "9f5a11833cbce15707872ed25975e49d65ae4ac0",
						"subject": "#183: ファイルメニュー構築処理を速度改善する"
					},
					{
						"revision": "a27f68b64d054e24d0a3ac3f165f6d5e04a0ad2b",
						"subject": "#187: COMの参照が解放されない"
					},
					{
						"revision": "fe0aebfc230e0d6459a715a117eadd782bf72638",
						"subject": "#186: GDIオブジェクトが解放されない",
						"comments": [
							"アイコン取得時に一回のアクセスで取得できない場合があるので数回アクセスするように変更",
							"通常アイコン取得時、API成功でもアイコンハンドルが取得できてない場合に後続処理を行わない"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "0489176fc54f793dbdac09effb524d09d7dfb6f7",
						"subject": "#172: changelog.xmlの補足事項",
						"comments": [
							"構成自体をざっくり修正"
						]
					},
					{
						"revision": "f403270f80000b5675dfad32222199b1d34104c2",
						"subject": "IF適応の漏れを修正"
					},
					{
						"revision": "3c264cd5b1b3c524dcebc3d09e85c041127c0727",
						"subject": "DBManager担当処理を分割"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/25",
		"version": "0.43.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "ファイル・ディレクトリアイテムのファイルメニューからディレクトリを開くには下位メニューから「ここを開く」を選択してください"
					},
					{
						"subject": "ファイルアイテムのファイルメニュー第一階層目はパスメニューから代用できるため「ここを開く」はありません"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "c65b4ed19b2eacc8b93ae130ce779ad363b6d54d",
						"subject": "#175: 設定画面のディスプレイ識別を視覚的に行う"
					},
					{
						"revision": "97ea508e7ab1e34ec988da8b53ff73fe49338d5a",
						"subject": "ファイルメニューを表示する際にシステムが隠しファイルを表示する設定であれば該当ファイルを半透明で表示する"
					},
					{
						"revision": "2c9de6cfd57c2a85ff2535dcd539de1c6cd36890",
						"subject": "SystemSkin: ツールバー文字列描画にシステムのタイトルバー描画処理を使用する"
					},
					{
						"revision": "c63c8d0188451395a33d3ca3278b0efb8d5f061e",
						"subject": "#173: クリップボードウィンドウ表示切り替え時にバルーンを表示する"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "2d432b805313cf5cd6938404c54e387cc4a7c59e",
						"subject": "#174: ファイルメニューが表示されない"
					},
					{
						"revision": "f35f8146e262a58783de927b801ca58e1d3aed79",
						"subject": "#176: ノートの文字列がURLの場合にオートリンクされる"
					},
					{
						"revision": "8f1e77dac716127e4362f8ff2464411f0e1943ce",
						"subject": "#162: 自動アップデート失敗時にPeが復帰しない"
					},
					{
						"revision": "fa03d1e087b4b2b8a04f7b645f1a80e6958830ff",
						"subject": "#177: ノートを本文入力状態で最小化しても本文入力が可能"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "76d69b05c5208a53b2bf180ad166a0b8cf855f24",
						"subject": "各種UIの共通処理をまとめる"
					},
					{
						"subject": "0.42.1の更新履歴で「リリースビルドバッチ整理」が#160になっていたのを#165に修正"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/22",
		"version": "0.42.1",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"revision": "7a38fc15197655ea9a941744f4c0e24860ff5d28",
						"subject": "#167: 標準入出力ダイアログのフォントを設定項目に表示する"
					},
					{
						"revision": "0387abfea5c3af98acba2948271b523240a377c5",
						"subject": "#158: 肥大化するarchiveディレクトリ"
					},
					{
						"revision": "3092a80fd5f4f217d8b1133d444375607192b4b3",
						"subject": "#170: accept.batを同期実行させない"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "bbb2f76b4ae4a92931906718cbde3b8acf2b31a1",
						"subject": "#161: 固定したノートでマウスカーソルが点滅する",
						"comments": [
							"詳細はIssuesを参照のこと"
						]
					},
					{
						"revision": "853a055234c33476e8fd61d6db517c88c43695f6",
						"subject": "#163: クリップボードのUnicodeな文字列"
					},
					{
						"revision": "1544ed313cd5a936dc3af284bfb38c86c0c8a3ed",
						"subject": "Hashのヘルプファイルをリポジトリ参照へ変更"
					},
					{
						"revision": "ad40f830b93720415224e8c874bbceeb46f9bbd0",
						"subject": "一部文言の修正"
					},
					{
						"revision": "8967a05e4fc44ada7ba393d4e2e7b923ac693117",
						"subject": "#68: UNICODEを含むショートカットファイルの読み込みに失敗"
					},
					{
						"subject": "0.42.0のビルド変更により設定項目にデバッグ用UIが表示されていた"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "bbf8b9f7ef559eebd0236fcd4261fd1dd9a0c1d5",
						"subject": "#165: リリースビルドバッチ整理"
					},
					{
						"revision": "e3ec638ee1ca1f8aec93f5aa1eb2878106873185",
						"subject": "#164: switchのDebug.Assert"
					},
					{
						"revision": "34960ebefb5d0820a2d2f7f023bc5f0858773240",
						"subject": "#166: readme.txtをMarkdownにする"
					},
					{
						"revision": "11f6e6e7e3e9f55ec8f7c7729038bfee64490163",
						"subject": "#160: リリースビルド時に変なdefineがある場合にエラーとする"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/20",
		"version": "0.41.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "#55対応によりノートの本文入力方法が変わりました。Windowsの付箋に近くなった感じです"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "c523a87135a248d192384cfa5ab4bda299e86169",
						"subject": "ノートのサイズ・位置変更中は透明にする"
					},
					{
						"revision": "9bf0cd67718bec973bb1411ba3ad6845782d7a68",
						"subject": "#150: システムスキンをきちんと環境に合わせる",
						"comments": [
							"まぁアイコンだけ"
						]
					},
					{
						"revision": "6694402d6870d38bbf6ee9465e0da97462836311",
						"subject": "#153: ノート編集時の初期状態"
					},
					{
						"revision": "799106280493668be468ae6120c171d85e9e2221",
						"subject": "32px以上のアイコンの場合に通常ファイルアイコンにサムネイルを使用"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "f4e785b8e4dacf26bca7fc54f8e020e3853fadb4",
						"subject": "#55: ノートの改行表示"
					},
					{
						"revision": "4d8241d562904003be368d54d57fb2fe12aa2b0b",
						"subject": "#155: デバッグログの条件式"
					},
					{
						"revision": "3f944c4a02e3e24466dd05678c8fb0b9d6d31f1c",
						"subject": "#156: 指定して実行ダイアログをタスクバーに表示する←？"
					},
					{
						"revision": "de599fae36a8824e6d124200c2fb40352ddab396",
						"subject": "標準入出力ウィンドウのツールバーボタンを非アクティブでも有効にする"
					},
					{
						"revision": "799106280493668be468ae6120c171d85e9e2221",
						"subject": "#2: アイコン取得"
					},
					{
						"revision": "47c521ea8aae5df4313f0fccc98b3ae82d05240c",
						"subject": "ホームダイアログで処理後に言語情報が吹っ飛ぶのを修正(多分開発ブランチでしか起こらないけど混入コミット探すのしんどい)"
					},
					{
						"revision": "ea64a3a87d8fda05b0f0ed580e58ad9ae4b0c8f9",
						"subject": "#157: 各ウィンドウ表示時に前面へ移動させる"
					},
					{
						"revision": "e031432aaf5a4947e0f12c65c367780e58805655",
						"subject": "アップデート処理で binPeUpdater.exe.config の削除処理が抜けていた"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/15",
		"version": "0.40.1",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "指定して実行ダイアログをタスクバーに表示する"
					},
					{
						"revision": "029c51b7fb9514b2d8594ca3a40a750f57eee0b1",
						"subject": "#143: 各種イメージリソースをISkinで管理する"
					},
					{
						"revision": "19bd29aefc541502c5f8d3756b70dfd4a0bf368e",
						"subject": "#44: スキン切り替え",
						"comments": [
							"実装のみで切り替えモジュールは含めてない"
						]
					},
					{
						"revision": "dbedd8185a43d391e7ae241269563341887269dd",
						"subject": "#148: ランチャーボタンのメニュー表示方法追加"
					},
					{
						"revision": "3fa85c221ea546fab3a01e95d218362c96f13465",
						"subject": "#145: ログにデバッグ出力"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "指定して実行ダイアログのオプションでディレクトリ選択ボタンがはるか彼方に消えていたのを修正"
					},
					{
						"revision": "987a88a61201f9a6865c5a8378a6e8b41447b81a",
						"subject": "ツールバーのツールチップがRDP等の非Aero環境でわけ分からん描画になっていた不具合の修正"
					},
					{
						"revision": "534ac11a1e56f42099809120d688eb5090e0de51",
						"subject": "#149: クリップボードプレビューでテキストフォントが変わる"
					},
					{
						"revision": "c493dd1b0d6b4c3d423747cf188312a4c90e4413",
						"subject": "#151: フォント設定UIで現在選択フォントを初期値とする"
					},
					{
						"revision": "e730ed868ecc61b9e4d0c3079ab1be5136516cd7",
						"subject": "#146: クリップボード切り替えホットキーの表示が言語ファイル通してない"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "36c19df96db15c6b1c082cd4270dd40cf8b5ae93",
						"subject": "#147: NuGetパッケージをまとめる"
					},
					{
						"revision": "113d6f59d0b1a1cd4d5b100043b4c4b389c9e0d7",
						"subject": "準備だけして使いもしていなかったマウスフックを無効にした"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/11",
		"version": "0.39.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "本バージョンからアップデートチェックに使用するアドレスが変更となります"
					},
					{
						"subject": "XML -> http://content-type-text.net/document/software/pe-update/update.xml"
					},
					{
						"subject": "詳細はオンラインヘルプを参照してください"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "7e5149cbb741c03214f9b5d1a95fd240921c0ba4",
						"subject": "#104: アップデート定義ファイルをリポジトリから外す"
					},
					{
						"revision": "9e6e24366567961f56a3e4de03ade9acfae8ee5c",
						"subject": "#135: コマンド(URI)アイテムに対する引数"
					},
					{
						"revision": "782040d7832c3bf0f3faf40d0c14dd0300e73e41",
						"subject": "コマンドアイテム設定時、プルダウンに環境変数PATHの実行ファイルをリストアップする"
					},
					{
						"revision": "e234d1c035cc4e9a421397e78c26f53b689eb44a",
						"subject": "#134: アップデート実行時にスクリプト実行"
					},
					{
						"revision": "62ba9830a6afdd7986bd6ffa5cf530721fb7d340",
						"subject": "#139: クリップボードウィンドウのホットキー"
					},
					{
						"revision": "452b69ff948c0a46297cc80a1e1d765fccd6137c",
						"subject": "#140: 組み込みアイテムの一覧"
					},
					{
						"revision": "db83e73bc16fd58a54137b0e9db8b3ebb3b563e5",
						"subject": "#142: Hash機能強化"
					},
					{
						"revision": "5fe681ad3f86409322635b13fcfd0d0e91f353f6",
						"subject": "ファイルアイテムの親フォルダを開く際にファイルを選択した状態でエクスプローラを開く"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "7bd3b5202a7b842b0bcd357f345bd679454fa15a",
						"subject": "#141: ホームダイアログの文言"
					},
					{
						"revision": "27f1b25c3efba2b667c8a82a5b6abab2fc6bc740",
						"subject": "#138: 組み込みアイテムの起動後後処理"
					},
					{
						"revision": "e89286a60a585d79d907a054114066d91b636d1d",
						"subject": "情報ダイアログのバックアップボタン削除"
					},
					{
						"revision": "5fe681ad3f86409322635b13fcfd0d0e91f353f6",
						"subject": "ファイルアイテムの作業フォルダに環境変数を含んでいる場合に、展開前パスがディレクトリパスとでない場合に正常にフォルダを開けない不具合の修正"
					},
					{
						"revision": "13e7be2ba5ce325c942992d3f26d9cbbc1cd72b2",
						"subject": "バックアップファイル世代対象を*.zipに限定し、バックアップ対象をディレクトリまで広げる"
					}
				]
			}
		]
	},
	{
		"date": "2015/01/03",
		"version": "0.38.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "d42011a11cf4e9eba9379e669b1506b2134d821d",
						"class": "compatibility",
						"subject": "#41: アイテムの種類",
						"comments": [
							"種別「URI」を廃止して「コマンド」を追加しました。下位互換のためURIアイテムの読み込みはサポートされますがコマンドアイテムへ変換されます"
						]
					},
					{
						"revision": "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						"subject": "組み込みアイテムを追加しましたが#118対応でのIF試験的意味合いが強いため該当プログラムの機能は弱いです"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "556e62fe00d948335c82f2b548648af6b17515e6",
						"subject": "#130: クリップボード履歴ダブルクリック時に保持データをすべてコピーする"
					},
					{
						"revision": "777bd5716c9b9c1a1f522ffe25f11acbac542e37",
						"subject": "クリップボード監視の切り替え機能を追加"
					},
					{
						"revision": "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						"subject": "#41: アイテムの種類",
						"comments": [
							"組み込みアイテムの追加"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "0.38.0で何をどう頑張っても落ちる不具合の修正。緊急のため0.38.0と0.38.1は統合"
					},
					{
						"revision": "29ff41ed68c8f966c784fec5d05b1fae50d38bba",
						"subject": "#131: HTMLクリップボードの読み取り元が日本語を含む場合に範囲計算が狂う"
					},
					{
						"revision": "c5b893be60bd911c67992af4037bd57d62bdec4c",
						"subject": "#129: HTMLクリップボードデータをファイル保存時にクリップボードデータとして保存している"
					},
					{
						"revision": "3beedc59b897a80cbf5bcba4a66ffd592e349c6e",
						"subject": "#132: RTFの書式が吹っ飛ぶ"
					},
					{
						"revision": "5cf1a43a9d5b503ee2448997d59263a59d16f465",
						"subject": "クリップボード履歴一覧からカーソルが外れた際にボタン一覧を非表示にする"
					},
					{
						"revision": "645ee77f26749c255b64af6ddb62c9af6770d943",
						"subject": "#133: PrintScreenでクリップボードに入んねー"
					},
					{
						"revision": "ed6b3b7cb7cb6333071383ed6646e087a40971d7",
						"subject": "クリップボード履歴一覧の描画がホイールスクロールで変になる"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "2f513e7e823cd0d1399306dff08045bb357ca43d",
						"subject": "#124: GUIコンポーネントのソースをまとめる"
					},
					{
						"revision": "bf46dfbbd00221f810f2954bc2ba3b6e9f241404",
						"subject": "#118: 自前プログラム呼び出し方法"
					}
				]
			}
		]
	},
	{
		"date": "2014/12/23",
		"version": "0.37.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "0.36.0でクリップボード処理がバグりまくっていたので修正しました"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "9bc1cb51706097f0c4aeb7ad35468c91aadedb27",
						"subject": "#128: クリップポ－ドの待機時間を延ばす"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "7e3d641206d9d681a1d19f614799bbe8f3a50472",
						"subject": "#127: HTML形式クリップボードデータ保持"
					},
					{
						"revision": "f1b45e68725c536ebcc9ac7b9c41982b244e7ef7",
						"subject": "#126: クリップボード履歴の項目ボタンが不思議"
					},
					{
						"revision": "14eb786e20344ae907656f79f177c3f409139730",
						"subject": "#125: ファイルをクリップボードへ取り込んだ後ファイル削除→プレビュー表示で例外"
					},
					{
						"subject": "クリップボードアイテムがファイルの場合にファイルが存在しない場合はコピー対象としない"
					}
				]
			}
		]
	},
	{
		"date": "2014/12/21",
		"version": "0.36.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "0b63f714daec6126e46322bc94b692a6bad6071c",
						"class": "compatibility",
						"subject": "#65: 下位互換@IconItem",
						"comments": [
							"Pe 0.29.0 からの下位互換をサポートしなくなりました"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "469eadb5d031563d83fac904cadeee2092fc5b51",
						"subject": "#115: ノートのタイトル入力を直観的に"
					},
					{
						"revision": "97945be931fb097ecdc831580cbbff57fd389a4d",
						"subject": "#113: 指定して実行ダイアログをモードレスにする"
					},
					{
						"revision": "955cefea4f81c0c8127be3dde423ad900ba92e01",
						"subject": "#120: ノートの削除を非拡張コンテキストメニューでも表示する"
					},
					{
						"revision": "1bbe16ee46986aba7663e66e1e59ead95196081a",
						"subject": "#119: クリップボード監視"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "bbb2492c77690d64e1b7be07f38ee9ae4ad05213",
						"subject": "#116: #106のバッチがBOM"
					},
					{
						"revision": "31b395cf4005b2a317b9db0bacce8a7eddc21f65",
						"subject": "#121: DwmGetColorizationColor() が大きめの値を返すと SetVisualStyle() で System.OverflowException"
					},
					{
						"revision": "d7ecb37158ce17dd9829155708752bd28d99d4fc",
						"subject": "#122: ログダイアログの表示位置とサイズが保存されてない"
					},
					{
						"revision": "1cfae55dae07910d523ebb35510f325a0cc99ccd",
						"subject": "#117: ツールバーのツールチップを消す"
					},
					{
						"revision": "97945be931fb097ecdc831580cbbff57fd389a4d",
						"subject": "#114: ウィンドウを親依存でなく独立して保持する",
						"comments": [
							"#113も解決"
						]
					}
				]
			}
		]
	},
	{
		"date": "2014/12/06",
		"version": "0.35.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"revision": "7e2cd76929891a874fb3494899d7b251d3de232c",
						"class": "compatibility",
						"subject": "#54に関連して各種パスを変更しました。ユーザー操作に影響する部分として [Pe]/bin/PeUpdater.exe を [Pe]/sbin/Updater/Updater.exe に変更したためファイアウォール、アンチウイルスソフト等の設定変更が必要な可能性があります"
					},
					{
						"subject": "各種パス変更に伴い過去バージョンの不要ファイルが含まれます。削除するには [Pe]/bat/clean.bat を実行してください"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "d85cf3d12e420d65babf6e64599eaab83b60ed48",
						"subject": "#79: メニューからのツールバー位置を視覚化"
					},
					{
						"revision": "7a6f206fef9bf358c01307d2069f3b82bd3523a6",
						"subject": "#108: ツールバーをユーザー操作で強制的に隠す"
					},
					{
						"revision": "2361a267964b772aa117c2c46745cd3c140746ea",
						"subject": "#110: 言語ファイルのデフォルト"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "93ac22d2e9902221af0cdd36c61b576a5e5e9209",
						"subject": "#56: マルチディスプレイ環境の切り替え",
						"comments": [
							"ディスプレイ位置変更時に一応追従"
						]
					},
					{
						"revision": "17785cfd712d9036a15b6a340c820e674f743b65",
						"subject": "#106: #102対応のバッチファイル含み漏れ"
					},
					{
						"revision": "bb3438add614a2876a141344a2e271b78cb7958f",
						"subject": "#105: 設定→ツールバーの項目順"
					},
					{
						"revision": "c138646841450de048868d33085f2864a79dee5f",
						"subject": "#111: タスクトレイコンテキストメニューが自動的に隠すツールバーに連動して閉じる"
					},
					{
						"revision": "c3ce164c8d39266aa1bb574741333f6252fe2d49",
						"subject": "自動的に隠すツールバーが隠れたときに前回フォアグラウンドウィンドウをフォアグラウンドウィンドウに設定"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "ff86166b83fe66b94389599100633dd1f0aa9647",
						"subject": "#109: build.shの文字コード"
					},
					{
						"revision": "7e2cd76929891a874fb3494899d7b251d3de232c",
						"subject": "#54: 名前空間と各名称がプログラム名(Pe)と直結してる"
					},
					{
						"revision": "33c9b96cd5465fb4ba9a656f09aa4ad5412a1110",
						"subject": "#54によりアップデート後のアセンブリ解決のため PInvoke.dll から PlatformInvoke.dll に名称変更"
					},
					{
						"revision": "cac67b69ba9b8d082ec6cddfc43f9166400d1f35",
						"subject": "各種アセンブリのAssemblyCopyrightを設定"
					},
					{
						"subject": "バージョン 0.33.0 での開発環境変更を追記"
					}
				]
			}
		]
	},
	{
		"date": "2014/11/30",
		"version": "0.34.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "#26対応によりバージョン表記を a.b.c.d から a.b.c.d-xxxx... に変更しました。ユーザー操作に影響はありませんが報告用情報の内容が変更されます"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"revision": "57ce440b369426c073965b71c49a2141398ffe53",
						"subject": "#100: 情報ダイアログからコピーする報告用情報に罫線"
					},
					{
						"revision": "5a519d4305a269af7bdf505c2ca4e1834e0b9972",
						"subject": "#97: コンポーネント情報整理"
					},
					{
						"revision": "bcb2155a0c7905a65b3d4e33756c43a0743bb631",
						"subject": "#102: 使用許諾ダイアログをユーザー意志で再表示"
					},
					{
						"revision": "8c9b33f16ba69bad9036abbd934d3191e035ae65",
						"subject": "#26: git commit hash"
					},
					{
						"revision": "05fb65035835bc4fad48885c3fd3ca0f8109712c",
						"subject": "#63: 自動アップデート時に優しく殺す"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "0d8c9432db5b921669eb6a0343f1ff8cb2460c9b",
						"subject": "#98: 「現在選択中グループ」のツールチップ文字列"
					},
					{
						"revision": "022e666df7daddb560dc3319bce175c798ddfb0c",
						"subject": "#103: 更新履歴のリビジョンを行末に。ついでにスタイルシートちょこっと設定"
					},
					{
						"revision": "7f5a04985721d44741030dd699fdca68749c3265",
						"subject": "ログ表示処理が非リリース構成で例外になる不具合の修正"
					},
					{
						"revision": "2a01315de61388e0b99bb26f01ba4c9bcfb826e6",
						"subject": "設定→ランチャ→その他の入力項目をウィンドウサイズ可変に対応できていなった不具合の修正"
					},
					{
						"revision": "8c9b33f16ba69bad9036abbd934d3191e035ae65",
						"subject": "#26によりバージョン情報ダイアログの表示項目順、バージョン情報・構成情報を入れ替え"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "4f56e98cbfa2680ffb7ed2b7ebcbca8af6b3557f",
						"subject": "#87: DBManager使用時のスパゲッティ具合"
					},
					{
						"revision": "5c6d563da2cb6a30ce3cadbc9176628e881b0f12",
						"subject": "#64: app.config の切り替え"
					},
					{
						"revision": "82d8307979942b749d0e3607464c5d6a1aee5c8f",
						"subject": "PeMain以外の各種アセンブリバージョン修正"
					}
				]
			}
		]
	},
	{
		"date": "2014/11/24",
		"version": "0.33.0",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"revision": "65c905ae5b74109082263b1972aead5d7a6cda30",
						"subject": "#92: 環境変数で指定したファイル アイテムのプロパティが表示されない"
					},
					{
						"revision": "2d6a501969e86fb4776b31777b61731cf725b713",
						"subject": "#93: ファイルアイテムが環境変数を含む場合にファイルメニューが非活性"
					},
					{
						"subject": "#92, #93に関連して環境変数を含む親ディレクトリ、作業ディレクトリのパスコピー・表示の不具合修正"
					},
					{
						"revision": "20f4d6b558d59e70b8aa8f635364d8b5fa003406",
						"subject": "#91: ツールチップがメニューを覆う",
						"comments": [
							"#78を含む"
						]
					},
					{
						"revision": "5f532dd9c09c3d01d0af55c81712e1e2ce029371",
						"subject": "#62: メニューに表示するホットキーが頭おかしい"
					},
					{
						"revision": "888da4e58a8ad763ba8fd73a642e812b0fb31c41",
						"subject": "#95: よろしくないホットキーのメニューショットカット割り当てで例外"
					},
					{
						"revision": "9cc9b57861d742d72e77ee8a8989d386732578f4",
						"subject": "#96: 認証が必要なネットワークでの更新履歴取得失敗"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"revision": "288f2be6be13360028f79515eb571ed5e6e33b36",
						"subject": "#69: ユニットテスト書こうぜ！"
					},
					{
						"revision": "a70ce895c81d7261a93e3345ea0299a3121ce737",
						"subject": "#89: ソース整理"
					},
					{
						"revision": "ef51dcde150cff0a4c3e7b781c43f31312f999a9",
						"subject": "#94: 変更履歴にコミットのリビジョンを含める"
					},
					{
						"subject": "開発環境を SharpDevelop 5.0 から Microsoft Visual Studio Community 2013 に変更"
					}
				]
			}
		]
	},
	{
		"date": "2014/11/19",
		"version": "0.32.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "#82: ポーズ時のタスクトレイアイコン"
					},
					{
						"subject": "#74: ランチャーアイテムをツールバー上で移動させる"
					},
					{
						"subject": "#84: ツールバーメインボタンでグループ切り替え"
					},
					{
						"subject": "#85: ショートカットファイルの登録処理"
					},
					{
						"subject": "#41: アイテムの種類",
						"comments": [
							"URI追加"
						]
					},
					{
						"subject": "#86: 使用者の環境情報を定型として出力"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#80: toolbar/main/tips の ${version-release}"
					},
					{
						"subject": "#77: ShellFolder アイテムのドロップダウン表示で Unhandled exception",
						"comments": [
							"#41により対応不要"
						]
					},
					{
						"subject": "#83: バッチ ファイルにパラメタが渡らない"
					},
					{
						"subject": "ツールバーのボタンサイズに左側余白を若干追加しました"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "Visual Studio Community使いたいでありんす"
					}
				]
			}
		]
	},
	{
		"date": "2014/11/13",
		"version": "0.31.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "グループ名の重複を許容しなくなりました。旧バージョンや手動設定でグループ名を重複させた場合に動作が不安定になる可能性があります"
					},
					{
						"subject": "設定ファイルのバックアップアーカイブから戻しを行う場合はグループ名の重複に注意してください"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "#71: 環境情報出力時にディスプレイ情報を出力する"
					},
					{
						"subject": "#72: ディレクトリのD&D登録時にアイテム種別の選択"
					},
					{
						"subject": "#73: ツールバーに対する初期グループの設定"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#70: ディレクトリアイテムで環境変数が展開されない"
					},
					{
						"subject": "設定ダイアログのランチャーアイコン設定処理のインデックス関連を改善"
					},
					{
						"subject": "#73の影響によりグループ名の重複時に連番を自動採番するように変更"
					},
					{
						"subject": "#73の影響によりグループ名編集時に trim"
					},
					{
						"subject": "#77: ShellFolder アイテムのドロップダウン表示で Unhandled exception",
						"comments": [
							"暫定対応により例外握り潰し"
						]
					}
				]
			}
		]
	},
	{
		"date": "2014/11/09",
		"version": "0.30.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "#67: ランチャアイテムの自動登録",
						"comments": [
							"基盤処理実装、細かい修正や defagroupt-launcher.xml の定義が必要"
						]
					},
					{
						"subject": "設定ダイアログで新規グループ作成時の初期グループ名に連番を設定する"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#66: UNC 環境での SQLite オープン"
					},
					{
						"subject": "ランチャー種別切り替え時の挙動を修正"
					},
					{
						"subject": "マルチディスプレイ環境でホームダイアログが非プライマリディスプレイに表示されることがあったためプライマリディスプレイに固定表示するように変更"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "すっかり忘れていた Hotkey Control",
						"comments": [
							"SpnotetButton をコンポーネント情報に追加と名前空間整理"
						]
					}
				]
			}
		]
	},
	{
		"date": "2014/11/05",
		"version": "0.29.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"class": "compatibility",
						"subject": "将来的な拡張に備えられるよう launcher-items.xml が変更されます。IconPath",
						"comments": [
							"IconIndex 要素は IconItem 要素の子として Path, Index 要素に置き換わります。古い各要素は手動設定を考慮して互換性のため保持されますが将来バージョンでは排除されます"
						]
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "アップデート処理前に設定データを保存"
					},
					{
						"subject": "#61: ${env}を編集する"
					},
					{
						"subject": "リリースビルドのバッチを修正。x86版で[Pe]/x64, x64版で[Pe]/x86を除外した"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ランチャーアイテムのアイコンデータ整理"
					},
					{
						"subject": "使用許諾、アップデートチェック画面のリンク選択時にIEでなくシステムの標準のブラウザでリンクを開くように変更"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "開発環境を SharpDevelop 4.4 から SharpDevelop 5.0 に変更"
					}
				]
			}
		]
	},
	{
		"date": "2014/11/01",
		"version": "0.28.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "システム環境のアイコンを Windows ちっくに置き換え"
					},
					{
						"subject": "タスクトレイコンテキストのノートに現在有効なノート一覧を表示する"
					},
					{
						"subject": "ノートのコンテキストメニューに拡張メニュー実装"
					},
					{
						"subject": "ノートのコンテキストメニュー項目、最小化にアイコン設定"
					},
					{
						"subject": "画面上のウィンドウ表示位置を保存・復帰させる機能の追加"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "タスクトレイ Pe アイコンダブルクリック時の処理がデバッグコードのままだった"
					},
					{
						"subject": "タスクトレイ Pe アイコンコンテキストメニューのツールバーアイコンを変更"
					},
					{
						"subject": "#59: TimeSpanがシリアライズされない"
					},
					{
						"subject": "システムイベントのメモリリークを修正"
					},
					{
						"subject": "#58: メニューに表示するホットキーが近い"
					},
					{
						"subject": "#17: Aero描画の切り替え"
					},
					{
						"subject": "ツールバーの文字列幅を制限"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "changelog.xml の 0.27.0 公開日が10/25になっていたので10/26に直した"
					},
					{
						"subject": "[PE]/etc/style, [PE]/etc/script を追加、それに伴い関連部分を色々と変更"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/26",
		"version": "0.27.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "ツールバーメインメニューに非表示を追加"
					},
					{
						"subject": "#8, #34とかの成果として、タスクトレイのコンテキストメニューを .NET Framework の推奨である ContextMenuStrip に変更(Forms非推奨？ 聞こえんなぁ)"
					},
					{
						"subject": "ContextMenuStrip への変更にあたりアイコンを設定"
					},
					{
						"subject": "ノートのカスタムカラーアイコンを非選択時は固定のアイコンを表示するように変更"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ログフォームのタイトルがアホになっていた"
					},
					{
						"subject": "#57: ツールバーのコンテキストメニューでカーソルが移動用"
					},
					{
						"subject": "通常スキンでノートのキャプションボタンを密着するよう変更"
					},
					{
						"subject": "Windowsからのユーザー切り替え時に表示UIの再描画処理を見直し"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "SystemEvents.UserPreferenceChanged イベントを受信"
					},
					{
						"subject": "SystemEvents.SessionSwitch イベントを受信"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/25",
		"version": "0.26.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "バージョン 0.23.0, 0.24.0, 0.25.0 の 64bit版を使用している場合、アップデートは下記URLから手動で行ってください"
					},
					{
						"subject": "https://bitbucket.org/sk_0520/pe/downloads/Pe_0-26-0_x64.zip"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "64bit版 PeUpdater が 32bit で生成されていたため旧プロセスを殺せなかった"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "PeUodaterプロジェクト設定見直し"
					},
					{
						"subject": "0.1.0 - 0.9.0 までのタグを削除"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/25",
		"version": "0.25.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "ノートの有無によってタスクトレイPeコンテキストメニュー内容の有効・無効切り替え"
					},
					{
						"subject": "ツールバーのグリップ部、ノートのキャプションにカーソルを持って行ったときに移動を示すカーソルに変更"
					},
					{
						"subject": "ノートのコンテキストメニューにアイコンべたべたはっつけてみた",
						"comments": [
							"最小化のアイコンは未定"
						]
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "前面表示処理が死んでた"
					},
					{
						"subject": "ツールバー最小化時に設定状態に関係なく最前面に表示するよう変更"
					},
					{
						"subject": "ドッキング状態により自動的に隠すメニューの有効無効を切り替える"
					},
					{
						"subject": "ツールバー位置のメニュー項目のチェックは丸で表示する"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/21",
		"version": "0.24.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "ノートの現在選択色を親メニューにも表示"
					},
					{
						"subject": "システムレジューム時にアップデートチェック実施"
					},
					{
						"subject": "#20関連として入力処理をサポート"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ノートの前面表示が常時最前面表示となっていた"
					},
					{
						"subject": "#38: 多言語によるUIの自動調整",
						"comments": [
							"設定/本体 気が付けば終わってた"
						]
					},
					{
						"subject": "使用許諾ダイアログ、ホームダイアログ、アップデートダイアログの前面移動を実装"
					},
					{
						"subject": "初回起動時のホームダイアログがウィンドウプロシージャを経由していない不具合を力技修正()"
					},
					{
						"subject": "#20: 準出力取得時に取得ウィンドウを閉じると例外",
						"comments": [
							"閉じないようにした"
						]
					},
					{
						"subject": "標準出力ダイアログのタブに当たる言語設定が古かった"
					},
					{
						"subject": "標準出力ダイアログをツールダイアログから通常ウィンドウへ変更"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "#53: デフォルト引数なくしたい",
						"comments": [
							"ﾋｬｯﾎｰｲ!!"
						]
					}
				]
			}
		]
	},
	{
		"date": "2014/10/18",
		"version": "0.23.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "フォーラム(https://groups.google.com/d/forum/pe_development)作成"
					},
					{
						"subject": "PeUpdate.exe のパスが [Pe]/PeUpdate.exe から [Pe]/bin/ 以下へ移動しました。ファイアウォール、アンチウイルスソフト等の設定変更が必要な可能性があります"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "スタートアップへの登録機能を設定画面に追加"
					},
					{
						"subject": "ホームダイアログの実装。アイテム検索機能は煮詰まるまで無効化"
					},
					{
						"subject": "情報ダイアログのリンクにグループを追加"
					},
					{
						"subject": "タスクトレイアイコンのコンテキストメニューにヘルプ追加"
					},
					{
						"subject": "#6: 言語設定"
					},
					{
						"subject": "ノートを一括で前面表示する機能追加"
					},
					{
						"subject": "ホットキー操作で行われる処理内容をバルーン表示する"
					},
					{
						"subject": "#56: マルチディスプレイ環境の切り替え",
						"comments": [
							"位置は知らんけどディスプレイ数を検知するよう修正"
						]
					},
					{
						"subject": "#51: タスクトレイダブルクリック機能実装"
					},
					{
						"subject": "初回起動時(使用許諾ダイアログとは別ロジック)にホームダイアログを表示"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "情報ダイアログのリンク押下時に訪問済みに色変更されていなかった"
					},
					{
						"subject": "アイコン表示ダイアログのアイコンインデックスを指定出来ていない不具合"
					},
					{
						"subject": "操作性が悪かったためノートの色選択機能をプルダウンからサブメニューへ変更"
					},
					{
						"subject": "使用許諾ダイアログ、アップデート実行ダイアログの誤操作を防ぐため Enter キー押下によるダイアログ標準動作を抑制"
					},
					{
						"subject": "アップデートダイアログのコントロール類を広げた"
					},
					{
						"subject": "初回起動時にフロートだとどこにあるのか分からんということでツールバーの初期状態をデスクトップの右側に変更"
					},
					{
						"subject": "ツールバーが自動的に隠す状態で表示する際にタスクバー位置を考慮していなかった"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "app.config の key 変更"
					},
					{
						"subject": "言語設定 [note/style/color] を [note/menu/color] に変更"
					},
					{
						"subject": "使用許諾の各URIを app.config で置き換える"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/13",
		"version": "0.22.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "ノートの前景色・背景色をノートコンテキストメニューから変更可能に"
					},
					{
						"subject": "全ウィンドウ非表示状態で非表示ウィンドウから表示された場合に復帰させないようにした"
					},
					{
						"subject": "ノートの内容を出力(入力は未実装)"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ノート最小化時の描画処理で本文が描画されないように修正"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "デバッグ時のデバッグ設定見直し"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/05",
		"version": "0.21.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "ObjectDumper使用"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "アップデート処理後に異常処理がなければコンソール画面を閉じる"
					},
					{
						"subject": "情報ダイアログに更新履歴表示ボタン追加"
					},
					{
						"subject": "情報ダイアログから手動アップデートチェックを行う際に確認ダイアログを表示"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "アップデート確認ダイアログに最新バージョンが表示されない不具合"
					},
					{
						"subject": "#41: アイテムの種類",
						"comments": [
							"ファイルとディレクトリで分岐させる。その他は未実装"
						]
					},
					{
						"subject": "ファイルランチャーメニューのファイルで列挙されたディレクトリを選択した際にディレクトリが存在しなければ例外"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "紆余曲折あった更新履歴の原本はXMLで統一"
					},
					{
						"subject": "SQLiteのx86/x64切り替え処理が自動化されたのかなんか知らんけど両方のDLLが含まれるようになってデブくなった"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/03",
		"version": "0.20.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "#49未対応版が本バージョンを正常に落とせるように一時的にRC版をアップデート確認リソースから外す。そのためバージョン0.22.0までRC版は配布しない"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "アップデートの結果ログとログ出力内容をまとめた"
					},
					{
						"subject": "ツールバーのボタンへのD&Dで指定して実行ダイアログを表示"
					},
					{
						"subject": "ログダイアログの詳細部分表示方法を全面と分割の切り替え"
					},
					{
						"subject": "#48: 全ノートに対する操作でロック状態は省く"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "使用許諾ダイアログ内の文言が他のUIテキストと異なっていた"
					},
					{
						"subject": "過去バージョンからの強制使用許諾表示が使用設定より優先される不具合"
					},
					{
						"subject": "マルチディスプレイで自動的に隠したツールバーの隠れた位置と表示位置が変な不具合"
					},
					{
						"subject": "#9: ディスプレイ名を分かりやすく"
					},
					{
						"subject": "#49: アップデートチェック処置でRC版のチェックが死んでる"
					},
					{
						"subject": "#50: アップデートチェック時にキャッシュされたデータを参照する"
					},
					{
						"subject": "#3: ツールバーメニューチェック表示がアイコンサイズに依存"
					},
					{
						"subject": "領域内に収まらないランチャーアイテムのメニュー表示で例外発生"
					},
					{
						"subject": "領域内に収まらないランチャーアイテムのファイル一覧がどんな設定でもやたらスリム"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "ランチャーアイテム種別選択を実装まで無効化"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/01",
		"version": "0.19.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "0.18.0対応としての高速リリース"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ツールバーでコンテキストが表示できないの修正"
					},
					{
						"subject": "#34: 付箋フォームのコンテキストメニューがマルチディスプレイで（ｒｙ",
						"comments": [
							"再修正"
						]
					},
					{
						"subject": "#35: 付箋の再描画",
						"comments": [
							"再修正"
						]
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "PeUpdater大幅改修"
					}
				]
			}
		]
	},
	{
		"date": "2014/10/01",
		"version": "0.18.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "サードパーティコンポーネント一覧追加"
					},
					{
						"subject": "ノートのタイトル描画フォントをデフォルトではシステムのキャプションバーフォントに変更"
					},
					{
						"subject": "フォント設定をシステムのダイアログのデフォルトに変更"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "タスクトレイのコンテキストメニュー表示時にログウィンドウの表示状態が反映されていない不具合の修正"
					},
					{
						"subject": "ログウィンドウがタスクマネージャのアプリケーションに表示されないように変更"
					},
					{
						"subject": "#35: 付箋の再描画"
					},
					{
						"subject": "#34: 付箋フォームのコンテキストメニューがマルチディスプレイで（ｒｙ"
					},
					{
						"subject": "ダイアログのカーソル自動移動処理がバグってた"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "PeMain.Data.Item関連のDisposeをあれやこれや"
					},
					{
						"subject": "アップデート用チェンジログをリリース/RC版で分離、0.20.0で現行チェンジログ削除予定"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/28",
		"version": "0.17.1",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "次回バージョンアップを兼ねて少しだけバージョンアップ"
					},
					{
						"subject": "非RCだがリリース版ではない微妙な立ち位置"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "#45: プログラムの自動更新"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "言語適用順を ${...} -> @[...] から @[...] -> ${...} に変更"
					},
					{
						"subject": "言語設定のキーが存在しなかった場合に<key>としていた処理から<>を付与しないように変更"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "/Pe/changelog.js 追加。 changelog.xml から最新バージョンを取得して /Pe/Update/update.html を作成"
					},
					{
						"subject": "change.log -> changelog.xml"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/23",
		"version": "0.17.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "標準出力取得ウィンドウに最前面固定の切り替え機能を追加"
					},
					{
						"subject": "#42: ホットキーの表示"
					},
					{
						"subject": "ノートのコンテキストメニューから「削除」を取り除く"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "Pe 情報ダイアログに表示されるリンクにメールアドレスを追加"
					},
					{
						"subject": "使用許諾ダイアログ内のリンク遷移を外部で行うように修正"
					},
					{
						"subject": "#21: 標準出力取得ウィンドウの更新"
					},
					{
						"subject": "標準出力取得ウィンドウの更新ツールチップに対して文言が設定されていなかった"
					},
					{
						"subject": "#46: 設定ダイアログから設定保存後の再起動で使用許諾ダイアログ表示"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/21",
		"version": "0.16.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "64bit 対応版配布開始"
					},
					{
						"subject": "情報ダイアログにデバッグ・リリースと対象プロセッサーを表示"
					},
					{
						"subject": "#43: 初回起動時の承認画面"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "ノートのコンテキストメニュー[フォント/変更]を現在選択されているフォントを表示するよう変更"
					},
					{
						"subject": "[Pe]/doc/readme-ja.txt 修正"
					},
					{
						"subject": "設定/使用言語を#6完了まで非活性に変更"
					}
				]
			},
			{
				"type": "developer",
				"logs": [
					{
						"subject": "リリースビルド用に /Pe/build.bat 追加"
					},
					{
						"subject": "配布アーカイブの圧縮形式を 7z -> zip へ変更"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/15",
		"version": "0.15.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "#22: 標準出力取得ウィンドウの機能実装"
					},
					{
						"subject": "#37: 本体設定時におけるノートの各種設定"
					},
					{
						"subject": "タスクトレイコンテキストメニューからウィンドウメニューをルートに移行"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "情報ウィンドウのタブインデックスを直感的に"
					},
					{
						"subject": "クリアアイコンの追加"
					},
					{
						"subject": "#20: 標準出力取得時に取得ウィンドウを閉じると例外"
					},
					{
						"subject": "内部実装: 出力取得でエラー取得時に標準出力とマークされていた"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/11",
		"version": "0.14.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "情報ウィンドウで各種ディレクトリを開くボタンの追加"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#39: タブインデックスの順序を直観的にする"
					},
					{
						"subject": "ツールバーが自動的に隠す状態でメニュー(コンテキスト/ボタン)表示した際にツールバーが隠れる不具合の修正"
					},
					{
						"subject": "設定ウィンドウ/ランチャのリサイズ処理を修正"
					},
					{
						"subject": "設定ウィンドウからツールバー位置変更で位置とサイズが変になる不具合の修正"
					}
				]
			}
		]
	},
	{
		"date": "2014/09/07",
		"version": "0.13.0",
		"contents": [
			{
				"type": "features",
				"logs": [
					{
						"subject": "指定して実行ダイアログへのD&Dで値設定"
					},
					{
						"subject": "指定して実行ダイアログの作業フォルダ欄へのD&Dで値設定"
					},
					{
						"subject": "#1: 情報ダイアログ実装"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "doc/change.log の出力設定を PreserveNewest に設定"
					},
					{
						"subject": "タスクトレイメニューからノート作成でスクリーン中央に表示する"
					},
					{
						"subject": "#38: 多言語によるUIの自動調整",
						"comments": [
							"設定/本体は機能確定まで未定"
						]
					},
					{
						"subject": "プログラムのアイコンとタスクトレイアイコンを統合"
					},
					{
						"subject": "#40: ノート最小化時における本文編集"
					},
					{
						"subject": "ツールバーメインアイコンの修正"
					},
					{
						"subject": "設定保存時にログウィンドウの言語設定に失敗する不具合の修正"
					},
					{
						"subject": "ログウィンドウを閉じた際に設定項目を非表示とするように修正"
					},
					{
						"subject": "ToolStripItemへの言語設定でツールチップ設定がちょっと変だったのを修正"
					}
				]
			}
		]
	},
	{
		"date": "2014/08/29",
		"version": "0.12.0",
		"contents": [
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "#11: ツールバー メインアイコン"
					},
					{
						"subject": "タスクトレイアイコンを変更"
					}
				]
			}
		]
	},
	{
		"date": "2014/08/28",
		"version": "0.11.0",
		"contents": [
			{
				"type": "note",
				"logs": [
					{
						"subject": "お小遣い帳レベルで更新履歴をつけてみる"
					}
				]
			},
			{
				"type": "features",
				"logs": [
					{
						"subject": "#15: ログダイアログの機能実装"
					}
				]
			},
			{
				"type": "fixes",
				"logs": [
					{
						"subject": "付箋とノートの文言をノートに統一"
					}
				]
			}
		]
	}
];/*--------RELEASE TAIL--------*/

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
