import fs from "node:fs";
import type { ChangelogVersion } from "../../Source/Help/script/types/changelog";
import { getValue } from "../../Source/Help/script/utils/access";

export interface Input {
	rootDirPath: string;
	changelogsJsonPath: string;
}

export interface Options {
	isRelease: boolean;
}

export function main(input: Input, options: Options) {
	const changelogsJson = fs.readFileSync(input.changelogsJsonPath).toString();
	const changelogs = JSON.parse(changelogsJson);
	const changelog = getValue(changelogs, 0) as ChangelogVersion;

	if (!options.isRelease) {
		return;
	}

	for (const { type, logs } of changelog.contents) {
		console.log({ type });
		for (const log of logs) {
			if (!log.subject.trim()) {
				throw new Error("subject");
			}

			if (log.comments) {
				const hasError = log.comments
					.map((a) => a.trim())
					.filter((a) => !a).length;
				if (hasError) {
					throw new Error(`${log.subject}: comments`);
				}
			}
		}
	}
}
