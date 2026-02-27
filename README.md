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