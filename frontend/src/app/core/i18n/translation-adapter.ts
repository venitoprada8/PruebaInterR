/**
 * Adaptador para migrar del sistema antiguo al nuevo
 *
 * Este adaptador permite usar ambos sistemas simultáneamente
 * mientras se realiza la migración gradual.
 */

import { AppTexts, TEXTS_ES, TEXTS_EN, Language } from '../../constants/texts';
import { Translations } from './types';
import { I18nService } from './i18n.service';

/**
 * Convierte AppTexts al formato de Translations genérico
 */
export function adaptAppTextsToTranslations(appTexts: AppTexts): Translations {
  return appTexts as any;
}

/**
 * Inicializa el servicio i18n con las traducciones existentes
 */
export function initializeI18nWithLegacyTexts(i18nService: I18nService): void {
  // Registrar español
  i18nService.registerLanguage(
    'es',
    adaptAppTextsToTranslations(TEXTS_ES),
    {
      name: 'Español',
      locale: 'es-ES',
      direction: 'ltr',
    }
  );

  // Registrar inglés
  i18nService.registerLanguage(
    'en',
    adaptAppTextsToTranslations(TEXTS_EN),
    {
      name: 'English',
      locale: 'en-US',
      direction: 'ltr',
    }
  );
}

/**
 * Wrapper para mantener compatibilidad con TranslationService antiguo
 */
export class LegacyTranslationServiceAdapter {
  constructor(private i18nService: I18nService) {}

  get texts(): AppTexts {
    return this.i18nService.translations as AppTexts;
  }

  get language(): Language {
    return this.i18nService.language as Language;
  }

  get language$() {
    return this.i18nService.language$;
  }

  setLanguage(lang: Language): void {
    this.i18nService.setLanguage(lang);
  }

  toggleLanguage(): void {
    const newLang = this.i18nService.language === 'es' ? 'en' : 'es';
    this.i18nService.setLanguage(newLang);
  }

  getText(path: string): string {
    return this.i18nService.translate(path);
  }

  getTextWithVars(path: string, vars: Record<string, any>): string {
    return this.i18nService.translate(path, vars);
  }
}

/**
 * Factory para crear TranslationService compatible
 */
export function createLegacyTranslationService(i18nService: I18nService): LegacyTranslationServiceAdapter {
  initializeI18nWithLegacyTexts(i18nService);
  return new LegacyTranslationServiceAdapter(i18nService);
}
