import {
	ItemsTypeError,
	KeyTypeError,
	OutOfRangeError,
	getValue,
} from "../../script/utils/access";

describe("getValue:Array", () => {
	const commonInput = [10, 20, 30];

	test("normal", () => {
		expect(getValue(commonInput, 1)).toBe(20);
	});

	test("key type", () => {
		expect(() => getValue(commonInput, "1" as unknown as number)).toThrow(
			new KeyTypeError("index is string"),
		);
	});

	test("out of range", () => {
		expect(() => getValue(commonInput, 4)).toThrow(
			new OutOfRangeError("index = 4"),
		);
	});

	test("undefined", () => {
		const input = [10, undefined, 30];
		expect(() => getValue(input, 1)).toThrow(new OutOfRangeError("index = 1"));
	});
});

describe("getValue:Map", () => {
	const commonInput1 = new Map([
		["a", "A"],
		["b", "B"],
		["c", "C"],
	]);
	const commonInput2 = commonInput1 as ReadonlyMap<string, string>;

	describe("normal", () => {
		test("editable", () => {
			expect(getValue(commonInput1, "a")).toBe("A");
		});
		test("readonly", () => {
			expect(getValue(commonInput2, "a")).toBe("A");
		});
	});

	describe("normal", () => {
		test("editable", () => {
			expect(() => getValue(commonInput1, "d")).toThrow(
				new OutOfRangeError("key = d"),
			);
		});
		test("readonly", () => {
			expect(() => getValue(commonInput2, "d")).toThrow(
				new OutOfRangeError("key = d"),
			);
		});
	});

	describe("undefined", () => {
		const input1 = new Map([
			["a", "A"],
			["b", undefined],
			["c", "C"],
		]);
		const input2 = input1 as ReadonlyMap<string, string | undefined>;

		test("editable", () => {
			expect(() => getValue(input1, "b")).toThrow(
				new OutOfRangeError("key = b"),
			);
		});
		test("editable", () => {
			expect(() => getValue(input2, "b")).toThrow(
				new OutOfRangeError("key = b"),
			);
		});
	});
});

describe("getValue:Record", () => {
	const commonInput: Record<PropertyKey, number | string | undefined> = {
		a: "A",
		b: 2,
		c: undefined,
	};

	test("normal", () => {
		expect(getValue(commonInput, "a")).toBe("A");
	});

	test("out of range", () => {
		expect(() => getValue(commonInput, "A")).toThrow(
			new OutOfRangeError("key = A"),
		);
	});

	test("undefined", () => {
		expect(() => getValue(commonInput, "c")).toThrow(
			new OutOfRangeError("key = c"),
		);
	});
});

describe("getValue:?", () => {
	test("unknown", () => {
		const input = 123;
		expect(() =>
			getValue(input as unknown as Record<PropertyKey, object>, "key"),
		).toThrow(new ItemsTypeError("[object Number]"));
	});
});
