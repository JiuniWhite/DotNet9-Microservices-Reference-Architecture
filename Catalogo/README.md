# 📦 Microservicio de Catálogo

Este servicio gestiona el catálogo de productos y categorías utilizando **PostgreSQL**.

## 🛠️ Tecnologías y Patrones
* **Framework:** .NET 9 (Minimal APIs)
* **Base de datos:** PostgreSQL 17 (Entity Framework Core)
* **Patrón de Mediación:** MediatR (CQRS)
* **Mapeo de Objetos:** AutoMapper
* **Validación:** FluentValidation
* **Documentación API:** Scalar

## 🔗 Endpoints (Vía API Gateway)
Todos los endpoints están etiquetados como **"Categoría"** en la interfaz Scalar.

| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/categories` | Listar todas las categorías. |
| `POST` | `/api/categories` | Crear una nueva categoría. |
| `PUT` | `/api/categories` | Actualizar una categoría existente. |

## ⚙️ Configuración (Variables de Entorno)
El servicio requiere la siguiente variable de entorno para conectarse a la base de datos:

* `ConnectionStrings__CatalogDb`: Cadena de conexión a PostgreSQL.

## 🚀 Cómo ejecutar
Este servicio depende de una base de datos PostgreSQL. Se recomienda ejecutarlo mediante el `docker-compose` en la raíz del repositorio.

**Ejecución manual (Docker):**
```bash
docker build -t catalog-api .
docker run -d -p 8080:80 --name catalog-api -e ConnectionStrings__CatalogDb="Host=db;Database=catalogdb;Username=user;Password=password" catalog-api