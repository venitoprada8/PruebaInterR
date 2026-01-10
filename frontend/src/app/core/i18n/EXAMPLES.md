# Ejemplos Prácticos de Uso

## Ejemplo 1: Componente Simple con Traducciones

```typescript
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { I18nService } from './core/i18n';

@Component({
  selector: 'app-greeting',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="greeting">
      <h1>{{ i18n.t('greeting.hello', { name: userName }) }}</h1>
      <p>{{ i18n.t('greeting.subtitle') }}</p>

      <button (click)="changeLanguage()">
        {{ i18n.language === 'es' ? 'Switch to English' : 'Cambiar a Español' }}
      </button>
    </div>
  `
})
export class GreetingComponent {
  userName = 'María';

  constructor(public i18n: I18nService) {}

  async changeLanguage() {
    const newLang = this.i18n.language === 'es' ? 'en' : 'es';
    await this.i18n.setLanguage(newLang);
  }
}
```

Traducciones:

```typescript
// es.ts
{
  greeting: {
    hello: '¡Hola {name}!',
    subtitle: 'Bienvenido a nuestra aplicación'
  }
}

// en.ts
{
  greeting: {
    hello: 'Hello {name}!',
    subtitle: 'Welcome to our application'
  }
}
```

---

## Ejemplo 2: Lista con Pluralización

```typescript
import { Component } from '@angular/core';
import { I18nService } from './core/i18n';

@Component({
  selector: 'app-cart',
  template: `
    <div class="cart">
      <h2>{{ i18n.t('cart.title') }}</h2>
      <p>{{ getItemsMessage() }}</p>
      <p>{{ i18n.t('cart.total') }}: {{ i18n.formatCurrency(total, 'USD') }}</p>
    </div>
  `
})
export class CartComponent {
  items = [
    { name: 'Laptop', price: 999 },
    { name: 'Mouse', price: 25 },
    { name: 'Keyboard', price: 75 }
  ];

  get total() {
    return this.items.reduce((sum, item) => sum + item.price, 0);
  }

  constructor(public i18n: I18nService) {}

  getItemsMessage(): string {
    return this.i18n.tp({
      count: this.items.length,
      zero: this.i18n.t('cart.noItems'),
      one: this.i18n.t('cart.oneItem'),
      other: this.i18n.t('cart.multipleItems', { count: this.items.length })
    });
  }
}
```

Traducciones:

```typescript
// es.ts
{
  cart: {
    title: 'Carrito de Compras',
    total: 'Total',
    noItems: 'No hay productos en el carrito',
    oneItem: 'Hay 1 producto en el carrito',
    multipleItems: 'Hay {count} productos en el carrito'
  }
}

// en.ts
{
  cart: {
    title: 'Shopping Cart',
    total: 'Total',
    noItems: 'No items in cart',
    oneItem: 'There is 1 item in cart',
    multipleItems: 'There are {count} items in cart'
  }
}
```

---

## Ejemplo 3: Formulario con Validaciones Traducidas

```typescript
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { I18nService } from './core/i18n';

