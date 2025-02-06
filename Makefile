# Nome do arquivo: Makefile

# Variáveis
COMPOSE_FILE=docker-compose.yml
COMPOSE_TELEMETRY_FILE=./environment_tools/docker-compose.telemetry.all.yml

# Alvos
.PHONY: up down create-network destroy rebuild k6-tests sonarqube elk

# Alvo para criar a rede se não existir
create-network:
	@if ! docker network inspect opentelemetry >/dev/null 2>&1; then \
		echo "Rede 'opentelemetry' não encontrada. Criando rede..."; \
		docker network create opentelemetry; \
	else \
		echo "Rede 'opentelemetry' já existe."; \
	fi

# Alvo para subir os serviços
up: create-network
	@echo "Subindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile all \
					-f $(COMPOSE_FILE) up -d

# Alvo para derrubar os serviços
down:
	@echo "Derrubando os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile all \
					-f $(COMPOSE_FILE) down

# Alvo para destruir os serviços
destroy:
	@echo "Destruindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile all \
					-f $(COMPOSE_FILE) down -v

# Alvo para reconstruir os serviços
rebuild:
	@echo "Reconstruindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_FILE) build

# Alvo para subir os serviços do ELK
otel: create-network
	@echo "Subindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile otel \
					-f $(COMPOSE_FILE) up -d

# Alvo para rodar os testes com k6
k6-tests: create-network
	@echo "Reconstruindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile k6-tests \
					-f $(COMPOSE_FILE) up -d --build

# Alvo para subir os serviços do SonarQube
sonarqube:
	@echo "Destruindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile sonarqube \
					-f $(COMPOSE_FILE) up -d

# Alvo para subir os serviços do ELK
elk: create-network
	@echo "Subindo os serviços com Docker Compose..."
	docker-compose -f $(COMPOSE_TELEMETRY_FILE) --profile elk \
					-f $(COMPOSE_FILE) up -d