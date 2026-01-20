describe("Theme System", () => {
  beforeEach(() => {
    cy.visit("/");
    cy.clearLocalStorage();
  });

  it("should load with light theme by default", () => {
    cy.get(".v-app").should("exist");
    cy.get(".v-theme--light").should("exist");
  });

  it("should toggle between light and dark themes", () => {
    // Verify initial light theme
    cy.get(".v-theme--light").should("exist");

    // Click theme toggle button
    cy.get('[title="Switch to Dark Mode"]').click();

    // Verify dark theme is applied
    cy.get(".v-theme--dark").should("exist");

    // Toggle back to light
    cy.get('[title="Switch to Light Mode"]').click();

    // Verify light theme is restored
    cy.get(".v-theme--light").should("exist");
  });

  it("should persist theme preference in localStorage", () => {
    // Switch to dark theme
    cy.get('[title="Switch to Dark Mode"]').click();

    // Verify localStorage is updated
    cy.window().then((win) => {
      expect(win.localStorage.getItem("app-theme")).to.equal("dark");
    });

    // Reload page
    cy.reload();

    // Verify dark theme is still active
    cy.get(".v-theme--dark").should("exist");
  });

  it("should display theme configuration on demo page", () => {
    cy.visit("/demo");

    // Find theme configuration card
    cy.contains("Theme Configuration").should("be.visible");

    // Verify current theme is displayed
    cy.contains("Current Theme:").should("be.visible");
    cy.contains("light").should("be.visible");

    // Verify app name is displayed
    cy.contains("App Name:").should("be.visible");
    cy.contains("Vue3 TypeScript Base").should("be.visible");
  });

  it("should display theme colors as chips", () => {
    cy.visit("/demo");

    // Verify color chips are displayed
    cy.contains("Theme Colors:").should("be.visible");

    // Verify some color names are present
    cy.contains("primary").should("be.visible");
    cy.contains("secondary").should("be.visible");
    cy.contains("success").should("be.visible");
    cy.contains("error").should("be.visible");
  });

  it("should update colors when theme is toggled", () => {
    cy.visit("/demo");

    // Get initial primary color value
    cy.contains(".v-chip", "primary")
      .should("be.visible")
      .invoke("text")
      .then((lightPrimary) => {
        // Toggle to dark theme
        cy.get('[title="Switch to Dark Mode"]').click();

        // Wait for theme to change
        cy.get(".v-theme--dark").should("exist");

        // Verify primary color changed
        cy.contains(".v-chip", "primary")
          .invoke("text")
          .should("not.equal", lightPrimary);
      });
  });

  it("should load theme configuration from theme.json", () => {
    // Intercept the theme.json request
    cy.intercept("GET", "/theme.json").as("themeConfig");

    cy.visit("/");

    // Wait for theme to load
    cy.wait("@themeConfig").then((interception) => {
      expect(interception.response?.statusCode).to.equal(200);
      expect(interception.response?.body).to.have.property("colors");
      expect(interception.response?.body.colors).to.have.property("light");
      expect(interception.response?.body.colors).to.have.property("dark");
    });
  });

  it("should apply primary color to app bar", () => {
    cy.visit("/");

    // Verify app bar exists and has color attribute
    cy.get(".v-app-bar").should("exist").and("have.attr", "color", "primary");
  });

  it("should update app name in header from theme config", () => {
    cy.request("/theme.json").then((response) => {
      const appName = response.body.customization.appName;

      cy.visit("/");

      // Verify app name in header
      cy.get(".v-app-bar-title").should("contain", appName);
    });
  });

  it("should maintain theme state during navigation", () => {
    // Switch to dark theme on home page
    cy.visit("/");
    cy.get('[title="Switch to Dark Mode"]').click();
    cy.get(".v-theme--dark").should("exist");

    // Navigate to demo page
    cy.get('[href="/demo"]').click();
    cy.url().should("include", "/demo");

    // Verify dark theme is still active
    cy.get(".v-theme--dark").should("exist");

    // Navigate back to home
    cy.get('[href="/"]').click();
    cy.url().should("not.include", "/demo");

    // Verify dark theme is still active
    cy.get(".v-theme--dark").should("exist");
  });

  it("should show correct theme toggle icon", () => {
    cy.visit("/");

    // In light mode, should show moon icon
    cy.get(".v-btn .v-icon").should("contain", "mdi-weather-night");

    // Toggle to dark mode
    cy.get('[title="Switch to Dark Mode"]').click();

    // In dark mode, should show sun icon
    cy.get(".v-btn .v-icon").should("contain", "mdi-weather-sunny");
  });
});
