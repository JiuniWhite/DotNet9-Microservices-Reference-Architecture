# 🛡️ API Gateway (YARP) & Consul Discovery

Punto de entrada único para el sistema. Utiliza **YARP (Yet Another Reverse Proxy)** para rutar peticiones a los microservicios, integrado dinámicamente con **Consul** para el descubrimiento de servicios.



## 🧠 Service Discovery con Consul
En lugar de IPs fijas, el Gateway consulta a Consul (`consul:8500`) para obtener la dirección IP y puerto actual de cada microservicio (`catalog-api`, `provider-api`). Si un contenedor se reinicia y cambia de IP, YARP lo detecta al instante gracias a Consul.

## ⚙️ Configuración y Ruteo (`appsettings.json`)
* **ReverseProxy:Routes**: Define las reglas de coincidencia de URL (`/api/catalog/*`).
* **ReverseProxy:Clusters**: Define los destinos y la integración con el proveedor de descubrimiento (`Consul`).

## 🔌 Puertos y URL
* **URL:** `http://localhost:8000`
* **Consul UI:** `http://localhost:8500`

## 🚀 Cómo ejecutar
Este componente requiere que la infraestructura de red esté activa.

**Ejecución manual (Docker):**
```bash
docker build -t api-gateway .
docker run -d -p 8000:80 --name api-gateway --network micro-network api-gateway