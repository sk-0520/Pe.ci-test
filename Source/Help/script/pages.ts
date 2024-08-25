export const PageKeys = [
	"help.index",
	"help.install_uninstall_data",
	"help.cpu",
	"help.privacy",
	"help.notify_area",
	"help.launcher",
	"help.launcher_toolbar",
	"help.launcher_command",
	"help.launcher_extends_execute",
	"help.note",
	"help.others",
	"help.others_appsettings",
	"help.others_commandline",
	"help.others_proxy",
	"help.others_plugin",
	"help.search",
	"help.changelog",
	"dev.index",
	"dev.build",
	"dev.branch",
	"dev.ci",
	"dev.table_main",
	"dev.table_large",
	"dev.table_temporary",
	"dev.plugin",
	"dev.plugin_reference",
	"dev.plugin_template",
] as const;
export type PageKey = (typeof PageKeys)[number];

export interface PageElement {
	key: PageKey;
	title: string;
	nodes?: PageElement[];
}

export const Pages: PageElement[] = [
	{
		key: "help.index",
		title: "はじめに",
		nodes: [
			{
				key: "help.install_uninstall_data",
				title: "インストール・アンインストール・保存データについて",
			},
			{
				key: "help.cpu",
				title: "32/64bitについて",
			},
			{
				key: "help.privacy",
				title: "プライバシー",
			},
		],
	},
	{
		key: "help.notify_area",
		title: "通知領域",
	},
	{
		key: "help.launcher",
		title: "ランチャー",
		nodes: [
			{
				key: "help.launcher_toolbar",
				title: "ツールバー",
			},
			{
				key: "help.launcher_command",
				title: "コマンド",
			},
			{
				key: "help.launcher_extends_execute",
				title: "指定して実行",
			},
		],
	},
	{
		key: "help.note",
		title: "ノート",
	},
	{
		key: "help.others",
		title: "その他",
		nodes: [
			{
				key: "help.others_appsettings",
				title: "アプリケーション構成ファイル",
			},
			{
				key: "help.others_commandline",
				title: "コマンドライン引数",
			},
			{
				key: "help.others_proxy",
				title: "プロキシ",
			},
			{
				key: "help.others_plugin",
				title: "プラグイン",
			},
			{
				key: "help.search",
				title: "🔍検索",
			},
		],
	},
	{
		key: "help.changelog",
		title: "更新履歴",
	},
	{
		key: "dev.index",
		title: "開発",
		nodes: [
			{
				key: "dev.build",
				title: "ビルド",
			},
			{
				key: "dev.branch",
				title: "ブランチ",
			},
			{
				key: "dev.ci",
				title: "CI・リリース",
			},
			{
				key: "dev.table_main",
				title: "テーブル:Main",
			},
			{
				key: "dev.table_large",
				title: "テーブル:Large",
			},
			{
				key: "dev.table_temporary",
				title: "テーブル:Temporary",
			},
			{
				key: "dev.plugin",
				title: "プラグイン開発",
				nodes: [
					{
						key: "dev.plugin_reference",
						title: "プラグイン参考実装",
					},
					{
						key: "dev.plugin_template",
						title: "プラグインテンプレート",
					},
				],
			},
		],
	},
] as const;
