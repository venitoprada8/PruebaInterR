/**
 * Servicio de Internacionalización (i18n) Abstraído
 *
 * Características:
 * - Carga dinámica de idiomas
 * - Interpolación de variables
 * - Pluralización
 * - Formateo según locale
 * - Lazy loading
 * - Validación de traducciones
 * - Fallback automático
 */

import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  Translations,
  LanguageConfig,
  InterpolationOptions,
  PluralOptions,
  I18nServiceOptions,
  TranslationProvider,
  LanguageChangeCallback,
} from './types';
import {
  interpolate,
  pluralize,
  getNestedValue,
  formatNumber,
  formatDate,
  formatCurrency,
  formatRelativeTime,
  validateTranslations,
  detectBrowserLanguage,
} from './helpers';

@Injectable({
  providedIn: 'root'
})
export class I18nService {
  private currentLanguage: string;
  private fallbackLanguage: string;
  private storageKey: string;
  private translations: Map<string, Translations> = new Map();
  private languageConfigs: Map<string, LanguageConfig> = new Map();
  private translationProviders: Map<string, TranslationProvider> = new Map();
  private languageChangeCallbacks: LanguageChangeCallback[] = [];

  private languageSubject: BehaviorSubject<string>;
  public language$: Observable<string>;

  constructor() {
    // Configuración por defecto
    const options: I18nServiceOptions = {
      defaultLanguage: 'es',
      fallbackLanguage: 'en',
      storageKey: 'app-i18n-language',
      autoDetectLanguage: true,
      validateTranslations: false,
    };

    this.storageKey = options.storageKey!;
    this.fallbackLanguage = options.fallbackLanguage!;

    // Detectar idioma inicial
    let initialLanguage = options.defaultLanguage!;

    if (options.autoDetectLanguage) {
      const savedLanguage = this.loadFromStorage();
      const browserLanguage = detectBrowserLanguage();
      initialLanguage = savedLanguage || browserLanguage || initialLanguage;
    }

    this.currentLanguage = initialLanguage;
    this.languageSubject = new BehaviorSubject<string>(this.currentLanguage);
    this.language$ = this.languageSubject.asObservable();
  }

  /**
   * Registra un idioma con sus traducciones
   */
  registerLanguage(
    code: string,
    translations: Translations | TranslationProvider,
    config?: Partial<LanguageConfig>
  ): void {
    // Guardar configuración del idioma
    const languageConfig: LanguageConfig = {
      code,
      name: config?.name || code.toUpperCase(),
      locale: config?.locale || code,
      direction: config?.direction || 'ltr',
      fallback: config?.fallback || this.fallbackLanguage,
    };

    this.languageConfigs.set(code, languageConfig);

    // Si es una función provider, guardarla para lazy loading
    if (typeof translations === 'function') {
      this.translationProviders.set(code, translations);
    } else {
      // Si son traducciones directas, guardarlas
      this.translations.set(code, translations);
    }
  }

  /**
   * Carga traducciones de un idioma (lazy loading)
   */
  private async loadTranslations(language: string): Promise<void> {
    if (this.translations.has(language)) {
      return; // Ya está cargado
    }

    const provider = this.translationProviders.get(language);
    if (!provider) {
      console.warn(`No translations or provider found for language: ${language}`);
      return;
    }

    try {
      const translations = await provider(language);
      this.translations.set(language, translations);
    } catch (error) {
      console.error(`Error loading translations for ${language}:`, error);
    }
  }

  /**
   * Cambia el idioma activo
   */
  async setLanguage(language: string): Promise<void> {
    if (!this.languageConfigs.has(language)) {
      console.error(`Language "${language}" is not registered`);
      return;
    }

    // Cargar traducciones si no están cargadas
    await this.loadTranslations(language);

    const oldLanguage = this.currentLanguage;
    this.currentLanguage = language;
    this.languageSubject.next(language);
    this.saveToStorage(language);

    // Notificar a los callbacks
    this.languageChangeCallbacks.forEach(callback => {
      callback(language, oldLanguage);
    });
  }

  /**
   * Obtiene el idioma actual
   */
  get language(): string {
    return this.currentLanguage;
  }

  /**
   * Obtiene la configuración del idioma actual
   */
  get languageConfig(): LanguageConfig | undefined {
    return this.languageConfigs.get(this.currentLanguage);
  }

