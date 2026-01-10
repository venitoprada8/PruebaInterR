# Etapa 1: Build
FROM node:18-alpine AS build

WORKDIR /app

# Copiar archivos de dependencias
COPY package*.json ./

# Instalar dependencias
RUN npm install

# Copiar el código fuente
COPY . .

# Build de la aplicación
RUN npm run build

# Etapa 2: Serve
FROM node:18-alpine AS runtime

WORKDIR /app

# Instalar http-server para servir la aplicación
RUN npm install -g http-server

# Copiar los archivos compilados desde la etapa de build
COPY --from=build /app/dist ./dist

# Exponer puerto
EXPOSE 4200

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD wget -q --spider http://localhost:4200/ || exit 1

# Comando de inicio
CMD ["http-server", "dist", "-p", "4200", "--cors"]
