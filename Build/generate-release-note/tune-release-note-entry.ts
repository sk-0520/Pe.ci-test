import path from "node:path";
import { type Input, main } from "./tune-release-note";

const rootDirPath = path.resolve(__dirname, "..", "..");

const input: Input = {
	rootDirPath: rootDirPath,
	changelogsPath: path.resolve(rootDirPath, "Define", "changelogs.json"),
	outputChangelogPath: path.resolve(rootDirPath, "Define", "@changelog.json"),
};

main(input);
