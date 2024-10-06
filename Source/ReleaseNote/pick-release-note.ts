import fs from "node:fs";
import { getValue } from "../Help/utils/access";

export interface Input {
	changelogsPath: string;
	outputChangelogPath: string;
}

export function main(input: Input) {
	console.debug({ input });

	const changelogsJson = fs.readFileSync(input.changelogsPath).toString();
	const changelogs = JSON.parse(changelogsJson);
	const changelog = getValue(changelogs, 0);

	fs.writeFileSync(input.outputChangelogPath, JSON.stringify(changelog));
}
