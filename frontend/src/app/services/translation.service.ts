import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppTexts, TEXTS_MAP, Language } from '../constants/texts';

@Injectable({
  providedIn: 'root',
})
export class TranslationService {
  private currentLanguage: Language = 'es'; // Idioma por defecto
  private languageSubject = new BehaviorSubject<Language>(this.currentLanguage);

  // Observable para que los componentes se suscriban a cambios de idioma
  public language$: Observable<Language> = this.languageSubject.asObservable();

  constructor() {
    // Cargar idioma guardado en localStorage si existe
    if (typeof localStorage !== 'undefined') {
      const savedLanguage = localStorage.getItem('app-language') as Language;
      if (savedLanguage && TEXTS_MAP[savedLanguage]) {
        this.currentLanguage = savedLanguage;
        this.languageSubject.next(this.currentLanguage);
      }
    }
  }

  /**
   * Obtiene todos los textos del idioma actual
   */
  get texts(): AppTexts {
    return TEXTS_MAP[this.currentLanguage];
  }

  /**
   * Obtiene el idioma actual
   */
  get language(): Language {
    return this.currentLanguage;
  }

  /**
   * Cambia el idioma de la aplicación
   * @param lang - Nuevo idioma ('es' | 'en')
   */
  setLanguage(lang: Language): void {
    if (TEXTS_MAP[lang]) {
      this.currentLanguage = lang;
      this.languageSubject.next(lang);
      localStorage.setItem('app-language', lang);
    } else {
      console.error(`Language "${lang}" is not supported`);
    }
  }

  /**
   * Alterna entre español e inglés
   */
  toggleLanguage(): void {
    const newLang: Language = this.currentLanguage === 'es' ? 'en' : 'es';
    this.setLanguage(newLang);
  }

  /**
   * Obtiene un texto específico usando dot notation
   * Ejemplo: getText('common.loading') -> 'Cargando...'
   * @param path - Ruta al texto (ej: 'common.loading')
   */
  getText(path: string): string {
    const keys = path.split('.');
    let result: any = this.texts;

    for (const key of keys) {
      if (result && typeof result === 'object' && key in result) {
        result = result[key];
      } else {
        console.warn(`Text path "${path}" not found`);
        return path; // Devolver la ruta si no se encuentra
      }
    }

    return typeof result === 'string' ? result : path;
  }

  /**
   * Obtiene un texto con interpolación de variables
   * Ejemplo: getTextWithVars('enrollment.maxSubjectsReached', {max: 3})
   * @param path - Ruta al texto
   * @param vars - Variables a interpolar
   */
  getTextWithVars(path: string, vars: Record<string, any>): string {
    let text = this.getText(path);

    Object.keys(vars).forEach((key) => {
      text = text.replace(new RegExp(`{${key}}`, 'g'), String(vars[key]));
    });

    return text;
  }
}
