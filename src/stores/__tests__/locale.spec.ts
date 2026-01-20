import { describe, it, expect, beforeEach } from "vitest";
import { setActivePinia, createPinia } from "pinia";
import { useLocaleStore } from "@/stores/locale";

describe("Locale Store", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
    localStorage.clear();
  });

  it("initializes with default locale", () => {
    const store = useLocaleStore();
    expect(store.currentLocale).toBeDefined();
    expect(["pt-BR", "en-US"]).toContain(store.currentLocale);
  });

  it("sets locale correctly", () => {
    const store = useLocaleStore();
    store.setLocale("en-US");
    expect(store.currentLocale).toBe("en-US");
  });

  it("persists locale to localStorage", () => {
    const store = useLocaleStore();
    store.setLocale("pt-BR");
    expect(localStorage.getItem("app-locale")).toBe("pt-BR");
  });

  it("loads saved locale from localStorage", () => {
    localStorage.setItem("app-locale", "en-US");
    const store = useLocaleStore();
    store.initializeLocale();
    expect(store.currentLocale).toBe("en-US");
  });

  it("falls back to browser language detection", () => {
    const store = useLocaleStore();
    store.initializeLocale();
    expect(["pt-BR", "en-US"]).toContain(store.currentLocale);
  });
});
