import path from "node:path";
import { type Input, type Options, main } from "./check-release-note";

const rootDirPath = path.resolve(__dirname, "..", "..");

const input: Input = {
	rootDirPath: rootDirPath,
	changelogsJsonPath: path.resolve(rootDirPath, "Define", "changelogs.json"),
};

const options: Options = {
	isRelease: true,
};

main(input, options);
