import fs from "node:fs";
import path from "node:path";

import { convert } from "html-to-text";
import * as pug from "pug";
import type { SearchMeta } from "./search-types";

const regNewLine = /\r?\n/g;

function getPugFiles(dirPath: string): string[] {
	const files = fs
		.readdirSync(dirPath, { withFileTypes: true, recursive: false })
		.filter((a) => path.extname(a.name) === ".pug")
		.filter((a) => !["help.changelogs.pug", "help.search.pug"].includes(a.name))
		.map((a) => path.resolve(a.path, a.name));

	return files;
}

function readToHtml(pugFilePath: string): string {
	const html = pug.renderFile(pugFilePath);
	return html;
}

function convertToText(html: string): string {
	const text = convert(html, {
		baseElements: {
			selectors: ["#content"],
		},
		wordwrap: false,
	});
	return text;
}

export function main(
	sourceDirectoryPath: string,
	outputFilePath: string,
): void {
	console.log({ sourceDirectoryPath, outputFilePath });

	const files = getPugFiles(sourceDirectoryPath);

	const output: SearchMeta = {};

	for (const file of files) {
		const filePath = path.parse(file);
		console.debug(filePath.name);

		const html = readToHtml(file);
		const text = convertToText(html);

		output[filePath.name] = {
			kind: filePath.name.startsWith("help.") ? "help" : "develop",
			contents: text.split(regNewLine).filter((a) => a),
		};
	}

	const json = JSON.stringify(output);
	fs.writeFileSync(outputFilePath, json);
}
