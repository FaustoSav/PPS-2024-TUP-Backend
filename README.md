# PPS-2024-TUP-Backend - Requisitos y Pasos para levantar el Backend


Requisitos Previos

- .NET 6.0 
- MySql 
- Visual Studio 2022 (opcional)


Instrucciones para Correr el Proyecto

1. Clonar el Repositorio

   ```bash
   git clone [https://github.com/tu_usuario/tu_repositorio.git](https://github.com/FaustoSav/PPS-2024-TUP-Backend.git)
   cd PPS-2024-TUP-Backend
   ```

2. Configurar Variables de Entorno

   Crea/Modifica el archivo `appsettings.Development.json` en la raíz del proyecto y configura las variables necesarias. Ejemplo:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=tu_base_de_datos;User Id=tu_usuario;Password=tu_contraseña;"
     },
     "JwtSettings": {
       "SecretKey": "tu_clave_secreta",
       "Issuer": "tu_issuer",
       "Audience": "tu_audience",
       "ExpiryInMinutes": 60
     }
   }
   ```

3. Restaurar Dependencias

   ```bash
   dotnet restore
   ```

4. Aplicar Migraciones a la Base de Datos

   ```bash
   dotnet ef database update
   ```

5. Correr la Aplicación en Modo Desarrollo
   
  - Situarse dentro de el Repositorio clonado y abrir el archivo `FornitureStore.sln` 

   La aplicación estará disponible en `https://localhost:7075`.


   Esto abrira una nueva ventana en localhost usando Swagger donde podrás probar todos los Endpoints.


Estructura de Directorios

- `Controllers/`: Controladores de la API.
- `Models/`: Modelos de datos y Dtos (Objetos de transferencia de datos).
- `Data/`: Contexto de la base de datos y configuraciones de entidades.
- `Services/`: Lógica de negocio y servicios.


Dependencias Principales

- `Microsoft.EntityFrameworkCore`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `BCrypt.Net-Next`
- `Microsoft.IdentityModel.Tokens`
- `Pomelo.EntityFrameworkCore.MySql`
- 

Contacto

Para cualquier duda o consulta, contacta al desarrollador principal en savoyafausto@gmail.com.

---
