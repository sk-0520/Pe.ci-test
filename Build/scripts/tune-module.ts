import fs from "node:fs";
import path from "node:path";

import * as nhp from "node-html-parser";

export async function main(workDirectoryPath: string): Promise<void> {
	const files = fs.readdirSync(workDirectoryPath);
	for (const file of files) {
		if (!file.endsWith(".html")) {
			continue;
		}
		const htmlPath = path.join(workDirectoryPath, file);
		const htmlData = fs.readFileSync(htmlPath);
		const htmlSource = htmlData.toString();

		const htmlTree = nhp.parse(htmlSource);

		const scriptElementList = htmlTree.querySelectorAll("script");
		for (const scriptElement of scriptElementList) {
			if (
				scriptElement.hasAttribute("type") &&
				scriptElement.getAttribute("type") === "module"
			) {
				scriptElement.removeAttribute("type");
			}
		}

		fs.writeFileSync(htmlPath, htmlTree.toString());
	}
}
