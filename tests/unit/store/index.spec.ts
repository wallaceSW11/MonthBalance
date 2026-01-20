import { describe, it, expect, beforeEach } from "vitest";
import { setActivePinia, createPinia } from "pinia";
import { useAppStore } from "@/store";

describe("App Store", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  it("initializes with correct default values", () => {
    const store = useAppStore();
    expect(store.counter).toBe(0);
    expect(store.user).toBeNull();
  });

  it("increments counter", () => {
    const store = useAppStore();
    store.increment();
    expect(store.counter).toBe(1);
  });

  it("computes double counter correctly", () => {
    const store = useAppStore();
    store.counter = 5;
    expect(store.doubleCounter).toBe(10);
  });

  it("sets user correctly", () => {
    const store = useAppStore();
    const userData = { name: "John Doe", email: "john@example.com" };
    store.setUser(userData);
    expect(store.user).toEqual(userData);
  });

  it("clears user correctly", () => {
    const store = useAppStore();
    store.setUser({ name: "John Doe", email: "john@example.com" });
    store.clearUser();
    expect(store.user).toBeNull();
  });
});
