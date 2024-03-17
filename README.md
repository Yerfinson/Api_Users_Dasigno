# API de Usuarios

Esta API proporciona endpoints para la gestión de usuarios en un sistema. Permite la creación, lectura, actualización y eliminación de usuarios, así como la búsqueda paginada de usuarios ademas de crear la base de datos haciendo uso de entity framework con solo onfigurar la adena de conexion.

## Prerrequisitos

Antes de utilizar esta API, asegúrese de cumplir con los siguientes requisitos:

1. **Configuración de la cadena de conexión:**
   - Para que la API funcione correctamente, debe configurar la cadena de conexión en el archivo `appsettings.json`.
   - La estructura del archivo `appsettings.json` debe ser la siguiente:
     ```json
     {
       "Logging": {
         "LogLevel": {
           "Default": "Information",
           "Microsoft.AspNetCore": "Warning"
         }
       },
       "AllowedHosts": "*",
       "ConnectionStrings": {
         "ConnectionDB": "Server=<su_servidor>;Database=DB_Users;Trusted_Connection=true;TrustServerCertificate=true"
       }
     }
     ```
     - Reemplace `<su_servidor>` con la dirección del servidor de su base de datos. Por ejemplo, `Server=mi_servidor\SQLEXPRESS` si está utilizando SQL Server Express localmente.
   - Asegúrese de que la cadena de conexión esté configurada correctamente y tenga acceso adecuado a la base de datos especificada.

Una vez que haya configurado correctamente la cadena de conexión en el archivo `appsettings.json`, la API estará lista para su funcionamiento.

## Endpoints

### 1. Crear Usuario

- **URL:** `POST /api/Users/CreateUser`
- **Descripción:** Crea un nuevo usuario en el sistema.
- **Parámetros de entrada:**
  - `Primer_Nombre` (string, requerido): Primer nombre del usuario.
  - `Segundo_nombre` (string, opcional): Segundo nombre del usuario.
  - `Primer_Apellido` (string, requerido): Primer apellido del usuario.
  - `Segundo_Apellido` (string, opcional): Segundo apellido del usuario.
  - `Fecha_de_nacimiento` (string, requerido): Fecha de nacimiento del usuario en formato YYYY-MM-DD.
  - `Sueldo` (int, requerido): Sueldo del usuario.
- **Códigos de respuesta:**
  - `200 OK`: Usuario creado correctamente. Devuelve el usuario creado.
  - `400 Bad Request`: Error en la solicitud o datos proporcionados no válidos.

### 2. Obtener Usuario por ID

- **URL:** `GET /api/Users/GetUser/{userId}`
- **Descripción:** Obtiene un usuario por su ID.
- **Parámetros de entrada:**
  - `userId` (int, requerido): ID del usuario a buscar.
- **Códigos de respuesta:**
  - `200 OK`: Usuario encontrado correctamente. Devuelve el usuario.
  - `404 Not Found`: No se encontró el usuario con el ID especificado.
  - `500 Internal Server Error`: Error interno del servidor.

### 3. Buscar Usuarios Paginados

- **URL:** `GET /api/Users/SearchUsersPagination`
- **Descripción:** Busca usuarios según los criterios especificados, con paginación de resultados.
- **Parámetros de entrada:**
  - `firstName` (string, opcional): Primer nombre del usuario a buscar.
  - `lastName` (string, opcional): Apellido del usuario a buscar.
  - `pageNumber` (int, opcional): Número de página para la paginación (predeterminado: 1).
  - `pageSize` (int, opcional): Tamaño de la página para la paginación (predeterminado: 10).
- **Códigos de respuesta:**
  - `200 OK`: Búsqueda realizada correctamente. Devuelve la lista de usuarios encontrados.
  - `400 Bad Request`: Error en la solicitud o parámetros de paginación no válidos.
  - `404 Not Found`: No se encontraron usuarios que coincidan con los criterios de búsqueda.
  - `500 Internal Server Error`: Error interno del servidor.

### 4. Eliminar Usuario por ID

- **URL:** `DELETE /api/Users/DeleteUserById`
- **Descripción:** Elimina un usuario por su ID.
- **Parámetros de entrada:**
  - `userId` (int, requerido): ID del usuario a eliminar.
- **Códigos de respuesta:**
  - `200 OK`: Usuario eliminado correctamente. Devuelve el usuario eliminado.
  - `404 Not Found`: No se encontró el usuario con el ID especificado.
  - `500 Internal Server Error`: Error interno del servidor.

### 5. Actualizar Usuario por ID

- **URL:** `PUT /api/Users/UpdateUserFromUserId`
- **Descripción:** Actualiza un usuario existente por su ID.
- **Parámetros de entrada:**
  - `userId` (int, requerido): ID del usuario a actualizar.
  - `Primer_Nombre` (string, opcional): Nuevo primer nombre del usuario.
  - `Segundo_nombre` (string, opcional): Nuevo segundo nombre del usuario.
  - `Primer_Apellido` (string, opcional): Nuevo primer apellido del usuario.
  - `Segundo_Apellido` (string, opcional): Nuevo segundo apellido del usuario.
  - `Fecha_de_nacimineto` (string, opcional): Nueva fecha de nacimiento del usuario en formato YYYY-MM-DD.
  - `Sueldo` (int, opcional): Nuevo sueldo del usuario.
- **Códigos de respuesta:**
  - `200 OK`: Usuario actualizado correctamente. Devuelve el usuario actualizado.
  - `400 Bad Request`: Error en la solicitud o datos proporcionados no válidos.
  - `404 Not Found`: No se encontró el usuario con el ID especificado.
