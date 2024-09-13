export type Kind = "text" | "issue" | "url";

export interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(^#(?<ISSUE>\d+))/;
const UrlRegex = /^(?<URL>(https?:\/\/[\w?=&./\-;#~%]+(?![\w?&./;#~%"=\-]*>)))/;

export function splitTokens(s: string): Token[] {
	const buffer: Token[] = [];

	let currentIndex = 0;
	while (currentIndex < s.length) {
		const work = s.substring(currentIndex);

		const issueMatch = work.match(IssueRegex);
		if (issueMatch?.groups && "ISSUE" in issueMatch.groups) {
			buffer.push({
				kind: "issue",
				value: issueMatch.groups.ISSUE,
			});
			currentIndex += issueMatch.groups.ISSUE.length + 1 /* #の分を追加 */;
			continue;
		}

		const urlMatch = work.match(UrlRegex);
		if (urlMatch?.groups && "URL" in urlMatch.groups) {
			buffer.push({
				kind: "url",
				value: urlMatch.groups.URL,
			});
			currentIndex += urlMatch.groups.URL.length;
			continue;
		}

		buffer.push({
			kind: "text",
			value: work.substring(0, 1),
		});

		currentIndex += 1;
	}

	const result: Token[] = [];
	for (let i = 0; i < buffer.length; i++) {
		if (i) {
			const work = result[result.length - 1];
			if (work.kind === "text" && buffer[i].kind === "text") {
				work.value += buffer[i].value;
			} else {
				result.push(buffer[i]);
			}
		} else {
			result.push(buffer[i]);
		}
	}

	return result;
}
