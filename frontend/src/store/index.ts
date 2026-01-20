import { defineStore } from "pinia";
import { ref, computed } from "vue";

export const useAppStore = defineStore("app", () => {
  const counter = ref(0);
  const user = ref<{ name: string; email: string } | null>(null);

  const doubleCounter = computed(() => counter.value * 2);

  const increment = () => {
    counter.value++;
  };

  const setUser = (userData: { name: string; email: string }) => {
    user.value = userData;
  };

  const clearUser = () => {
    user.value = null;
  };

  return {
    counter,
    user,
    doubleCounter,
    increment,
    setUser,
    clearUser,
  };
});
