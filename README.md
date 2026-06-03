# EcoSystem Connect - Proyecto Final

Este repositorio contiene la estructura base del proyecto EcoSystem Connect, construido utilizando una **Arquitectura N-Capas** (N-Tier Architecture) para garantizar la mantenibilidad y escalabilidad del sistema.

## Estructura de Capas
* **EcoSystem.API (Capa de Presentación):** Proyecto ASP.NET Core Web API encargado exclusivamente de exponer los endpoints HTTP y comunicarse con el exterior.
* **EcoSystem.Data (Capa de Acceso a Datos):** Biblioteca de clases responsable de la comunicación directa con la base de datos mediante Entity Framework Core.

**Regla de Oro Aplicada:** Cada capa tiene una responsabilidad única y la comunicación fluye de forma estrictamente descendente.