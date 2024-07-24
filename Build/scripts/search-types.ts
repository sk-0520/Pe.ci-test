export type JsonValue = {
	kind: "help" | "develop";
	contents: string[];
};

export type SearchMeta = Record<string, JsonValue>;
