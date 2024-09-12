import type { PageElement, PageKey } from "../pages";

export interface PageProps {
	selectedPageKey: PageKey;
	callbackSelectPageKey: (pageKey: PageKey) => void;
	currentPage: PageElement;
}