@Component({
  selector: 'app-contact-form',
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <div class="form-group">
        <label>{{ i18n.t('form.name') }}</label>
        <input formControlName="name" />
        <div *ngIf="form.get('name')?.errors as errors" class="error">
          <span *ngIf="errors['required']">{{ i18n.t('form.errors.required', { field: i18n.t('form.name') }) }}</span>
          <span *ngIf="errors['minlength']">{{ getMinLengthError('name', errors['minlength']) }}</span>
        </div>
      </div>

      <div class="form-group">
        <label>{{ i18n.t('form.email') }}</label>
        <input type="email" formControlName="email" />
        <div *ngIf="form.get('email')?.errors as errors" class="error">
          <span *ngIf="errors['required']">{{ i18n.t('form.errors.required', { field: i18n.t('form.email') }) }}</span>
          <span *ngIf="errors['email']">{{ i18n.t('form.errors.invalidEmail') }}</span>
        </div>
      </div>

      <button type="submit" [disabled]="form.invalid">
        {{ i18n.t('form.submit') }}
      </button>
    </form>
  `
})
export class ContactFormComponent implements OnInit {
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    public i18n: I18nService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  getMinLengthError(field: string, error: any): string {
    return this.i18n.t('form.errors.minLength', {
      field: this.i18n.t(`form.${field}`),
      min: error.requiredLength
    });
  }

  onSubmit() {
    if (this.form.valid) {
      console.log(this.form.value);
    }
  }
}
```

Traducciones:

```typescript
// es.ts
{
  form: {
    name: 'Nombre',
    email: 'Correo Electrónico',
    submit: 'Enviar',
    errors: {
      required: 'El campo {field} es requerido',
      minLength: 'El campo {field} debe tener al menos {min} caracteres',
      invalidEmail: 'Correo electrónico inválido'
    }
  }
}

// en.ts
{
  form: {
    name: 'Name',
    email: 'Email',
    submit: 'Submit',
    errors: {
      required: 'The {field} field is required',
      minLength: 'The {field} field must be at least {min} characters',
      invalidEmail: 'Invalid email address'
    }
  }
}
```

---

## Ejemplo 4: Tabla con Formateo de Datos

```typescript
import { Component } from '@angular/core';
import { I18nService } from './core/i18n';

interface Transaction {
  id: number;
  date: Date;
  amount: number;
  description: string;
}

@Component({
  selector: 'app-transactions',
  template: `
    <div class="transactions">
      <h2>{{ i18n.t('transactions.title') }}</h2>

      <table>
        <thead>
          <tr>
            <th>{{ i18n.t('transactions.date') }}</th>
            <th>{{ i18n.t('transactions.description') }}</th>
            <th>{{ i18n.t('transactions.amount') }}</th>
            <th>{{ i18n.t('transactions.time') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let transaction of transactions">
            <td>{{ formatDate(transaction.date) }}</td>
            <td>{{ transaction.description }}</td>
            <td [class.negative]="transaction.amount < 0">
              {{ i18n.formatCurrency(transaction.amount, 'USD') }}
            </td>
            <td>{{ i18n.formatRelativeTime(transaction.date) }}</td>
          </tr>
        </tbody>
      </table>

      <div class="summary">
        <strong>{{ i18n.t('transactions.total') }}:</strong>
        {{ i18n.formatCurrency(totalAmount, 'USD') }}
      </div>
    </div>
  `
})
export class TransactionsComponent {
  transactions: Transaction[] = [
    { id: 1, date: new Date('2025-01-08'), amount: 250.00, description: 'Depósito' },
    { id: 2, date: new Date('2025-01-07'), amount: -50.00, description: 'Retiro' },
    { id: 3, date: new Date('2025-01-05'), amount: 1000.00, description: 'Transferencia' }
  ];

  get totalAmount() {
    return this.transactions.reduce((sum, t) => sum + t.amount, 0);
  }

  constructor(public i18n: I18nService) {}

  formatDate(date: Date): string {
    return this.i18n.formatDate(date, {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
}
```

Traducciones:

```typescript
// es.ts
{
  transactions: {
    title: 'Transacciones Recientes',
    date: 'Fecha',
    description: 'Descripción',
    amount: 'Monto',
    time: 'Hace',
    total: 'Total'
  }
}

// en.ts
{
  transactions: {
    title: 'Recent Transactions',
    date: 'Date',
    description: 'Description',
    amount: 'Amount',
    time: 'Time',
    total: 'Total'
  }
}
```

---

## Ejemplo 5: Lazy Loading de Idiomas

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { I18nService } from './core/i18n';
import { createJSONTranslationProvider } from './core/i18n/json-loader';

export function initializeI18n(i18n: I18nService) {
  return () => {
    // Provider para cargar desde JSON
    const jsonProvider = createJSONTranslationProvider('/assets/i18n');

    // Registrar idiomas con lazy loading
    i18n.registerLanguage('es', jsonProvider, {
      name: 'Español',
      locale: 'es-ES'
    });

    i18n.registerLanguage('en', jsonProvider, {
      name: 'English',
      locale: 'en-US'
    });

    i18n.registerLanguage('fr', jsonProvider, {
      name: 'Français',
      locale: 'fr-FR'
    });

    i18n.registerLanguage('de', jsonProvider, {
      name: 'Deutsch',
      locale: 'de-DE'
    });

    // Establecer idioma inicial
    return i18n.setLanguage('es');
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    {
      provide: APP_INITIALIZER,
      useFactory: initializeI18n,
      deps: [I18nService],
      multi: true
    }
  ]
};
```

```typescript
// Componente selector de idioma
@Component({
  selector: 'app-language-selector',
  template: `
    <select [(ngModel)]="selectedLang" (change)="onChange()">
      <option *ngFor="let lang of languages" [value]="lang.code">
        {{ lang.name }}
      </option>
    </select>
  `
})
export class LanguageSelectorComponent {
  languages = [
    { code: 'es', name: 'Español' },
    { code: 'en', name: 'English' },
    { code: 'fr', name: 'Français' },
    { code: 'de', name: 'Deutsch' }
  ];

  selectedLang: string;

  constructor(public i18n: I18nService) {
    this.selectedLang = i18n.language;
  }

  async onChange() {
    await this.i18n.setLanguage(this.selectedLang);
  }
}
```

---

## Ejemplo 6: Directiva de Traducción

```typescript
import { Directive, ElementRef, Input, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { I18nService } from './core/i18n';

@Directive({
  selector: '[appTranslate]',
  standalone: true
})
export class TranslateDirective implements OnInit, OnDestroy {
  @Input('appTranslate') key!: string;
  @Input() translateParams?: any;

  private subscription?: Subscription;

  constructor(
    private el: ElementRef,
    private i18n: I18nService
  ) {}

  ngOnInit() {
    this.updateTranslation();

    // Actualizar cuando cambie el idioma
    this.subscription = this.i18n.language$.subscribe(() => {
      this.updateTranslation();
    });
  }

  ngOnDestroy() {
    this.subscription?.unsubscribe();
  }

  private updateTranslation() {
    const translation = this.i18n.t(this.key, this.translateParams);
    this.el.nativeElement.textContent = translation;
  }
}
```

Uso:

```html
<h1 appTranslate="welcome.title" [translateParams]="{ name: userName }"></h1>
<p appTranslate="welcome.subtitle"></p>
```

---

## Ejemplo 7: Guard para Idioma en Rutas

```typescript
import { inject } from '@angular/core';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { I18nService } from './core/i18n';

export const languageGuard = (route: ActivatedRouteSnapshot) => {
  const i18n = inject(I18nService);
  const router = inject(Router);

  const requestedLang = route.paramMap.get('lang');

  if (requestedLang && i18n.hasLanguage(requestedLang)) {
    i18n.setLanguage(requestedLang);
    return true;
  }

  // Redirigir a la ruta con el idioma actual
  router.navigate([i18n.language, ...route.url.map(segment => segment.path)]);
  return false;
};
```

Configuración de rutas:

```typescript
export const routes: Routes = [
  {
    path: ':lang',
    canActivate: [languageGuard],
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'about', component: AboutComponent }
    ]
  },
  { path: '', redirectTo: '/es/home', pathMatch: 'full' }
];
```

---

## Ejemplo 8: Servicio de Notificaciones Traducido

```typescript
import { Injectable } from '@angular/core';
import { I18nService } from './core/i18n';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private i18n: I18nService) {}

  success(messageKey: string, params?: any) {
    const message = this.i18n.t(`notifications.${messageKey}`, params);
    this.show(message, 'success');
  }

  error(messageKey: string, params?: any) {
    const message = this.i18n.t(`notifications.${messageKey}`, params);
    this.show(message, 'error');
  }

  info(messageKey: string, params?: any) {
    const message = this.i18n.t(`notifications.${messageKey}`, params);
    this.show(message, 'info');
  }

  private show(message: string, type: 'success' | 'error' | 'info') {
    // Implementación de notificación (toast, snackbar, etc.)
    console.log(`[${type.toUpperCase()}] ${message}`);
  }
}
```

Uso:

```typescript
@Component({...})
export class UserComponent {
  constructor(
    private notifications: NotificationService,
    private userService: UserService
  ) {}

  async saveUser(user: User) {
    try {
      await this.userService.save(user);
      this.notifications.success('userSaved', { name: user.name });
    } catch (error) {
      this.notifications.error('saveFailed');
    }
  }
}
```

Traducciones:

```typescript
// es.ts
{
  notifications: {
    userSaved: 'Usuario {name} guardado correctamente',
    saveFailed: 'Error al guardar el usuario',
    // ...
  }
}
```

---

## Ejemplo 9: Validación en Desarrollo

```typescript
// validate-translations.ts
import { I18nService } from './core/i18n';

export function validateAllTranslations(i18n: I18nService): void {
  if (!environment.production) {
    const reference = 'en'; // Idioma de referencia
    const languages = i18n.availableLanguages.filter(lang => lang !== reference);

    languages.forEach(lang => {
      const result = i18n.validateLanguage(lang, reference);

      if (!result.isValid) {
        console.group(`⚠️ Translation issues in ${lang}`);

        if (result.missingKeys.length > 0) {
          console.warn('Missing keys:', result.missingKeys);
        }

        if (result.extraKeys.length > 0) {
          console.info('Extra keys:', result.extraKeys);
        }

        console.groupEnd();
      } else {
        console.log(`✅ Translations for ${lang} are complete`);
      }
    });
  }
}
```

Llamar en `app.config.ts`:

```typescript
export function initializeI18n(i18n: I18nService) {
  return () => {
    // ... registrar idiomas ...

    // Validar en desarrollo
    validateAllTranslations(i18n);

    return i18n.setLanguage('es');
  };
}
```

---

## Consejos Finales

1. **Siempre usa claves**: Nunca pongas texto directo en los templates
2. **Agrupa por contexto**: Organiza las traducciones lógicamente
3. **Usa interpolación**: En vez de concatenar strings
4. **Aprovecha el formateo**: Usa los métodos de formateo de i18n
5. **Valida regularmente**: Ejecuta validación en desarrollo
6. **Documenta claves complejas**: Agrega comentarios cuando sea necesario
7. **Testing**: Prueba tu app en todos los idiomas soportados
