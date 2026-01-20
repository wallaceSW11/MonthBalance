describe("Home Page", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("displays the welcome message", () => {
    cy.contains("h1", "Welcome to Vue 3 + TypeScript Base").should(
      "be.visible"
    );
  });

  it("has a link to the demo page", () => {
    cy.contains("View Demo").should("be.visible");
  });

  it("navigates to demo page when clicking the demo link", () => {
    cy.contains("View Demo").click();
    cy.url().should("include", "/demo");
    cy.contains("h1", "Component Demo").should("be.visible");
  });

  it("displays feature list", () => {
    cy.contains("Vue 3 Composition API").should("be.visible");
    cy.contains("TypeScript").should("be.visible");
    cy.contains("Vuetify 3").should("be.visible");
  });
});
