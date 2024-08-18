import type { FC, MouseEvent } from "react";
import { type PageElement, type PageKey, Pages } from "../../pages";
import { getPage } from "../../utils/page";

type PageContentProps = {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
	currentPage: PageElement
};

export const PageContent: FC<PageContentProps> = (props: PageContentProps) => {
	const { selectedPageKey, currentPage } = props;

	return <>{currentPage.title}</>;
};
