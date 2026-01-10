# Sistema de Internacionalización (i18n) Abstraído

## Descripción

Sistema robusto y flexible de internacionalización para Angular que proporciona:

- ✅ **Type-safe**: Tipado fuerte con TypeScript
- ✅ **Lazy Loading**: Carga de idiomas bajo demanda
- ✅ **Interpolación**: Variables en textos
- ✅ **Pluralización**: Manejo automático de plurales
- ✅ **Formateo**: Números, fechas, monedas según locale
- ✅ **Validación**: Verificación de traducciones completas
- ✅ **Fallback**: Idioma de respaldo automático
- ✅ **Sin dependencias**: Usa APIs nativas del navegador
- ✅ **Retrocompatible**: Adaptador para sistema anterior

---

## Instalación Rápida

### Opción 1: Usar con el sistema existente (Recomendado)

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { I18nService } from './core/i18n';
import { initializeI18nWithLegacyTexts } from './core/i18n/translation-adapter';

export const appConfig: ApplicationConfig = {
  providers: [
    // ... otros providers
    {
      provide: I18nService,
      useFactory: () => {
        const service = new I18nService();
        initializeI18nWithLegacyTexts(service);
        return service;
      }
    }
  ]
};
```

### Opción 2: Sistema completamente nuevo

```typescript
// app.config.ts
import { I18nService } from './core/i18n';

// En tu inicialización
const i18n = new I18nService();

// Registrar idiomas
i18n.registerLanguage('es', {
  common: {
    hello: 'Hola {name}',
    // ...
  }
});

i18n.registerLanguage('en', {
  common: {
    hello: 'Hello {name}',
    // ...
  }
});
```

---

## Uso Básico

### En un Componente

```typescript
import { Component } from '@angular/core';
import { I18nService } from './core/i18n';

@Component({
  selector: 'app-example',
  template: `
    <h1>{{ i18n.t('common.hello', { name: userName }) }}</h1>
    <p>{{ i18n.t('messages.welcome') }}</p>
    <button (click)="changeLanguage()">
      {{ i18n.language === 'es' ? 'English' : 'Español' }}
    </button>
  `
})
export class ExampleComponent {
  userName = 'Juan';

  constructor(public i18n: I18nService) {}

  changeLanguage() {
    const newLang = this.i18n.language === 'es' ? 'en' : 'es';
    this.i18n.setLanguage(newLang);
  }
}
```

---

## Características Avanzadas

### 1. Interpolación de Variables

```typescript
// Traducción
{
  greeting: 'Hola {name}, tienes {count} mensajes'
}

// Uso
i18n.t('greeting', { name: 'María', count: 5 });
// Resultado: "Hola María, tienes 5 mensajes"
```

### 2. Pluralización

```typescript
// En el componente
const message = i18n.tp({
  count: items.length,
  zero: 'No hay items',
  one: 'Hay 1 item',
  other: 'Hay {count} items'
});

// Ejemplos:
// count = 0  => "No hay items"
// count = 1  => "Hay 1 item"
// count = 5  => "Hay 5 items"
```

### 3. Formateo de Números

```typescript
// Número básico
i18n.formatNumber(1234.56);
// es: "1.234,56"
// en: "1,234.56"

// Porcentaje
i18n.formatNumber(0.75, { style: 'percent' });
// es: "75 %"
// en: "75%"
```

### 4. Formateo de Fechas

```typescript
const date = new Date('2025-01-09');

// Fecha corta
i18n.formatDate(date, { dateStyle: 'short' });
// es: "9/1/25"
// en: "1/9/25"

// Fecha larga
i18n.formatDate(date, { dateStyle: 'long' });
// es: "9 de enero de 2025"
// en: "January 9, 2025"

// Fecha y hora
i18n.formatDate(date, {
  year: 'numeric',
  month: 'long',
  day: 'numeric',
  hour: '2-digit',
  minute: '2-digit'
});
```

### 5. Formateo de Moneda

```typescript
// USD
i18n.formatCurrency(1234.56, 'USD');
// es: "1.234,56 US$"
// en: "$1,234.56"

// EUR
i18n.formatCurrency(1234.56, 'EUR');
// es: "1.234,56 €"
// en: "€1,234.56"
```

### 6. Tiempo Relativo

```typescript
const date = new Date(Date.now() - 1000 * 60 * 60 * 2); // Hace 2 horas

