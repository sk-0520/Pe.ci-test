export interface PageElement {
	key: string;
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
