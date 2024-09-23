export class AccessError extends Error {
	constructor(message: string) {
		super(message);
		this.name = this.constructor.name;
	}
}

export class KeyTypeError extends AccessError {}
export class OutOfRangeError extends AccessError {}
export class ItemsTypeError extends AccessError {}

/**
 * 配列から要素を取得。
 *
 * @param array 配列。
 * @param index 添え字。
 * @throws {@link OutOfRangeError} 取得できないか値が `undefine`
 */
export function getValue<TValue>(
	array: ReadonlyArray<TValue>,
	index: number,
): Exclude<TValue, undefined>;
/**
 * マップから値を取得。
 *
 * @param map マップ。
 * @param key キー。
 * @throws {@link OutOfRangeError} 取得できないか値が `undefine`
 */
export function getValue<TKey extends PropertyKey, TValue>(
	map: Map<TKey, TValue> | ReadonlyMap<TKey, TValue>,
	key: TKey,
): Exclude<TValue, undefined>;
/**
 * レコードから値取得
 *
 * @param record レコード。
 * @param key キー。
 * @throws {@link OutOfRangeError} 取得できないか値が `undefine`
 */
export function getValue<TKey extends PropertyKey, TValue>(
	record: Record<TKey, TValue>,
	key: TKey,
): Exclude<TValue, undefined>;
export function getValue<TKey extends PropertyKey | number, TValue>(
	items:
		| ReadonlyArray<TValue>
		| (Map<TKey, TValue> | ReadonlyMap<TKey, TValue>)
		| Record<TKey, TValue>,
	key: TKey,
): Exclude<TValue, undefined> {
	// ReadonlyArray
	if (Array.isArray(items)) {
		if (typeof key !== "number") {
			throw new KeyTypeError(`index is ${typeof key}`);
		}
		const element = items[key];
		if (element === undefined) {
			throw new OutOfRangeError(`index = ${key}`);
		}
		return element;
	}

	// Map
	if (Object.prototype.toString.call(items) === "[object Map]") {
		const map = items as Map<TKey, TValue>;
		const element = map.get(key);
		if (element === undefined) {
			throw new OutOfRangeError(`key = ${String(key)}`);
		}
		return element as Exclude<TValue, undefined>;
	}

	// Record
	if (typeof items === "object") {
		const record = items as Record<TKey, TValue>;
		const element = record[key];
		if (element === undefined) {
			throw new OutOfRangeError(`key = ${String(key)}`);
		}
		return element as Exclude<TValue, undefined>;
	}

	throw new ItemsTypeError(Object.prototype.toString.call(items));
}
