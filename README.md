# EcoSystem Connect - Proyecto Final

Este repositorio contiene la estructura base del proyecto EcoSystem Connect, construido utilizando una **Arquitectura N-Capas** (N-Tier Architecture) para garantizar la mantenibilidad y escalabilidad del sistema.

## Estructura de Capas
* **EcoSystem.API (Capa de Presentación):** Proyecto ASP.NET Core Web API encargado exclusivamente de exponer los endpoints HTTP y comunicarse con el exterior.
* **EcoSystem.Business (Capa de Negocio):** Biblioteca de clases que centraliza las reglas de negocio, validaciones e interfaces de servicios.
* **EcoSystem.Data (Capa de Acceso a Datos):** Biblioteca de clases responsable de la comunicación directa con la base de datos mediante Entity Framework Core.
* **EcoSystem.Client (Capa Cliente):** Aplicación multiplataforma construida con .NET MAUI bajo el patrón MVVM, encargada de la interfaz de usuario y la interacción con la API.

**Regla de Oro Aplicada:** Cada capa tiene una responsabilidad única y la comunicación fluye de forma estrictamente descendente (Cliente -> API -> Business -> Data).

## Tecnologías Utilizadas
* C# y ASP.NET Core 8
* .NET MAUI (Multi-platform App UI)
* Entity Framework Core (Code-First)
* Base de Datos Relacional: PostgreSQL (alojado en Supabase)
* Seguridad: JWT y BCrypt
* Documentación de API: Swagger UI

## 🚀 Últimas Actualizaciones

### Registro de Usuarios (Autenticación)
Se integró la funcionalidad completa de registro de usuarios públicos, conectando la aplicación cliente con la API backend.

* **Arquitectura:** Implementación estricta del patrón MVVM (Model-View-ViewModel).
* **Modelo de Datos:** Se creó `UserLoginDto` en el cliente para mantener paridad exacta con los requerimientos del endpoint de la API, optimizando la transferencia de datos (únicamente `Username` y `Password`).
* **Interfaz Gráfica (UI):** Diseño limpio en `RegistroUsuarioPage.xaml` utilizando `Data Binding` para enlazar los campos de texto directamente con `RegistroUsuarioViewModel`.
* **Seguridad y Backend:** El cliente se comunica por medio de peticiones HTTP (`POST /api/Auth/register`) con `EcoSystem.API`. Las contraseñas son encriptadas mediante **BCrypt** antes de guardarse en la base de datos a través de Entity Framework (`AppDbContext`).
* **Navegación:** Retorno automático a la pantalla de Login tras un registro exitoso utilizando `Shell.Current.GoToAsync("..")`.

## 🛠️ Cómo ejecutar el entorno de desarrollo

Para que la funcionalidad de creación de usuarios (y el resto de la aplicación) opere correctamente, es indispensable ejecutar tanto el servidor (API) como el cliente de forma simultánea.

**Paso 1: Configuración Inicial**
1. Clonar el repositorio.
2. Configurar la cadena de conexión de PostgreSQL en `appsettings.json` dentro de `EcoSystem.API`.

**Paso 2: Levantar el Backend (API)**
1. Abre una terminal y navega a la carpeta del servidor: `cd EcoSystem.API`
2. Ejecuta el proyecto: `dotnet run`
3. Toma nota del puerto local asignado (ej. `https://localhost:5124`). La interfaz visual de Swagger se abrirá automáticamente en tu navegador para pruebas. *No cierres esta terminal.*

**Paso 3: Levantar el Cliente (MAUI)**
1. Abre una nueva terminal y navega a la carpeta de la aplicación: `cd EcoSystem.Client`
2. Verifica que la URL del `HttpClient` en los ViewModels apunte al puerto correcto de tu API local.
3. Ejecuta la aplicación en tu emulador o dispositivo local.