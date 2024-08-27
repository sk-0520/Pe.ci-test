import { Link } from "@mui/material";
import type { FC } from "react";
import { splitTokens } from "../../utils/changelog";

const issueLink = "https://github.com/sk-0520/Pe/issues/";

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
				return `${a.value}`;
			case "issue":
				return (
					<Link href={issueLink + a.value} target={`issue_${a.value}`}>
						#{a.value}
					</Link>
				);
			case "url":
				return <Link href={a.value}>{a.value}</Link>;
		}
	});
};
