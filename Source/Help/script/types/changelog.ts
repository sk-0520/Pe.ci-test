export const ChangelogContentKinds = [
	"features",
	"fixes",
	"developer",
	"note",
] as const;

export type ChangelogContentKind = (typeof ChangelogContentKinds)[number];

export const ChangelogContentItemTypes = [
	"compatibility",
	"notice",
	"nuget",
	"myget",
	"plugin-compatibility",
] as const;

export type ChangelogContentItemType =
	(typeof ChangelogContentItemTypes)[number];

export interface ChangelogContentItem {
	revision: string;
	class?: ChangelogContentItemType;
	subject: string;
	comments?: string[];
}

export interface ChangelogContent {
	type: ChangelogContentKind;
	logs: ChangelogContentItem[];
}

export interface ChangelogVersion {
	date: string;
	version: string;
	group?: string;
	contents: ChangelogContent[];
}

export type Changelogs = ChangelogVersion[];
