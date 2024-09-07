export const Sqlite3BasicTypes = ["integer", "real", "text", "blob"] as const;
export const Sqlite3AffinityTypes = [
	"datetime", // sqlite3 的には数値っぽいから違うんやけどね
	"boolean",
] as const;
export type Sqlite3BasicType = (typeof Sqlite3BasicTypes)[number];
export type Sqlite3AffinityType = (typeof Sqlite3BasicTypes)[number];
export const Sqlite3Types = [
	...Sqlite3BasicTypes,
	...Sqlite3AffinityTypes,
] as const;
export type Sqlite3Type = (typeof Sqlite3Types)[number];

export const SqliteTypeMap = new Map<Sqlite3Type, Sqlite3BasicType>([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<Sqlite3Type, Sqlite3BasicType>;
