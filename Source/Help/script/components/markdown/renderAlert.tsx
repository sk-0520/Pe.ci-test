import { AlertTitle, Typography } from "@mui/material";
import Alert from "@mui/material/Alert";
import { RuleType } from "markdown-to-jsx";
import type { ReactNode } from "react";
import React from "react";
import type { MarkdownRule } from "./markdown";

const AlertKinds = ["NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION"] as const;
type AlertKind = (typeof AlertKinds)[number];

export const renderAlert: MarkdownRule = (
	next,
	node,
	renderChildren,
	state,
) => {
	// https://docs.github.com/ja/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts
	if (node.type !== RuleType.blockQuote) {
		return undefined;
	}

	console.debug(node);

	if (
		2 <= node.children.length &&
		node.children[0].type === RuleType.newlineCoalescer &&
		node.children[1].type === RuleType.paragraph
	) {
		console.debug("node");
		const firstChildren = node.children[1].children;
		if (3 <= firstChildren.length) {
			const valid = {
				head:
					firstChildren[0].type === RuleType.text &&
					firstChildren[0].text === "[",
				kind:
					firstChildren[1].type === RuleType.text &&
					firstChildren[1].text.startsWith("!") &&
					AlertKinds.includes(firstChildren[1].text.substring(1) as AlertKind),
				tail:
					firstChildren[2].type === RuleType.text &&
					firstChildren[2].text.startsWith("]"),
			};
			if (
				valid.head &&
				valid.kind &&
				valid.tail &&
				firstChildren[1].type === RuleType.text
			) {
				console.debug(node);
				const rawKind = firstChildren[1].text.substring(1) as AlertKind;

				let children: ReactNode;
				if (3 <= node.children.length) {
					if (firstChildren[2].type !== RuleType.text) {
						throw new Error(JSON.stringify(firstChildren[2]));
					}

					children = (
						<>
							<AlertTitle sx={{ fontWeight: "bold" }}>
								{firstChildren[2].text.substring(1).trim()}
							</AlertTitle>
							{node.children.splice(2).map((a, i) => renderChildren(a, state))}
						</>
					);
				} else {
					if (firstChildren[2].type !== RuleType.text) {
						throw new Error(JSON.stringify(firstChildren[2]));
					}
					children = (
						<Typography>{firstChildren[2].text.substring(1).trim()}</Typography>
					);
				}

				switch (rawKind) {
					case "NOTE":
						return <Alert severity="info">{children}</Alert>;
					case "TIP":
						return <Alert severity="info">{children}</Alert>;
					case "IMPORTANT":
						return <Alert severity="info">{children}</Alert>;
					case "WARNING":
						return <Alert severity="warning">{children}</Alert>;
					case "CAUTION":
						return <Alert severity="error">{children}</Alert>;
				}
			}
		}
	}

	return undefined;
};
