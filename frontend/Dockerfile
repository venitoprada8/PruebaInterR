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

# Etapa 2: Serve con Nginx
FROM nginx:alpine

# Copiar archivo de configuración de Nginx
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

# Copiar los archivos compilados desde la etapa de build (browser folder para SSR)
COPY --from=build /app/dist/student-registration-frontend/browser /usr/share/nginx/html

# Exponer puerto
EXPOSE 4200

# Health check disabled - wget not available in nginx:alpine
# Service uses restart: unless-stopped in docker-compose for recovery
# HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
#   CMD wget -q --spider http://localhost:4200/ || exit 1

# Comando de inicio
CMD ["nginx", "-g", "daemon off;"]
