import { atom } from "jotai";
import { type PageKey, PageKeys } from "../pages";

interface SideMenuStore {
	selectedPageKey: PageKey;
}

export const SideMenuStoreAtom = atom<SideMenuStore>({
	selectedPageKey: PageKeys[0],
});