i18n.formatRelativeTime(date);
// es: "hace 2 horas"
// en: "2 hours ago"
```

### 7. Lazy Loading con JSON

```typescript
import { createJSONTranslationProvider } from './core/i18n';

// Registrar idiomas con lazy loading
const jsonProvider = createJSONTranslationProvider('/assets/i18n');

i18n.registerLanguage('es', jsonProvider);
i18n.registerLanguage('en', jsonProvider);
i18n.registerLanguage('fr', jsonProvider);

// Las traducciones se cargarán solo cuando se seleccione el idioma
await i18n.setLanguage('fr'); // Carga /assets/i18n/fr.json
```

Estructura de archivos JSON:

```
assets/
└── i18n/
    ├── es.json
    ├── en.json
    └── fr.json
```

Contenido de `es.json`:

```json
{
  "common": {
    "hello": "Hola {name}",
    "goodbye": "Adiós"
  },
  "messages": {
    "welcome": "Bienvenido a la aplicación"
  }
}
```

### 8. Dynamic Imports (Webpack)

```typescript
import { createModuleTranslationProvider } from './core/i18n';

const provider = createModuleTranslationProvider((lang) => {
  return import(`./translations/${lang}.ts`);
});

i18n.registerLanguage('es', provider);
```

### 9. Observar Cambios de Idioma

```typescript
// Método 1: Observable
i18n.language$.subscribe(lang => {
  console.log('Idioma cambió a:', lang);
});

// Método 2: Callback
const unsubscribe = i18n.onLanguageChange((newLang, oldLang) => {
  console.log(`Cambió de ${oldLang} a ${newLang}`);
});

// Cancelar suscripción
unsubscribe();
```

### 10. Validación de Traducciones

```typescript
// Validar que español tenga todas las claves de inglés
const result = i18n.validateLanguage('es', 'en');

if (!result.isValid) {
  console.log('Claves faltantes:', result.missingKeys);
  console.log('Claves extra:', result.extraKeys);
}

// Ejemplo de salida:
// {
//   isValid: false,
//   missingKeys: ['common.save', 'errors.network'],
//   extraKeys: ['old.deprecated']
// }
```

---

## Pipe para Templates

Crear un pipe personalizado:

```typescript
import { Pipe, PipeTransform } from '@angular/core';
import { I18nService } from './core/i18n';

@Pipe({
  name: 'translate',
  standalone: true,
  pure: false // Para que se actualice cuando cambie el idioma
})
export class TranslatePipe implements PipeTransform {
  constructor(private i18n: I18nService) {}

  transform(key: string, params?: any): string {
    return this.i18n.t(key, params);
  }
}
```

Uso en templates:

```html
<h1>{{ 'common.hello' | translate:{ name: userName } }}</h1>
<p>{{ 'messages.welcome' | translate }}</p>
```

---

## Migración desde el Sistema Antiguo

### Paso 1: Instalar el nuevo sistema

Ya está hecho. Los archivos están en `src/app/core/i18n/`.

### Paso 2: Usar el adaptador (Compatibilidad total)

```typescript
// En tu componente actual
import { Component } from '@angular/core';
import { TranslationService } from './services/translation.service';

// Cambia a:
import { I18nService } from './core/i18n';
import { LegacyTranslationServiceAdapter } from './core/i18n/translation-adapter';

@Component({
  // ...
})
export class MyComponent {
  // Antes:
  constructor(public translationService: TranslationService) {}

  // Después (mantiene la misma API):
  constructor(i18n: I18nService) {
    this.translationService = new LegacyTranslationServiceAdapter(i18n);
  }
}
```

### Paso 3: Migrar gradualmente

Una vez que funcione con el adaptador, puedes migrar componente por componente:

```typescript
// Antes:
get t() {
  return this.translationService.texts;
}

// Template:
{{ t.common.hello }}

// Después:
constructor(public i18n: I18nService) {}

// Template:
{{ i18n.t('common.hello') }}
```

---

## Configuración Avanzada

### Detectar idioma del navegador

```typescript
import { detectBrowserLanguage } from './core/i18n';

