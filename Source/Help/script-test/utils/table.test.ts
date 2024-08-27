import { splitEntities } from "../../script/utils/table";

describe("splitEntities", () => {
	test("empty", () => {
		const actual = splitEntities("");
		expect(actual).toHaveLength(0);
	});

	test("1 line", () => {
		const actual = splitEntities("A\n___\nB");
		expect(actual).toHaveLength(2);
		expect(actual[0]).toBe("A");
		expect(actual[1]).toBe("B");
	});

	test("2 line", () => {
		const actual = splitEntities("A\n___\nB\n___\nC");
		expect(actual).toHaveLength(3);
		expect(actual[0]).toBe("A");
		expect(actual[1]).toBe("B");
		expect(actual[2]).toBe("C");
	});
});
