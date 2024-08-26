import type { FC } from "react";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

type Kind = "text" | "issue" | "url";

interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(#(?<ISSUE>\d+))/;
const UrlRegex = /(?<URL>(https?:\/\/[\w?=&./\-;#~%]+(?![\w?&./;#~%"=-]*>)))/;

function splitTokens(s: string): Token[] {
	const result: Token[] = [];

	// if(s.startsWith(".NET Framework 4.6 は")) {
	// 	debugger
	// }

	let prevIndex = 0;
	let currentIndex = 0;
	while (currentIndex < s.length) {
		const work = s.substring(currentIndex);

		const issueMatch = work.match(IssueRegex);
		if (issueMatch?.groups && "ISSUE" in issueMatch.groups) {
			if(issueMatch.index && prevIndex < issueMatch.index) {
				result.push({
					kind: "text",
					value: work.substring(0, issueMatch.index),
				});
			}

			result.push({
				kind: "issue",
				value: issueMatch.groups.ISSUE,
			});
			prevIndex = currentIndex += issueMatch.groups.ISSUE.length + 1 + 1;
			continue;
		}

		const urlMatch = work.match(UrlRegex);
		if (urlMatch?.groups && "URL" in urlMatch.groups) {
			if(urlMatch.index && prevIndex < urlMatch.index) {
				result.push({
					kind: "text",
					value: work.substring(0, urlMatch.index),
				});
			}

			result.push({
				kind: "url",
				value: urlMatch.groups.URL,
			});
			prevIndex = currentIndex += urlMatch.groups.URL.length + 1;
			continue;
		}

		currentIndex += 1;
	}

	if(prevIndex < currentIndex) {
		result.push({
			kind: "text",
			value: s.substring(prevIndex, currentIndex),
		});
	}

	console.debug(result)

	return result;
}

interface ChangelogReplaceLinkProps {
	children: string;
}

export const ChangelogReplaceLink: FC<ChangelogReplaceLinkProps> = (
	props: ChangelogReplaceLinkProps,
) => {
	const { children } = props;

	const tokens = splitTokens(children);

	if (!tokens) {
		return children;
	}

	return tokens.map((a, i) => {
		switch (a.kind) {
			case "text":
				return `<${a.value}>`;
			case "issue":
				return `{#${a.value}}`;
			case "url":
				return `[${a.value}]`;
		}
	});
};
