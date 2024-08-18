import {
	AppBar,
	Box,
	Divider,
	Drawer,
	ThemeProvider,
	Toolbar,
	Typography,
	createTheme,
} from "@mui/material";
import { useAtom } from "jotai";
import type { FC } from "react";
import { PageContent } from "./components/layouts/PageContent";
import { SideMenu } from "./components/layouts/SideMenu";
import { type PageKey, Pages } from "./pages";
import { SideMenuStoreAtom } from "./stores/SideMenuStore";
import { getPage } from "./utils/page";

const sidebarWidth = 240;

export const App: FC = () => {
	const [sideMenuStoreAtom, setSideMenuStoreAtom] = useAtom(SideMenuStoreAtom);

	const handleSelectPageKey = (pageKey: PageKey) => {
		setSideMenuStoreAtom({
			...sideMenuStoreAtom,
			selectedPageKey: pageKey,
		});
	};

	const currentPage = getPage(sideMenuStoreAtom.selectedPageKey, Pages)

	return (
			<Box sx={{ display: "flex" }}>
				<AppBar
					position="fixed"
					sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
				>
					<Toolbar>
						<Typography variant="h6" noWrap component="div">
							{ currentPage.title }
						</Typography>
					</Toolbar>
				</AppBar>
				<Drawer
					sx={{
						width: sidebarWidth,
						flexShrink: 0,
						"& .MuiDrawer-paper": {
							width: sidebarWidth,
							boxSizing: "border-box",
						},
					}}
					variant="permanent"
					anchor="left"
				>
					<Toolbar />
					<Divider />
					<SideMenu
						selectedPageKey={sideMenuStoreAtom.selectedPageKey}
						handleSelectPageKey={handleSelectPageKey}
					/>
				</Drawer>

				<Box
					component="main"
					sx={{ flexGrow: 1, bgcolor: "background.default", p: 1 }}
				>
					<Toolbar />
					<PageContent
						selectedPageKey={sideMenuStoreAtom.selectedPageKey}
						handleSelectPageKey={handleSelectPageKey}
						currentPage={currentPage}
					/>
				</Box>
			</Box>
	);
};
