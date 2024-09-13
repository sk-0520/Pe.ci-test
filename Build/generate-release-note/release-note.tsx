import { ThemeProvider, createTheme } from "@mui/material/styles";
import React, { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { AppTheme } from "../../Source/Help/script/theme/AppTheme";
import { ReleaseNote } from "./ReleaseNote";

const root = createRoot(document.getElementById("release-note") as HTMLElement);
const theme = createTheme(AppTheme);

root.render(
	<StrictMode>
		<ThemeProvider theme={theme}>
			<ReleaseNote />
		</ThemeProvider>
	</StrictMode>,
);
