import { ThemeProvider, createTheme } from "@mui/material";
import { useAtom } from "jotai";
import type { FC } from "react";
import React from "react";
import { PageContent } from "./components/layouts/PageContent";
import { SideMenu } from "./components/layouts/SideMenu";
import { type PageKey, Pages } from "./pages";
import { SideMenuStoreAtom } from "./stores/SideMenuStore";
import { AppTheme } from "./theme/AppTheme";

export const App: FC = () => {
	const theme = createTheme(AppTheme);
	const [sideMenuStoreAtom, setSideMenuStoreAtom] = useAtom(SideMenuStoreAtom);

	const handleSelectPageKey = (pageKey: PageKey) => {
		setSideMenuStoreAtom({
			...sideMenuStoreAtom,
			selectedPageKey: pageKey,
		});
	};

	return (
		<ThemeProvider theme={theme}>
			<SideMenu
				selectedPageKey={sideMenuStoreAtom.selectedPageKey}
				handleSelectPageKey={handleSelectPageKey}
			/>
			<PageContent
				selectedPageKey={sideMenuStoreAtom.selectedPageKey}
				handleSelectPageKey={handleSelectPageKey}
			/>
		</ThemeProvider>
	);
};
