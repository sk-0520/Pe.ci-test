export const ChangelogContentKinds = [
	"features",
	"fixes",
	"developer",
	"note",
] as const;

export type ChangelogContentKind = (typeof ChangelogContentKinds)[number];

export interface ChangelogContentItem {
	revision: string;
	class?: string;
	subject: string;
	comments?: string[];
}

export interface ChangelogContent {
	type: ChangelogContentKind;
	logs: ChangelogContentItem[];
}

export interface ChangelogContents {
	date: string;
	version: string;
	group?: string;
	contents: ChangelogContent;
}

export type Changelogs = ChangelogContents[];
