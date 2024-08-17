import type { FC } from "react";
import React from "react";
import { Pages } from "../../pages";
import { MenuItem } from "./MenuItem";

export const Menu: FC = () => {
	return (
		<ul>
			{Pages.map((a) => (
				<MenuItem key={a.key} page={a} />
			))}
		</ul>
	);
};
