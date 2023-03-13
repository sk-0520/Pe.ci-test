
/**
 * 要素の追加位置。
 */
export const enum AttachPosition {
	/** 最後。 */
	Last,
	/** 最初。 */
	First,
	/** 直前。 */
	Previous,
	/** 直後。 */
	Next,
}


/**
 * ノード生成処理。
 */
export interface NodeFactory {
	//#region property

	readonly element: Node;

	//#endregion
}

/**
 * テキストノード生成処理。
 */
export class TextFactory implements NodeFactory {
	constructor(public readonly element: Text) {
	}
}

/**
 * 要素生成処理。
 */
export class TagFactory<TElement extends Element> implements NodeFactory {
	constructor(public readonly element: TElement) {
	}

	public createTag<K extends keyof HTMLElementTagNameMap>(tagName: K, options?: ElementCreationOptions): TagFactory<HTMLElementTagNameMap[K]>;
	/** @deprecated */
	public createTag<K extends keyof HTMLElementDeprecatedTagNameMap>(tagName: K, options?: ElementCreationOptions): TagFactory<HTMLElementDeprecatedTagNameMap[K]>;
	public createTag<THTMLElement extends HTMLElement>(tagName: string, options?: ElementCreationOptions): TagFactory<THTMLElement>;
	public createTag(tagName: string, options?: ElementCreationOptions): TagFactory<HTMLElement> {
		const createdElement = document.createElement(tagName, options);
		this.element.appendChild(createdElement);

		const nodeFactory = new TagFactory(createdElement);
		return nodeFactory;
	}

	public createText(text: string): TextFactory {
		const createdNode = document.createTextNode(text);
		this.element.appendChild(createdNode);

		const nodeFactory = new TextFactory(createdNode);
		return nodeFactory;
	}
}
