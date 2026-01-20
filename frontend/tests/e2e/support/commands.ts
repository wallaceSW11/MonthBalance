declare global {
  namespace Cypress {
    interface Chainable {
      clickButton(text: string): Chainable<void>;
    }
  }
}

Cypress.Commands.add("clickButton", (text: string) => {
  cy.contains("button", text).click();
});

export {};
