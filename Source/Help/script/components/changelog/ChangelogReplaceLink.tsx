import type { FC } from "react";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

type Kind = "text" | "issue" | "url";

interface Token {
	kind: Kind;
	value: string;
}

const TokenRegex =
	/(#(?<ISSUE>\d+))?|(?<URL>(http|https|ftp):\/\/[\w?=&.\/\-;#~%]+(?![\w\s?&.\/;#~%"=-]*>))?/g;

function splitTokens(s: string): Token[] {
	const regExpExecArrays = s.matchAll(TokenRegex);
	const result: Token[] = [];

	for (const regExpExecArray of regExpExecArrays) {

		if (regExpExecArray.groups) {
			if ("ISSUE" in regExpExecArray.groups) {
				result.push({
					kind: "issue",
					value: regExpExecArray.groups.ISSUE,
				});
			}
			if ("URL" in regExpExecArray.groups) {
				result.push({
					kind: "url",
					value: regExpExecArray.groups.URL,
				});
			}
		} else {
			result.push({
				kind: "text",
				value: regExpExecArray[0],
			});
		}
	}

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
				return a.value;
			case "issue":
				return `issue:${a.value}`;
			case "url":
				return `url:${a.value}`;
		}
	});
};
