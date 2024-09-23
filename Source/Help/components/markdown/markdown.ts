import type { MarkdownToJSX } from "markdown-to-jsx";
import type { ReactChild } from "react";

export type MarkdownRule = (
	//next: () => ReactChild,
	node: MarkdownToJSX.ParserResult,
	renderChildren: MarkdownToJSX.RuleOutput,
	state: MarkdownToJSX.State,
) => ReactChild | undefined;
