import AnnouncementOutlinedIcon from "@mui/icons-material/AnnouncementOutlined";
import LightbulbOutlinedIcon from "@mui/icons-material/LightbulbOutlined";
import { AlertTitle, Typography, styled } from "@mui/material";
import Alert from "@mui/material/Alert";
import { type MarkdownToJSX, RuleType } from "markdown-to-jsx";
import React from "react";
import type { MarkdownRule } from "./markdown";

const AlertKinds = ["NOTE", "TIP", "IMPORTANT", "WARNING", "CAUTION"] as const;
type AlertKind = (typeof AlertKinds)[number];

const AlertDisplays: { [key in AlertKind]: string } = {
	NOTE: "ノート",
	TIP: "TIPS",
	IMPORTANT: "重要",
	WARNING: "警告",
	CAUTION: "注意",
};

const StyledAlert = styled(Alert)({
	marginBlock: "1em",
});

function getFirstChildren(
	node: MarkdownToJSX.BlockQuoteNode,
):
	| { firstChildren: MarkdownToJSX.ParserResult[]; firstIndex: number }
	| undefined {
	const paragraphIndex = node.children.findIndex(
		(a) => a.type === RuleType.paragraph,
	);
	if (
		paragraphIndex === -1 ||
		node.children[paragraphIndex].type !== RuleType.paragraph
	) {
		return undefined;
	}

	const firstChildren = node.children[paragraphIndex].children;

	return {
		firstChildren,
		firstIndex: paragraphIndex,
	};
}

// https://docs.github.com/ja/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#alerts
export const renderAlert: MarkdownRule = (node, renderChildren, state) => {
	if (node.type !== RuleType.blockQuote) {
		return undefined;
	}

	const firstItems = getFirstChildren(node);
	if (firstItems === undefined) {
		return undefined;
	}

	const { firstChildren, firstIndex } = firstItems;
	if (firstChildren.length < 3) {
		return undefined;
	}

	const valid = {
		head:
			firstChildren[0].type === RuleType.text && firstChildren[0].text === "[",
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
		firstChildren[1].type === RuleType.text &&
		firstChildren[2].type === RuleType.text
	) {
		const rawKind = firstChildren[1].text.substring(1) as AlertKind;

		// [!xxxx] の末尾 ] を破棄
		const firstElements = firstChildren.splice(2);
		if (
			1 <= firstElements.length &&
			firstElements[0].type === RuleType.text &&
			firstElements[0].text.startsWith("]")
		) {
			firstElements[0].text = firstElements[0].text.substring(1).trimStart();
		}

		const children = (
			<>
				<AlertTitle sx={{ fontWeight: "bold" }}>
					{AlertDisplays[rawKind]}
				</AlertTitle>
				<Typography>{renderChildren(firstElements, state)}</Typography>
				{renderChildren(node.children.splice(firstIndex + 1), state)}
			</>
		);

		switch (rawKind) {
			case "NOTE":
				return <Alert severity="info">{children}</Alert>;
			case "TIP":
				return (
					<StyledAlert
						severity="info"
						icon={
							<LightbulbOutlinedIcon htmlColor="green" fontSize="inherit" />
						}
						sx={{
							background: "#CBFFD3",
						}}
					>
						{children}
					</StyledAlert>
				);
			case "IMPORTANT":
				return (
					<StyledAlert
						severity="info"
						icon={
							<AnnouncementOutlinedIcon htmlColor="purple" fontSize="inherit" />
						}
						sx={{
							background: "#DCC2FF",
						}}
					>
						{children}
					</StyledAlert>
				);
			case "WARNING":
				return <StyledAlert severity="warning">{children}</StyledAlert>;
			case "CAUTION":
				return <StyledAlert severity="error">{children}</StyledAlert>;
		}
	}

	return undefined;
};
