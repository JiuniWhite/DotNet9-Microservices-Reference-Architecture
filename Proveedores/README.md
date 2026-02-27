# 🏢 Microservicio de Proveedores

Este servicio gestiona la información de proveedores y utiliza **MongoDB** para la persistencia de datos tipo documento.

## 🛠️ Tecnologías y Patrones
* **Framework:** .NET 9 (Minimal APIs)
* **Base de datos:** MongoDB
* **Patrón de Arquitectura:** Clean Architecture
* **Patrón de Casos de Uso:** Use Cases / Interactors

## 🔗 Endpoints (Vía API Gateway)
Todos los endpoints están prefijados con `/api/providers`.

| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/providers` | Listar todos los proveedores. |
| `POST` | `/api/providers` | Crear un nuevo proveedor. |
| `PUT` | `/api/providers` | Actualizar un proveedor existente. |
| `DELETE`| `/api/providers/{id}`| Eliminar un proveedor por ID. |

## ⚙️ Configuración (Variables de Entorno)
El servicio requiere la siguiente configuración para conectarse a MongoDB:

* `ConnectionStrings__ProviderDb`: Cadena de conexión a MongoDB.
* `DatabaseSettings__DatabaseName`: Nombre de la base de datos en Mongo.



## 🚀 Cómo ejecutar
Este servicio depende de MongoDB. Se recomienda ejecutarlo mediante el `docker-compose` en la raíz del repositorio.

**Ejecución manual (Docker):**
```bash
docker build -t provider-api .
docker run -d -p 8081:80 --name provider-api -e ConnectionStrings__ProviderDb="mongodb://mongo:27017" provider-api