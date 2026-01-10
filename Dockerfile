# This is a placeholder Dockerfile
# Dockploy will use docker-compose.yml instead
FROM alpine:latest

LABEL description="Student Registry Application"

WORKDIR /app

COPY docker-compose.yml .

# This container just exits, docker-compose handles the actual deployment
CMD ["echo", "Docker Compose deployment managed by Dockploy"]
