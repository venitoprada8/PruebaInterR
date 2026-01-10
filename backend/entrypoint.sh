#!/bin/sh

# Crear .env con las variables de entorno
cat > /app/.env << EOF
DB_SERVER=${DB_SERVER:-localhost}
DB_PORT=${DB_PORT:-3306}
DB_NAME=${DB_NAME:-StudentRegistrationDB}
DB_USER=${DB_USER:-root}
DB_PASSWORD=${DB_PASSWORD:-toor}
EOF

# Ejecutar la aplicación .NET
exec dotnet StudentRegistration.Api.dll
