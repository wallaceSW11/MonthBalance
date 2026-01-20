import { useI18n } from "vue-i18n";
import { useLocaleStore } from "@/stores/locale";
import type { LocaleCode } from "@/locales";

/**
 * Composable helper para i18n com funcionalidades extras
 * Facilita o uso de traduções e troca de idiomas
 */
export function useI18nHelpers() {
  const { t, locale } = useI18n();
  const localeStore = useLocaleStore();

  /**
   * Traduz uma chave com suporte a parâmetros
   * @param key - Chave da tradução (ex: 'demo.title')
   * @param params - Parâmetros opcionais para interpolação
   */
  const translate = (key: string, params?: Record<string, any>) => {
    return params ? t(key, params) : t(key);
  };

  /**
   * Muda o idioma atual
   * @param newLocale - Novo código de locale (pt-BR, en-US)
   */
  const changeLocale = (newLocale: LocaleCode) => {
    localeStore.setLocale(newLocale);
  };

  /**
   * Retorna o idioma atual
   */
  const currentLocale = () => {
    return locale.value as LocaleCode;
  };

  return {
    t: translate,
    changeLocale,
    currentLocale,
    locale,
  };
}
