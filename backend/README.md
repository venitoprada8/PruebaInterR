# Registro de estudiantes Backend

Este proyecto es una API RESTful desarrollada en .NET implementando Clean Architecture. Su objetivo es gestionar el registro de estudiantes, profesores y materias, permitiendo realizar inscripciones y controlar los creditos academicos.

## Tecnologias

- .NET
- Entity Framework Core
- MySQL
- Swagger (OpenAPI)

## Estructura de la Solucion

La solucion sigue los principios de Clean Architecture distribuyendo las responsabilidades en las siguientes capas:

- **src/Api**: Capa de presentacion que contiene los controladores REST y la configuracion de inicio (Program.cs).
- **src/Application**: Contiene la logica de negocio, servicios, interfaces de servicios y DTOs (Data Transfer Objects).
- **src/Domain**: El nucleo del negocio con las entidades, validaciones, excepciones de dominio e interfaces del repositorio.
- **src/Infrastructure**: Detalles de implementacion como el acceso a datos (Entity Framework), configuraciones de base de datos y migraciones.

## Requisitos Previos

Antes de ejecutar la aplicacion, asegurese de tener instalado:

1. .NET SDK
2. Un servidor de base de datos MySQL

## Configuracion

La aplicacion utiliza un archivo `.env` para gestionar la configuracion sensible como la conexion a la base de datos.

1. En la raiz del proyecto, busque el archivo `.env.example`.
2. Cree una copia de este archivo y nombrelo `.env`.
3. Abra el archivo `.env` y configure las variables segun su entorno local:

```env
DB_SERVER=localhost
DB_PORT=3306
DB_NAME=StudentRegistrationDB
DB_USER=root
DB_PASSWORD=su_contraseña
```

Nota: La aplicacion intentara leer este archivo al iniciar para construir la cadena de conexion a MySQL.

## Ejecucion

Para poner en marcha el proyecto siga estos pasos:

1. Abra una terminal en la raiz del proyecto.

2. Restaure las dependencias:
   ```bash
   dotnet restore
   ```

3. Ejecute el proyecto de la API:
   ```bash
   dotnet run --project src/Api/StudentRegistration.Api.csproj
   ```

Al iniciar en entorno de desarrollo, la aplicacion intentara conectar a la base de datos, aplicar migraciones pendientes y cargar datos iniciales (seed data) automaticamente.

Una vez iniciada, puede acceder a la interfaz de documentacion Swagger navegando a la URL raiz indicada en la consola (por ejemplo: `http://localhost:5xxx` o `https://localhost:7xxx`).

<!-- ## Pruebas

El proyecto incluye pruebas unitarias y de integracion. Para ejecutarlas:

```bash
dotnet test
``` -->

## Gestion de Base de Datos

Si realiza cambios en las entidades del Dominio y necesita actualizar la base de datos:

**Crear una nueva migracion:**
```bash
dotnet ef migrations add NombreMigracion --project src/Infrastructure --startup-project src/Api
```

**Actualizar la base de datos manualmente:**
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```
