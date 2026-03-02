# Turnos Amaris — Backend API

API RESTful para el sistema de agendamiento de turnos bancarios.  
Construida con **ASP.NET Core 8** y **Clean Architecture**.

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/AmarisTurnos.git
cd AmarisTurnos
```
### 2. Ejecutar la aplicación

**Visual Studio 2022:**
1. Abre `AmarisTurnos.sln`
2. Establece `Amaris.Api` como proyecto de inicio
3. Presiona `F5` o haz clic en **Ejecutar**

## Autenticación

La API usa **JWT Bearer Token**. Para acceder a los endpoints protegidos:

### Obtener el token

**POST** `/api/auth/login`

```json
{
  "username": "admin",
  "password": "admin123*"
}
```
**Respuesta:**
```json
{
  "success": true,
  "message": "Login exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "username": "admin",
    "expiracion": "2026-03-02T22:00:00Z"
  },
  "statusCode": 200
}
```
