import fs from "node:fs";
import path from "node:path";

export interface Input {
	inputFilePath: string;
	outputFilePath: string;
}

export function main(input: Input): void {
	const dir = path.dirname(input.outputFilePath);
	if (!fs.existsSync(dir)) {
		fs.mkdirSync(dir);
	}
	fs.copyFileSync(input.inputFilePath, input.outputFilePath);
}
