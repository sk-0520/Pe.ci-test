import type { FC } from "react";
import { HelpMarkdown } from "../../components/HelpMarkdown";
import type { PageProps } from "../../types/PageProps";

export const HelpIndexPage: FC<PageProps> = (props: PageProps) => {
	return <HelpMarkdown>{markdown}</HelpMarkdown>;
};

const markdown = /* markdown */ `

# 概要

本ソフトウェア(以後 Pe)はランチャーが主機能のデスクトップユーティリティです。 Pe は WTFPL 2 でライセンスされています。

# 必要環境

* [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)の動作要件を満たしたハードウェアスペック(環境自体は同梱)
* Windows 10, 11
	* x86 版は 32/64 bit で動作可能
	* x64 版は 64 bit のみで動作可能
	* Arm は知らん

# 連絡先

* [プロジェクトサイト](https://github.com/sk-0520/Pe)
* [フォーラム](https://github.com/sk-0520/Pe/discussions)
* [Webサイト](https://peserver.site/)
* [おれんち](https://content-type-text.net/)

`;
