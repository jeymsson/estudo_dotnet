# API - Stock, Component and Portfolio Management

This project is a simple API to manage stocks, components and portfolios.

## Architectural diagram

[Architectural diagram](./docs/C4%20Model/1%20-%20Context.puml)

![Architectural diagram](./docs/C4%20Model/1%20-%20Context.png)

## Services

- **App ASP.NET Core**:

   - **API**: API to manage stocks, components and portfolios.

- **Database**:

   - **MS SQL Server**: Database to store data.

- **Host telemetry**:

   - **Node Exporter**: Collects host metrics.

- **App telemetry**:

   - **Tempo**: Collects application metrics.

- **Logs telemetry**:

   - **Loki**: Collects and stores logs.

- **Tracking telemetry**:

   - **Jaeger**: Collects and stores traces.

- **Telemetry**:

   - **OpenTelemetry Collector**: Collects and stores telemetry data.
   - **Prometheus**: Collects and stores metrics.

- **Data visualization**:

   - **Grafana**: Visualizes metrics and logs.

## Project Structure

- `docker-compose.yml`: Docker Compose file to run the services.
- `prometheus.yml`: Prometheus configuration file.
- `grafana/provisioning/datasources/datasources.yml`: Grafana provisioning configuration for datasources.
- `grafana/provisioning/dashboards/dashboards.yml`: Grafana provisioning configuration for dashboards.
- `Dockerfile`: Dockerfile to build the API image.

## Requirements

- Docker
- Docker Compose

## Running the Project

```sh
git clone <REPOSITORY_URL>
cd <REPOSITORY_NAME>
docker compose up -d
```

# Accesses

- API: http://localhost:5001/scalar
- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000
- Node Exporter: http://localhost:9100
- Jaeger: http://localhost:16686
