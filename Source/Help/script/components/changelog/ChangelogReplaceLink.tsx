import type { FC } from "react";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

type Kind = "text" | "issue" | "url";

interface Token {
	kind: Kind;
	value: string;
}

const IssueRegex = /(#(?<ISSUE>\d+))/;
const UrlRegex = /(?<URL>https?:\/\/.+)/;

function splitTokens(s: string): Token[] {
	const result: Token[] = [];

	let prev = 0;
	let i = 0;
	while (i < s.length) {
		const work = s.substring(i);

		const issue = work.match(IssueRegex);
		if (issue?.groups && "ISSUE" in issue.groups) {
			if (prev !== i) {
				result.push({
					kind: "text",
					value: s.substring(prev, i),
				});
			}
			result.push({
				kind: "issue",
				value: issue.groups.ISSUE,
			});
			i += issue.groups.ISSUE.length + 1;
			prev = i;
			continue;
		}
		const url = work.match(UrlRegex);
		if (url?.groups && "URL" in url.groups) {
			if (prev !== i) {
				result.push({
					kind: "text",
					value: s.substring(prev, i),
				});
			}
			result.push({
				kind: "url",
				value: url.groups.URL,
			});
			i += url.groups.URL.length;
			prev = i;
			continue;
		}

		i += 1;
	}
	if(prev !== i - 1) {
		result.push({
			kind: "text",
			value: s.substring(prev, i),
		});
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
				return `text:${a.value}`;
			case "issue":
				return `issue:${a.value}`;
			case "url":
				return `url:${a.value}`;
		}
	});
};
