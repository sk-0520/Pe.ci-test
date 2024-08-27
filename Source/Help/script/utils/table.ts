
export function splitEntities(markdown: string): string[] {
	if(!markdown) {
		return []
	}

	return markdown.split(/^___$/gm).map(a => a.trim());
}

