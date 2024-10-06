import changelogsArchives from "../../../Define/changelogs-archive.json";
import changelogs from "../../../Define/changelogs.json";
import type { Changelogs } from "../types/changelog";

export function getChangelogs(): Changelogs {
	const result = [...changelogs];
	Array.prototype.push.apply(result, changelogsArchives);

	return result as unknown as Changelogs;
}
