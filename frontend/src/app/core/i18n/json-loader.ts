/**
 * Loader para cargar traducciones desde archivos JSON
 *
 * Permite lazy loading de traducciones desde archivos externos
 */

import { Translations } from './types';

/**
 * Carga traducciones desde un archivo JSON
 */
export async function loadTranslationsFromJSON(path: string): Promise<Translations> {
  try {
    const response = await fetch(path);

    if (!response.ok) {
      throw new Error(`Failed to load translations from ${path}: ${response.statusText}`);
    }

    const data = await response.json();
    return data as Translations;
  } catch (error) {
    console.error(`Error loading translations from ${path}:`, error);
    throw error;
  }
}

/**
 * Crea un provider de traducciones desde un path de JSON
 */
export function createJSONTranslationProvider(basePath: string = '/assets/i18n') {
  return (language: string): Promise<Translations> => {
    const path = `${basePath}/${language}.json`;
    return loadTranslationsFromJSON(path);
  };
}

/**
 * Loader para módulos ES
 * Útil con dynamic imports
 */
export function createModuleTranslationProvider(
  moduleLoader: (lang: string) => Promise<{ default: Translations }>
) {
  return async (language: string): Promise<Translations> => {
    const module = await moduleLoader(language);
    return module.default;
  };
}

/**
 * Ejemplo de uso con dynamic imports:
 *
 * const provider = createModuleTranslationProvider((lang) => {
 *   return import(`./translations/${lang}.ts`);
 * });
 */
