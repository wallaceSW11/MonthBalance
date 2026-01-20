export function formatCurrency(value: number, locale: string = 'pt-BR'): string {
  const options: Intl.NumberFormatOptions = {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }
  
  return new Intl.NumberFormat(locale, options).format(value)
}

export function parseCurrency(value: string, locale: string = 'pt-BR'): number {
  if (!value) return 0
  
  let cleanValue = value.replace(/[^\d,.-]/g, '')
  
  if (locale === 'pt-BR') {
    cleanValue = cleanValue.replace(/\./g, '').replace(',', '.')
  } else {
    cleanValue = cleanValue.replace(/,/g, '')
  }
  
  const parsed = parseFloat(cleanValue)
  
  return isNaN(parsed) ? 0 : parsed
}
