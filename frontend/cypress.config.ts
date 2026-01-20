import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: "http://localhost:5173",
    supportFile: "tests/e2e/support/e2e.ts",
    specPattern: "tests/e2e/**/*.cy.ts",
    videosFolder: "cypress/videos",
    screenshotsFolder: "cypress/screenshots",
    fixturesFolder: "tests/e2e/fixtures",
    viewportWidth: 1280,
    viewportHeight: 720,
  },
});
