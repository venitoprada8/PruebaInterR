/**
 * Helpers para internacionalización
 */

import { InterpolationOptions, PluralOptions } from './types';

/**
 * Interpola variables en un string
 * Ejemplo: interpolate('Hello {name}!', { name: 'John' }) => 'Hello John!'
 */
export function interpolate(text: string, params?: InterpolationOptions): string {
  if (!params) return text;

  return text.replace(/\{(\w+)\}/g, (match, key) => {
    const value = params[key];
    return value !== undefined ? String(value) : match;
  });
}

/**
 * Maneja pluralización según la cantidad
 * Ejemplo: pluralize({ count: 5, one: '1 item', other: '{count} items' })
 */
export function pluralize(options: PluralOptions, params?: InterpolationOptions): string {
  const { count, zero, one, other } = options;

  let selected: string;

  if (count === 0 && zero) {
    selected = zero;
  } else if (count === 1 && one) {
    selected = one;
  } else {
    selected = other;
  }

  // Interpolar {count} automáticamente
  const finalParams = { ...params, count };
  return interpolate(selected, finalParams);
}

/**
 * Obtiene un valor anidado usando dot notation
 * Ejemplo: getNestedValue({ a: { b: 'value' } }, 'a.b') => 'value'
 */
export function getNestedValue(obj: any, path: string): any {
  const keys = path.split('.');
  let result = obj;

  for (const key of keys) {
    if (result && typeof result === 'object' && key in result) {
      result = result[key];
    } else {
      return undefined;
    }
  }

  return result;
}

/**
 * Formatea un número según el locale
 */
export function formatNumber(
  value: number,
  locale: string,
  options?: Intl.NumberFormatOptions
): string {
  return new Intl.NumberFormat(locale, options).format(value);
}

/**
 * Formatea una fecha según el locale
 */
export function formatDate(
  date: Date | string | number,
  locale: string,
  options?: Intl.DateTimeFormatOptions
): string {
  const dateObj = typeof date === 'string' || typeof date === 'number' ? new Date(date) : date;
  return new Intl.DateTimeFormat(locale, options).format(dateObj);
}

/**
 * Formatea moneda según el locale
 */
export function formatCurrency(
  value: number,
  locale: string,
  currency: string = 'USD'
): string {
  return new Intl.NumberFormat(locale, {
    style: 'currency',
    currency,
  }).format(value);
}

/**
 * Formatea tiempo relativo (hace 5 minutos, en 2 días, etc.)
 */
export function formatRelativeTime(
  date: Date | string | number,
  locale: string = 'en'
): string {
  const dateObj = typeof date === 'string' || typeof date === 'number' ? new Date(date) : date;
  const now = new Date();
  const diffMs = dateObj.getTime() - now.getTime();
  const diffSeconds = Math.floor(diffMs / 1000);
  const diffMinutes = Math.floor(diffSeconds / 60);
  const diffHours = Math.floor(diffMinutes / 60);
  const diffDays = Math.floor(diffHours / 24);

  try {
    const rtf = new Intl.RelativeTimeFormat(locale, { numeric: 'auto' });

    if (Math.abs(diffDays) >= 1) {
      return rtf.format(diffDays, 'day');
    } else if (Math.abs(diffHours) >= 1) {
      return rtf.format(diffHours, 'hour');
    } else if (Math.abs(diffMinutes) >= 1) {
      return rtf.format(diffMinutes, 'minute');
    } else {
      return rtf.format(diffSeconds, 'second');
    }
  } catch {
    // Fallback si RelativeTimeFormat no está disponible
    return dateObj.toLocaleString(locale);
  }
}

/**
 * Valida que todas las claves existan en ambos objetos
 */
export function validateTranslations(
  reference: Record<string, any>,
  target: Record<string, any>,
  prefix: string = ''
): { missingKeys: string[]; extraKeys: string[] } {
  const missingKeys: string[] = [];
  const extraKeys: string[] = [];

  // Buscar claves faltantes
  for (const key in reference) {
    const fullPath = prefix ? `${prefix}.${key}` : key;

    if (!(key in target)) {
      missingKeys.push(fullPath);
    } else if (typeof reference[key] === 'object' && typeof target[key] === 'object') {
      const nested = validateTranslations(reference[key], target[key], fullPath);
      missingKeys.push(...nested.missingKeys);
      extraKeys.push(...nested.extraKeys);
    }
  }

  // Buscar claves extra
  for (const key in target) {
    const fullPath = prefix ? `${prefix}.${key}` : key;

    if (!(key in reference)) {
      extraKeys.push(fullPath);
    }
  }

  return { missingKeys, extraKeys };
}

/**
 * Aplana un objeto anidado a dot notation
 * Ejemplo: { a: { b: 'value' } } => { 'a.b': 'value' }
 */
export function flattenObject(obj: Record<string, any>, prefix: string = ''): Record<string, string> {
  const result: Record<string, string> = {};

  for (const key in obj) {
    const fullPath = prefix ? `${prefix}.${key}` : key;
    const value = obj[key];

    if (typeof value === 'object' && value !== null && !Array.isArray(value)) {
      Object.assign(result, flattenObject(value, fullPath));
    } else {
      result[fullPath] = String(value);
    }
  }

  return result;
}

/**
 * Detecta el idioma del navegador
 */
export function detectBrowserLanguage(): string {
  if (typeof navigator === 'undefined') return 'en';

  const lang = navigator.language || (navigator as any).userLanguage;

  // Extraer código de idioma (ej: 'es-ES' => 'es')
  return lang.split('-')[0].toLowerCase();
}
