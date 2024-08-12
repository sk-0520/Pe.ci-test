import type { SearchMeta } from "../../../Build/scripts/search-types";
import helpMeta from "../../../Define/help-meta.json";
import Dom from "./core/Dom";

type SearchPosition = {
	index: number;
	length: number;
};

type SearchLine = {
	content: string;
	positions: SearchPosition[];
};

type SearchResult = {
	fileName: string;
	lines: Array<SearchLine>;
};

//TODO: 検索結果文字列長はここで結果に含めないといけない
function indexOf(
	haystack: string,
	needle: string,
	startIndex: number,
	regex: boolean,
	caseSensitive: boolean,
): number {
	if (regex) {
		const regex = new RegExp(needle, caseSensitive ? "i" : "");
		return haystack.search(regex);
	}

	if (caseSensitive) {
		return haystack.indexOf(needle, startIndex);
	}

	return haystack.toUpperCase().indexOf(needle.toUpperCase(), startIndex);
}

function search(
	input: string,
	caseSensitive: boolean,
	regex: boolean,
	develop: boolean,
): SearchResult[] {
	if (!input) {
		throw new Error("input");
	}

	const meta = helpMeta as SearchMeta;
	const result: Array<SearchResult> = [];

	for (const [baseName, value] of Object.entries(meta)) {
		const work: SearchResult = {
			fileName: `${baseName}.html`,
			lines: [],
		};

		if (baseName === "help.search") {
			continue;
		}

		if (value.kind === "develop") {
			if (!develop) {
				continue;
			}
		}

		let startIndex = 0;
		for (const content of value.contents) {
			const searchLine: SearchLine = {
				content: content,
				positions: [],
			};

			const index = indexOf(content, input, startIndex, regex, caseSensitive);
			if (index === -1) {
				continue;
			}

			searchLine.positions.push({
				index: index,
				length: input.length,
			});

			startIndex += input.length;

			work.lines.push(searchLine);
		}
		if (work.lines.length) {
			result.push(work);
		}
	}

	return result;
}

function applyResult(
	searchResultElement: HTMLElement,
	result: ReadonlyArray<SearchResult>,
): void {
	const templateResultElement = Dom.requireElementById(
		"template-result",
		HTMLTemplateElement,
	);
	const templateResultFileElement = Dom.requireElementById(
		"template-result-file",
		HTMLTemplateElement,
	);

	const resultElement = Dom.cloneTemplate(templateResultElement);
	const resultTargetElement = Dom.requireSelector(resultElement, "[name=list]");

	for (const item of result) {
		const resultFileElement = Dom.cloneTemplate(templateResultFileElement);
		const fileElement = Dom.requireSelector(resultFileElement, "[name=file]");
		const linkElement = document.createElement("a");
		linkElement.href = `${item.fileName}`;
		linkElement.textContent = item.fileName;
		fileElement.appendChild(linkElement);

		const contentElement = Dom.requireSelector(
			resultFileElement,
			"[name=content]",
		);
		for (const line of item.lines) {
			const itemElement = document.createElement("li");
			itemElement.textContent = line.content;
			contentElement.appendChild(itemElement);
		}

		resultTargetElement.appendChild(resultFileElement);
		resultTargetElement.appendChild(contentElement);
	}

	searchResultElement.textContent = "";
	searchResultElement.appendChild(resultElement);
}

const searchFormElement = Dom.requireElementById(
	"search-input",
	HTMLFormElement,
);
searchFormElement.addEventListener("submit", (ev) => {
	ev.preventDefault();

	const searchInputQueryElement = Dom.requireElementById(
		"search-input-query",
		HTMLInputElement,
	);
	const searchInputCaseSensitiveElement = Dom.requireElementById(
		"search-input-case-sensitive",
		HTMLInputElement,
	);
	const searchInputRegexElement = Dom.requireElementById(
		"search-input-regex",
		HTMLInputElement,
	);
	const searchInputDevelopElement = Dom.requireElementById(
		"search-input-develop",
		HTMLInputElement,
	);

	const searchResultElement = Dom.requireElementById("search-result");

	try {
		const result = search(
			searchInputQueryElement.value,
			searchInputCaseSensitiveElement.checked,
			searchInputRegexElement.checked,
			searchInputDevelopElement.checked,
		);
		console.debug(result);
		if (result.length) {
			applyResult(searchResultElement, result);
		} else {
			throw new Error("検索結果なし");
		}
	} catch (ex: unknown) {
		const templateErrorsElement = Dom.requireElementById(
			"template-errors",
			HTMLTemplateElement,
		);
		const errorsElement = Dom.cloneTemplate(templateErrorsElement);
		const errorElement = Dom.requireSelector(errorsElement, "[name=error]");
		if (ex instanceof Error) {
			errorElement.textContent = ex.message;
		} else {
			errorElement.textContent = `なんかエラー: ${JSON.stringify(ex)}`;
		}
		searchResultElement.textContent = "";
		searchResultElement.appendChild(errorsElement);
	}
});
