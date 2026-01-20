import ptBR from "@/locales/pt-BR";
import enUS from "@/locales/en-US";

export const messages = {
  "pt-BR": ptBR,
  "en-US": enUS,
};

export const availableLocales = [
  { code: "pt-BR", name: "Português (Brasil)", countryCode: "BR" },
  { code: "en-US", name: "English (US)", countryCode: "US" },
] as const;

export type LocaleCode = keyof typeof messages;

function getDefaultLocale(): LocaleCode {
  const browserLang = navigator.language.toLowerCase()
  
  return browserLang.startsWith('pt') ? 'pt-BR' : 'en-US'
}

export const defaultLocale: LocaleCode = getDefaultLocale();
