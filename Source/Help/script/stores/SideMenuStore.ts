import { atom } from "jotai";
import { type PageKey, PageKeys } from "../pages";

export const SelectedPageKeyAtom = atom<PageKey>(PageKeys[0]);
