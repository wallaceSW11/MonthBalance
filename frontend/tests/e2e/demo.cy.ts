describe("Demo Page", () => {
  beforeEach(() => {
    cy.visit("/demo");
  });

  it("displays component demo heading", () => {
    cy.contains("h1", "Component Demo").should("be.visible");
  });

  it("displays all button types", () => {
    cy.contains("button", "Primary").should("be.visible");
    cy.contains("button", "Secondary").should("be.visible");
    cy.contains("button", "Tertiary").should("be.visible");
    cy.contains("button", "Quartenary").should("be.visible");
  });

  it("shows notification when button is clicked", () => {
    cy.contains("button", "Primary").click();
    cy.contains("Button Clicked").should("be.visible");
  });

  it("displays notification buttons", () => {
    cy.contains("Success Notification").should("be.visible");
    cy.contains("Error Notification").should("be.visible");
    cy.contains("Warning Notification").should("be.visible");
    cy.contains("Info Notification").should("be.visible");
  });

  it("shows success notification when success button is clicked", () => {
    cy.contains("Success Notification").click();
    cy.contains("Success!").should("be.visible");
  });

  it("displays loading button", () => {
    cy.contains("Show Loading").should("be.visible");
  });

  it("increments counter in Pinia store", () => {
    cy.contains("Counter:")
      .invoke("text")
      .then((text) => {
        const initialValue = parseInt(text.match(/\d+/)?.[0] || "0");

        cy.contains("Increment Counter").click();

        cy.contains("Counter:").should(
          "contain",
          (initialValue + 1).toString()
        );
      });
  });
});
