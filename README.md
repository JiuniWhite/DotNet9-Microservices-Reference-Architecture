# 🚀 .NET 9 Cloud-Native Microservices: Reference Architecture

![.NET 9](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue.svg)
![Architecture](https://img.shields.io/badge/Clean-Architecture-green.svg)

Esta es una **Arquitectura de Referencia Maestra** diseñada para sistemas de alta escalabilidad. Implementa un ecosistema de microservicios desacoplados utilizando **Service Discovery dinámico**, **API Gateway inteligente** y **Persistencia Políglota**.

## 🏗️ Diagrama de Arquitectura
Este diagrama muestra cómo **YARP** interactúa con **Consul** para rutar peticiones a los servicios correctos.

```mermaid
graph TD
    Client[Cliente/Browser] -->|Puerto 8000| Gateway[YARP Gateway]
    Gateway -->|Consulta| Consul[Consul Service Discovery]
    Consul -.->|Retorna IP:Puerto| Gateway
    Gateway -->|Proxy Pass| Catalog[Catalog API - Postgres]
    Gateway -->|Proxy Pass| Provider[Provider API - MongoDB]
    
    subgraph Infrastructure
        Consul
        DB1[(PostgreSQL)]
        DB2[(MongoDB)]
    end
    
    Catalog --> DB1
    Provider --> DB2

🛠️ Ficha Técnica de Arquitectura

    Edge Layer: YARP (Yet Another Reverse Proxy) con Rate Limiting y transformaciones de ruta.

    Service Mesh & Discovery: Consul para el registro dinámico de instancias.

    Microservices: Minimal APIs en .NET 9 bajo patrones de Clean Architecture.

    Persistence Layer:

        PostgreSQL 17 (Catalog Service) para datos relacionales complejos.

        MongoDB (Provider Service) para documentos NoSQL.

🚀 Cómo ejecutar el proyecto (Docker)

Solo necesitas tener Docker instalado.

    Levantar la infraestructura (Consul, DBs):
    Bash

    docker-compose up -d consul db-catalog provider-mongo

    Levantar los microservicios:
    Bash

    docker-compose up -d catalog-api provider-api api-gateway

Endpoints Clave:

    API Gateway (Entrada): http://localhost:8000

    Consul Dashboard: http://localhost:8500

    Scalar Doc (Catalog): http://localhost:8000/catalog/scalar/v1

💼 Puntos Clave para Defensa Técnica (CV)

    Por qué Consul: "No usamos IPs fijas en el Gateway. Si un servicio se cae y Docker lo reinicia con otra IP, Consul actualiza a YARP automáticamente en milisegundo
    Por qué Persistencia Políglota: "Catalog usa Postgres porque necesita integridad referencial (ACID). Provider usa Mongo porque necesita agilidad para cambiar la estructura de los datos sin migrar tablas".

    Clean Architecture: "Tengo separadas la lógica de negocio (Core) de la tecnología de base de datos (Infrastructure). Si mañana quiero cambiar Postgres por MySQL, no toco el núcleo de la aplicación".


### 🚀 Secuencia de Git para subirlo:

```