import { useAtom } from "jotai";
import type { FC } from "react";
import React from "react";
import { SideMenu } from "./components/layout/SideMenu";
import type { PageKey } from "./pages";
import { SideMenuStoreAtom } from "./store/SideMenuStore";

export const App: FC = () => {
	const [sideMenuStoreAtom, setSideMenuStoreAtom] = useAtom(SideMenuStoreAtom);

	const handleSelectPageKey = (pageKey: PageKey) => {
		setSideMenuStoreAtom({
			...sideMenuStoreAtom,
			selectedPageKey: pageKey,
		});
	};

	return (
		<SideMenu
			selectedPageKey={sideMenuStoreAtom.selectedPageKey}
			handleSelectPageKey={handleSelectPageKey}
		/>
	);
};
