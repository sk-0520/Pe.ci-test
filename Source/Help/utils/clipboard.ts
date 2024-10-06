export async function copy(s: string): Promise<void> {
	await navigator.clipboard.writeText(s);
}
