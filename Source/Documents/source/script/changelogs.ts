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
						"subject": "#625: ノートを非表示にした際に元に戻すをサポートする",
						"comments": [
							"以下操作のみを対象とする",
							"Alt + F4",
							"×"
						]
					},
					{
						"revision": "",
						"subject": "#624: ツールバーを提供UI以外から閉じたときに元に戻すをサポートする",
						"comments": [
							"以下操作のみを対象とする",
							"Alt + F4"
						]
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
						"subject": "#633: ランチャーグループ名に _ が存在するとアクセスキー扱いとなっている"
					},
					{
						"revision": "",
						"subject": "#636: 通知ログがカーソル位置指定で通知ログウィンドウにクリック可能なアイテムがある場合は常時追従してはいけない"
					},
					{
						"revision": "",
						"subject": "#628: 出来立てほやほやのノート位置情報が保存されていない"
					},
					{
						"revision": "",
						"subject": "#638: コマンド検索時の0件ヒット文字列表記をまともにする"
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
