# Sistema de Venta de Entradas (Ticketing System) - Proyecto de Software

## Descripción
Plataforma robusta para la gestión de eventos y reserva de asientos, diseñada para garantizar la integridad de datos bajo alta demanda mediante el uso de Concurrencia Optimista y transacciones ACID.

## Arquitectura y Tecnologías

* **Backend:** .NET 8 con Clean Architecture (Domain, Application, Infrastructure, Api).
* **Patrones:** CQRS (Commands/Queries y Handlers), Repository Pattern y Unit of Work.
* **Persistencia:** Entity Framework Core con SQL Server y migraciones automáticas.
* **Seguridad:** Autenticación y Autorización basada en JWT.
* **Frontend:** Interfaz web asíncrona utilizando HTML5, CSS3 (Bootstrap) y JavaScript Vanilla.

## Requisitos Previos

* .NET 8 SDK.
* SQL Server LocalDB o SQLEXPRESS.

## Instrucciones de Configuración

1. Clonar el repositorio.
2. Configurar la cadena de conexión en `TicketingSystem.Api/appsettings.json`.
3. Ejecutar migraciones para crear la base de datos y cargar los datos iniciales (Seed):
   ```bash
   dotnet ef database update --project TicketingSystem.Infrastructure --startup-project TicketingSystem.Api
   ```

## Ejecución

1. Iniciar la API:
   ```bash
   dotnet run --project TicketingSystem.Api
   ```
2. Abrir el Frontend: Abrir el archivo `TicketingSystem.Frontend/Index.html` en un navegador.

## Pruebas
Comando para ejecutar la suite de tests unitarios e integrales:
```bash
dotnet test
```