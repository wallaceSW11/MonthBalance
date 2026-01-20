import { vi } from "vitest";
import { config } from "@vue/test-utils";
import { createVuetify } from "vuetify";

// Create minimal Vuetify instance for tests
const vuetify = createVuetify();

config.global.plugins = [vuetify];

config.global.mocks = {
  $route: {
    path: "/",
    name: "Home",
  },
  $router: {
    push: vi.fn(),
  },
};
