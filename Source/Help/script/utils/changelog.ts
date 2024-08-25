import changelogsArchives from "../../../../Define/changelogs-archive.json";
import changelogs from "../../../../Define/changelogs.json";

export function getChangelogs() {
	const result = [...changelogs];
	Array.prototype.push.apply(result, changelogsArchives);

	return result;
}