const browserLang = detectBrowserLanguage(); // 'es', 'en', etc.
i18n.setLanguage(browserLang);
```

### Múltiples idiomas con fallback

```typescript
i18n.registerLanguage('es', TEXTS_ES, {
  name: 'Español',
  locale: 'es-ES',
  fallback: 'en'
});

i18n.registerLanguage('pt', TEXTS_PT, {
  name: 'Português',
  locale: 'pt-BR',
  fallback: 'es' // Si falta una clave en PT, usar ES
});
```

### Idiomas RTL (Árabe, Hebreo, etc.)

```typescript
i18n.registerLanguage('ar', TEXTS_AR, {
  name: 'العربية',
  locale: 'ar-SA',
  direction: 'rtl'
});

// En tu componente global
i18n.language$.subscribe(lang => {
  const config = i18n.languageConfig;
  document.dir = config?.direction || 'ltr';
});
```

---

## API Completa

### Métodos Principales

| Método | Descripción |
|--------|-------------|
| `registerLanguage(code, translations, config?)` | Registra un idioma |
| `setLanguage(language)` | Cambia el idioma activo |
| `translate(key, params?)` / `t(key, params?)` | Traduce una clave |
| `translatePlural(options, params?)` / `tp(options, params?)` | Traduce con pluralización |
| `formatNumber(value, options?)` | Formatea número |
| `formatDate(date, options?)` | Formatea fecha |
| `formatCurrency(value, currency?)` | Formatea moneda |
| `formatRelativeTime(date)` | Formatea tiempo relativo |
| `validateLanguage(lang, reference?)` | Valida traducciones |
| `onLanguageChange(callback)` | Escucha cambios de idioma |

### Propiedades

| Propiedad | Descripción |
|-----------|-------------|
| `language` | Idioma actual |
| `locale` | Locale actual (para Intl API) |
| `languageConfig` | Configuración del idioma actual |
| `translations` | Todas las traducciones del idioma actual |
| `availableLanguages` | Lista de idiomas disponibles |
| `language$` | Observable de cambios de idioma |

---

## Ventajas vs Sistema Anterior

| Característica | Sistema Anterior | Sistema Nuevo |
|----------------|------------------|---------------|
| Interpolación | Básica | Avanzada con sintaxis clara |
| Pluralización | ❌ | ✅ Automática |
| Formateo | ❌ | ✅ Números, fechas, monedas |
| Lazy Loading | ❌ | ✅ Desde JSON o módulos |
| Validación | ❌ | ✅ Verifica completitud |
| Fallback | Básico | ✅ Multinivel configurable |
| Type-safe | Parcial | ✅ Completo |
| Extensible | Limitado | ✅ Totalmente pluggable |

---

## Mejores Prácticas

1. **Usar claves descriptivas**: `user.profile.editButton` en vez de `button1`
2. **Agrupar por contexto**: Organizar traducciones por feature/módulo
3. **Evitar HTML en traducciones**: Usar interpolación en su lugar
4. **Validar traducciones**: Ejecutar validación en CI/CD
5. **Lazy load idiomas grandes**: No cargar todos los idiomas al inicio
6. **Usar formateo nativo**: Aprovechar `Intl` API del navegador
7. **Mantener fallbacks**: Siempre tener inglés como respaldo

---

## Testing

```typescript
import { I18nService } from './core/i18n';

describe('MyComponent', () => {
  let i18n: I18nService;

  beforeEach(() => {
    i18n = new I18nService();
    i18n.registerLanguage('en', {
      test: {
        message: 'Hello {name}'
      }
    });
    i18n.setLanguage('en');
  });

  it('should translate correctly', () => {
    const result = i18n.t('test.message', { name: 'World' });
    expect(result).toBe('Hello World');
  });
});
```

---

## Roadmap Futuro

- [ ] Soporte para variables con formateo inline: `{count, number, percent}`
- [ ] Traducción de rutas (i18n routing)
- [ ] Herramientas CLI para extraer/validar traducciones
- [ ] Integración con servicios de traducción (Lokalise, Crowdin)
- [ ] Cache más inteligente con service workers
- [ ] Soporte para ICU Message Format
- [ ] Editor visual de traducciones

---

## Soporte

Para preguntas o problemas, revisa:
- Esta documentación
- Los archivos de ejemplo en `src/app/core/i18n/`
- El adaptador de compatibilidad para migración

## Licencia

Parte del proyecto Student Registration System.
