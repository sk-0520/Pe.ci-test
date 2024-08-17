import React, { type FC, type MouseEvent } from "react";
import { type PageElement, type PageKey, Pages } from "../../pages";

type PageContentProps = {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
};

function getPageCore(
	pageKey: PageKey,
	pages: readonly PageElement[],
): PageElement | undefined {
	for (const page of pages) {
		if (page.key === pageKey) {
			return page;
		}
		if (page.nodes) {
			const childPage = getPageCore(pageKey, page.nodes);
			if (childPage) {
				return childPage;
			}
		}
	}

	return undefined;
}

function getPage(pageKey: PageKey, pages: readonly PageElement[]): PageElement {
	const page = getPageCore(pageKey, pages);
	if (!page) {
		throw new Error(pageKey);
	}
	return page;
}

export const PageContent: FC<PageContentProps> = (props: PageContentProps) => {
	const { selectedPageKey } = props;
	const page = getPage(selectedPageKey, Pages);

	return (
		<>
			{page.title}
		</>
	);
};
