import { ThemeProvider, createTheme } from "@mui/material";
import React ,{ StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "./App";
import { AppTheme } from "./theme/AppTheme";

const root = createRoot(document.getElementById("app") as HTMLElement);
const theme = createTheme(AppTheme);

root.render(
	<StrictMode>
		<ThemeProvider theme={theme}>
			<App />
		</ThemeProvider>
	</StrictMode>,
);
