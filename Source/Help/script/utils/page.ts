import { type PageElement, type PageKey, PageKeys } from "../pages";

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

export function getPage(
	pageKey: PageKey,
	pages: readonly PageElement[],
): PageElement {
	const page = getPageCore(pageKey, pages);
	if (!page) {
		throw new Error(pageKey);
	}
	return page;
}

export function getPageKey(quey: URLSearchParams): PageKey {
	const pageValue = quey.get("page");
	if (pageValue) {
		const rawPageKey = pageValue as PageKey;
		if (PageKeys.includes(rawPageKey)) {
			return rawPageKey;
		}
	}

	throw new Error("pathName");
}
