import type { PageElement, PageKey } from "../pages";

export interface PageProps {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
	currentPage: PageElement;
}
