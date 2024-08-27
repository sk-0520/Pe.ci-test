const ColumnIndex = {
	primaryKey: 0,
	notNull: 1,
	foreignKey: 2,
	logicalName: 3,
	absoluteName: 4,
	logicalType: 5,
	cliType: 6,
	check: 7,
	comment: 8,
};

export function splitEntities(markdown: string): string[] {
	if (!markdown) {
		return [];
	}

	return markdown.split(/^___$/gm).map((a) => a.trim());
}
