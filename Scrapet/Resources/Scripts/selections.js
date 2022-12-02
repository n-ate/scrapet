function Selection(ns, self) {

    self.AttributeValueSelector = {
        CONTAINS: "*=",
        ENDS_WITH: "$=",
        EQUALS: "=",
        HYPHEN_EQUALS: "|=",
        SPACE_CONTAINS: "~=",
        STARTS_WITH: "^=",
        contains(value) {
            return value == CONTAINS ||
                value == ENDS_WITH ||
                value == EQUALS ||
                value == HYPHEN_EQUALS ||
                value == SPACE_CONTAINS ||
                value == STARTS_WITH;
        }
    };
    Object.freeze(self.AttributeValueSelector); //freeze as enum

    class Attribute {
        constructor(name, value) {
            this.Name = name;
            this.Value = value;
        }

        toCssSelector(valueSelector) {
            if (valueSelector == null) valueSelector = AttributeValueSelector.EQUALS;
            else if (!AttributeValueSelector.contains(valueSelector)) throw `Parameter valueSelector must be of enum type AttributeValueSelector.`;
            return `[${this.Name}=${this.Value}]`;
        }
    }
    self.Attribute = Attribute;

    class AttributeCollection {
        constructor(element) {
            this.Attributes = Array.from(element.attributes)
                .filter(a => { return a.specified && a.nodeName !== 'class'; }) //TODO: add odd framework generated attributes
                .map(a => new Attribute(a.nodeName, a.textContent));
        }
    }
    self.AttributeCollection = AttributeCollection;

}