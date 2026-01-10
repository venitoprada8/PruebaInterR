/**
 * Sistema de internacionalización (i18n) - Tipos
 */

// Tipo recursivo para estructura de traducciones
export type TranslationValue = string | TranslationObject;

export interface TranslationObject {
  [key: string]: TranslationValue;
}

// Estructura de traducciones genérica
export interface Translations {
  [key: string]: TranslationValue;
}

// Opciones de interpolación
export interface InterpolationOptions {
  [key: string]: string | number | boolean;
}

// Opciones de pluralización
export interface PluralOptions {
  count: number;
  zero?: string;
  one?: string;
  other: string;
}

// Configuración de un idioma
export interface LanguageConfig {
  code: string;
  name: string;
  locale: string; // Para Intl API (ej: 'es-ES', 'en-US')
  direction?: 'ltr' | 'rtl'; // Dirección del texto
  fallback?: string; // Idioma de respaldo
}

// Metadata de traducción
export interface TranslationMetadata {
  version: string;
  lastUpdated: Date;
  author?: string;
  completeness?: number; // Porcentaje de traducción completado
}

// Paquete de traducción completo
export interface TranslationPackage {
  language: LanguageConfig;
  metadata: TranslationMetadata;
  translations: Translations;
}

// Opciones del servicio de traducción
export interface I18nServiceOptions {
  defaultLanguage?: string;
  fallbackLanguage?: string;
  availableLanguages?: string[];
  storageKey?: string;
  autoDetectLanguage?: boolean;
  validateTranslations?: boolean;
}

// Resultado de validación
export interface ValidationResult {
  isValid: boolean;
  missingKeys: string[];
  extraKeys: string[];
}

// Provider de traducciones (para lazy loading)
export type TranslationProvider = (language: string) => Promise<Translations> | Translations;

// Hook para cambios de idioma
export type LanguageChangeCallback = (newLanguage: string, oldLanguage: string) => void;
