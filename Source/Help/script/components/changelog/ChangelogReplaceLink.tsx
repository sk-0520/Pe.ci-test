import type { FC } from "react";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

type Kind = "text" | "issue" | "url";

interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(^#(?<ISSUE>\d+))/;
const UrlRegex = /(?<URL>^(https?:\/\/[\w?=&./\-;#~%]+(?![\w?&./;#~%"=-]*>)))/;

function splitTokens(s: string): Token[] {
	const result: Token[] = [];

	let currentIndex = 0;
	while (currentIndex < s.length) {
		const work = s.substring(currentIndex);

		const issueMatch = work.match(IssueRegex);
		if (issueMatch?.groups && "ISSUE" in issueMatch.groups) {
			result.push({
				kind: "issue",
				value: issueMatch.groups.ISSUE,
			});
			currentIndex += issueMatch.groups.ISSUE.length + 1 + 1;
			continue;
		}

		const urlMatch = work.match(UrlRegex);
		if (urlMatch?.groups && "URL" in urlMatch.groups) {
			result.push({
				kind: "url",
				value: urlMatch.groups.URL,
			});
			currentIndex += urlMatch.groups.URL.length + 1;
			continue;
		}

		result.push({
			kind: "text",
			value: work.substring(0, 1),
		});

		currentIndex += 1;
	}

	console.debug(result);

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
