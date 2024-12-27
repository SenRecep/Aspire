# Aspire Microservice Starter

[![NuGet](https://img.shields.io/nuget/v/SenRecep.Aspire.svg)](https://www.nuget.org/packages/SenRecep.Aspire)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SenRecep.Aspire.svg)](https://www.nuget.org/packages/SenRecep.Aspire)
[![GitHub](https://img.shields.io/github/license/SenRecep/Aspire)](https://github.com/SenRecep/Aspire)
[![GitHub Stars](https://img.shields.io/github/stars/SenRecep/Aspire)](https://github.com/SenRecep/Aspire)

A modern, production-ready microservices template built with .NET Aspire 9.0. This template provides a robust foundation for building scalable, maintainable, and cloud-ready microservices applications.

## Features

- **.NET Aspire Integration**: Built on .NET 9.0 with full Aspire support for cloud-native development
- **Microservices Architecture**:
  - Product Service
  - Category Service
  - API Gateway (YARP Proxy)
- **Building Blocks**:
  - Domain-Driven Design (DDD) structure
  - Clean Architecture implementation
  - Shared infrastructure components
- **Infrastructure Components**:
  - PostgreSQL database integration
  - Redis caching
  - RabbitMQ message broker
  - Seq logging
  - Keycloak authentication
- **Development Features**:
  - Centralized package management
  - Code analysis and quality tools
  - Entity Framework Core with PostgreSQL
  - MassTransit for message handling
  - OpenTelemetry integration

## Project Structure

```
source/
├── src/
│   ├── BuildingBlocks/           # Shared components and utilities
│   │   ├── Caching/             # Redis and base caching implementations
│   │   ├── Database/            # Database access and migrations
│   │   ├── Logging/            # Serilog implementation
│   │   ├── MessageBrokers/     # MassTransit and message broker implementations
│   │   └── OpenTelemetry/      # Telemetry and monitoring
│   ├── Services/                # Microservices
│   │   ├── ProductService/     # Product management service
│   │   ├── CategoryService/    # Category management service
│   │   └── ProxyService/      # YARP API Gateway
│   └── Host/                    # Aspire host and configuration
└── tests/                       # Test projects
```

## Prerequisites

- .NET 9.0 SDK
- Docker Desktop
- Visual Studio 2022 or later (recommended)

## Installation

1. Install the template:

```bash
dotnet new install SenRecep.Aspire
```

2. Create a new project:

```bash
dotnet new aspire-microservice -n YourCompanyName
```

## Getting Started

1. Clone the repository
2. Navigate to the solution directory
3. Run the application:

```bash
cd source/src/Host/CompanyName.AppHost
dotnet run
```

## Development Environment

The template uses .NET Aspire's orchestration to manage:

- PostgreSQL databases for each service
- Redis cache
- RabbitMQ message broker
- Seq for centralized logging
- Keycloak for authentication

## Architecture

This template follows Clean Architecture principles with a domain-driven design approach:

- **Domain Layer**: Core business logic and entities
- **Application Layer**: Use cases and business rules
- **Infrastructure Layer**: External concerns and implementations
- **Presentation Layer**: API endpoints and controllers

## Configuration

The template uses centralized package management through `Directory.Packages.props` and shared build properties in `Directory.Build.props`.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
