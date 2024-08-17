
export const PageKeys = [
"help.top",
"help.install-uninstall-data",
"help.notifyarea",
"help.launcher"
] as const
export type PageKey = typeof PageKeys[number]

export interface PageElement {
	key: PageKey;
	title: string;
	nodes?: PageElement[];
}

export const Pages: PageElement[] = [
	{
		key: "help.top",
		title: "はじめに",
		nodes: [
			{
				key: "help.install-uninstall-data",
				title: "インストール・アンインストール・保存データについて",
			},
		],
	},
	{
		key: "help.notifyarea",
		title: "通知領域"
	},
	{
		key: "help.launcher",
		title: "ランチャー"
	},
] as const;
