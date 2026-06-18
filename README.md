# EcoSystem Connect - Proyecto Final

Este repositorio contiene la estructura base del proyecto EcoSystem Connect, construido utilizando una **Arquitectura N-Capas** (N-Tier Architecture) para garantizar la mantenibilidad y escalabilidad del sistema.

## Estructura de Capas
* **EcoSystem.API (Capa de Presentación):** Proyecto ASP.NET Core Web API encargado exclusivamente de exponer los endpoints HTTP y comunicarse con el exterior.
* **EcoSystem.Business (Capa de Negocio):** Biblioteca de clases que centraliza las reglas de negocio, validaciones e interfaces de servicios.
* **EcoSystem.Data (Capa de Acceso a Datos):** Biblioteca de clases responsable de la comunicación directa con la base de datos mediante Entity Framework Core.

**Regla de Oro Aplicada:** Cada capa tiene una responsabilidad única y la comunicación fluye de forma estrictamente descendente (API -> Business -> Data).



## Tecnologías Utilizadas
* C# y ASP.NET Core 8
* Entity Framework Core (Code-First)
* Base de Datos Relacional: PostgreSQL (alojado en Supabase)
* Documentación de API: Swagger UI


## Instrucciones de Ejecución
1. Clonar el repositorio.
2. Configurar la cadena de conexión de PostgreSQL en `appsettings.json`.
3. Abrir la terminal en la carpeta `EcoSystem.API`.
4. Ejecutar el comando `dotnet run`.