  /**
   * Obtiene el locale del idioma actual (para Intl API)
   */
  get locale(): string {
    return this.languageConfig?.locale || this.currentLanguage;
  }

  /**
   * Obtiene una traducción por clave
   */
  translate(key: string, params?: InterpolationOptions): string {
    const translations = this.translations.get(this.currentLanguage);

    if (!translations) {
      return this.getFallbackTranslation(key, params);
    }

    const value = getNestedValue(translations, key);

    if (value === undefined) {
      return this.getFallbackTranslation(key, params);
    }

    if (typeof value !== 'string') {
      console.warn(`Translation key "${key}" is not a string`);
      return key;
    }

    return params ? interpolate(value, params) : value;
  }

  /**
   * Alias corto para translate
   */
  t(key: string, params?: InterpolationOptions): string {
    return this.translate(key, params);
  }

  /**
   * Obtiene traducción con fallback
   */
  private getFallbackTranslation(key: string, params?: InterpolationOptions): string {
    const fallbackTranslations = this.translations.get(this.fallbackLanguage);

    if (fallbackTranslations) {
      const value = getNestedValue(fallbackTranslations, key);
      if (typeof value === 'string') {
        return params ? interpolate(value, params) : value;
      }
    }

    console.warn(`Translation not found: ${key}`);
    return key; // Devolver la clave como último recurso
  }

  /**
   * Traduce con pluralización
   */
  translatePlural(options: PluralOptions, params?: InterpolationOptions): string {
    // Las opciones de plural pueden estar en el idioma actual
    // Por ahora usamos el helper directamente
    return pluralize(options, params);
  }

  /**
   * Alias corto para translatePlural
   */
  tp(options: PluralOptions, params?: InterpolationOptions): string {
    return this.translatePlural(options, params);
  }

  /**
   * Obtiene todas las traducciones del idioma actual
   */
  get translations(): Translations {
    return this.translations.get(this.currentLanguage) || {};
  }

  /**
   * Obtiene idiomas disponibles
   */
  get availableLanguages(): string[] {
    return Array.from(this.languageConfigs.keys());
  }

  /**
   * Verifica si un idioma está disponible
   */
  hasLanguage(language: string): boolean {
    return this.languageConfigs.has(language);
  }

  /**
   * Registra un callback para cambios de idioma
   */
  onLanguageChange(callback: LanguageChangeCallback): () => void {
    this.languageChangeCallbacks.push(callback);

    // Devolver función para desregistrar
    return () => {
      const index = this.languageChangeCallbacks.indexOf(callback);
      if (index > -1) {
        this.languageChangeCallbacks.splice(index, 1);
      }
    };
  }

  /**
   * Valida que las traducciones estén completas
   */
  validateLanguage(language: string, referenceLanguage: string = this.fallbackLanguage): {
    isValid: boolean;
    missingKeys: string[];
    extraKeys: string[];
  } {
    const reference = this.translations.get(referenceLanguage);
    const target = this.translations.get(language);

    if (!reference || !target) {
      return { isValid: false, missingKeys: [], extraKeys: [] };
    }

    const result = validateTranslations(reference as any, target as any);

    return {
      isValid: result.missingKeys.length === 0 && result.extraKeys.length === 0,
      ...result,
    };
  }

  // === Métodos de formateo ===

  /**
   * Formatea un número según el locale actual
   */
  formatNumber(value: number, options?: Intl.NumberFormatOptions): string {
    return formatNumber(value, this.locale, options);
  }

  /**
   * Formatea una fecha según el locale actual
   */
  formatDate(date: Date | string | number, options?: Intl.DateTimeFormatOptions): string {
    return formatDate(date, this.locale, options);
  }

  /**
   * Formatea moneda según el locale actual
   */
  formatCurrency(value: number, currency: string = 'USD'): string {
    return formatCurrency(value, this.locale, currency);
  }

  /**
   * Formatea tiempo relativo según el locale actual
   */
  formatRelativeTime(date: Date | string | number): string {
    return formatRelativeTime(date, this.locale);
  }

  // === Persistencia ===

  private saveToStorage(language: string): void {
    try {
      localStorage.setItem(this.storageKey, language);
    } catch (error) {
      console.warn('Could not save language to localStorage', error);
    }
  }

  private loadFromStorage(): string | null {
    try {
      return localStorage.getItem(this.storageKey);
    } catch (error) {
      console.warn('Could not load language from localStorage', error);
      return null;
    }
  }
}
