import path from "node:path";
import { type Input, main } from "./output-release-note";

const rootDirPath = path.resolve(__dirname, "..", "..");

const input: Input = {
	inputFilePath: path.resolve(rootDirPath, "Output", "release-note.html"),
	outputFilePath: path.resolve(rootDirPath, "history", "Pe.html"),
};

main(input);
