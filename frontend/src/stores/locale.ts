import { defineStore } from "pinia";
import { ref, watch } from "vue";
import { i18n } from "@/plugins/i18n";
import { defaultLocale, type LocaleCode } from "@/locales";

// Usar a mesma chave que a BaseLib usa
const LOCALE_STORAGE_KEY = "locale";

function detectBrowserLocale(): LocaleCode {
  const browserLang = navigator.language;
  return browserLang.startsWith("pt") ? "pt-BR" : "en-US";
}

function loadSavedLocaleOrDetect(): LocaleCode {
  const savedLocale = localStorage.getItem(
    LOCALE_STORAGE_KEY
  ) as LocaleCode | null;

  if (savedLocale && (savedLocale === "pt-BR" || savedLocale === "en-US")) {
    return savedLocale;
  }

  return detectBrowserLocale();
}

function syncLocaleWithI18n(locale: LocaleCode) {
  i18n.global.locale.value = locale;
}

function persistLocale(locale: LocaleCode) {
  localStorage.setItem(LOCALE_STORAGE_KEY, locale);
}

export const useLocaleStore = defineStore("locale", () => {
  const currentLocale = ref<LocaleCode>(defaultLocale);

  const initializeLocale = () => {
    const locale = loadSavedLocaleOrDetect();
    currentLocale.value = locale;
    syncLocaleWithI18n(locale);
  };

  const setLocale = (locale: LocaleCode) => {
    currentLocale.value = locale;
    syncLocaleWithI18n(locale);
    persistLocale(locale);
  };

  watch(currentLocale, (newLocale) => {
    syncLocaleWithI18n(newLocale);
    persistLocale(newLocale);
  });

  return {
    currentLocale,
    setLocale,
    initializeLocale,
  };
});
