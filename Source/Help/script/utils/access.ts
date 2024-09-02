export class AccessError extends Error {
	constructor(message: string) {
		super(message);
		this.name = this.constructor.name;
	}
}

export class KeyTypeError extends AccessError {}
export class OutOfRangeError extends AccessError {}
export class ItemsTypeError extends AccessError {}

export function getElement<TValue>(
	array: ReadonlyArray<TValue>,
	index: number,
): Exclude<TValue, undefined>;
export function getElement<TKey extends PropertyKey, TValue>(
	map: Map<TKey, TValue> | ReadonlyMap<TKey, TValue>,
	key: TKey,
): Exclude<TValue, undefined>;
export function getElement<TKey extends PropertyKey, TValue>(
	record: Record<TKey, TValue>,
	key: TKey,
): Exclude<TValue, undefined>;
export function getElement<TKey extends PropertyKey | number, TValue>(
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
