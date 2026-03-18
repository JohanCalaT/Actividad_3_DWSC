# Actividad 3 - DWSC (Desarrollo Web Basado en Servicios y Componentes)

Este proyecto desarrolla una API RESTful en **ASP.NET Core (.NET 10)** conectada a una base de datos **PostgreSQL** mediante Docker. Es la solución a la Actividad 3 de la asignatura, reemplazando la arquitectura original sugerida (Java EE / Jersey / WildFly) por tecnologías modernas de Microsoft.

La API permite realizar operaciones CRUD sobre una tabla `sampleusers`.

---

## 🚀 Requisitos Previos e Instalación

Para que el proyecto funcione al clonarlo, debes asegurarte de tener instaladas las siguientes herramientas en tu máquina local:

### 1. Instalar .NET 10 SDK
El código fue escrito utilizando **ASP.NET Core 10**. Debes descargar e instalar el SDK correspondiente a tu sistema operativo.
- **Descarga oficial:** [Página de descargas de .NET](https://dotnet.microsoft.com/download)
- Para verificar que se instaló correctamente, abre una terminal y ejecuta:
  ```bash
  dotnet --version
  ```

### 2. Instalar Docker Desktop
La base de datos PostgreSQL se ejecuta dentro de un contenedor Docker para aislar las dependencias y facilitar la ejecución.
- **Descarga oficial:** [Página de descargas de Docker](https://www.docker.com/products/docker-desktop)
- Verifica la instalación ejecutando:
  ```bash
  docker --version
  docker compose version
  ```

### 3. Git
Para clonar este repositorio:
- **Descarga oficial:** [Git-SCM](https://git-scm.com/downloads)

---

## 🛠️ Tutorial: Cómo Ejecutar el Proyecto

Sigue estos pasos cuidadosamente tras clonar el repositorio:

### Paso 1: Clonar el Repositorio
Abre una terminal y ejecuta:
```bash
git clone https://github.com/Johan-M-Developer/Actividad_3_DWSC.git
cd Actividad_3_DWSC
```

### Paso 2: Levantar la Base de Datos
La configuración de Docker Compose levantará una instancia de PostgreSQL en el puerto `5432` con la base de datos `dwsc` e inicializará automáticamente la tabla `sampleusers`.
```bash
docker compose up -d
```
*(Nota: Docker descargará la imagen de PostgreSQL si es la primera vez que lo ejecutas).*

### Paso 3: Ejecutar la API
Entra en la carpeta del proyecto C# e inicia el servidor de desarrollo (Kestrel):
```bash
cd UniversidadAPI
dotnet run
```
La consola te indicará la URL donde Kestrel está escuchando (por lo general, `http://localhost:5000` o `https://localhost:5001`). 

### Paso 4: (Opcional) Interactuar mediante Swagger
Puedes abrir en tu navegador:
`http://localhost:5000/swagger`
para interactuar gráficamente con la API.

---

## 📚 Respuestas Teóricas de la Práctica

A continuación se responden las preguntas solicitadas en la guía del profesor sobre la adaptación de este código:

### Justificación de Infraestructura (.NET vs Java EE)

**Kestrel vs. WildFly:**
WildFly es un servidor de aplicaciones completo nativo de Java EE diseñado para alojar múltiples aplicaciones (ficheros `.war`). Kestrel subvierte ese paradigma: en ASP.NET Core, la aplicación es un ejecutable independiente de consola que trae embebido su propio servidor web optimizado (Kestrel). Al usar `dotnet run`, Kestrel escucha las peticiones HTTP directamente, sin necesidad de instalar o mantener un servidor externo en el sistema anfitrión.

**¿Por qué en .NET no existe el archivo `web.xml`?**
El archivo `web.xml` (Deployment Descriptor) es específico de la especificación Servlets en Java. .NET Core posee una filosofía de configuración moderna basada en código (`Program.cs`) y JSON (`appsettings.json`), configurando su *pipeline* de middleware dinámicamente en RAM.

### Cuestionario de la Actividad

**Pregunta A: ¿En qué formato se obtienen los datos al acceder a un usuario específico y por qué?**
Por defecto, se obtienen en **JSON**. En ASP.NET Core, el pipeline configurado por `AddControllers()` inyecta serializadores de JSON de forma nativa. Sin embargo, **nuestro código ha sido adaptado con `AddXmlDataContractSerializerFormatters()`** para habilitar el Content Negotiation. Esto significa que si el cliente solicita explícitamente `Accept: application/xml`, la API responderá con un XML estructurado.

**Pregunta B: ¿Cómo se definen qué códigos de estado (200, 400, etc.) deben devolverse en cada operación?**
En los controladores de Web API que heredan de `ControllerBase`, los códigos HTTP se determinan retornando diferentes clases derivadas de `ActionResult`. 
- `Ok()` devuelve un **200 OK**.
- `NotFound()` devuelve un **404 Not Found**.
- `BadRequest()` devuelve un **400 Bad Request**.
- `NoContent()` devuelve un **204 No Content** (útil tras eliminar/actualizar).
- `CreatedAtAction()` devuelve un **201 Created** junto con la URI del nuevo recurso.
- `Conflict()` devuelve un **409 Conflict** (usado al intentar insertar `username` o `dni` duplicados).

**Pregunta C: ¿Es posible cambiar el orden de los usuarios sin cambiar la consulta SQL? ¿Cómo se hace en el código?**
**Sí, es plenamente posible usando C# LINQ.**
En nuestro método GET, hemos resuelto este requisito de la siguiente forma: 
1. Realizamos el acceso asíncrono `await _context.Usuarios.ToListAsync();`. Esto ejecuta un `SELECT * FROM sampleusers` simple en SQL y materializa la respuesta completa en la memoria del servidor.
2. Posteriormente, aplicamos `.OrderBy(u => u.Username).ToList()` a esa lista en la memoria de C#, logrando el ordenamiento deseado sin manipular el query enviado a PostgreSQL.

---

## 🧪 Guía de Pruebas (cURL y Content Negotiation)

Asegúrate de tener la API corriendo. Aquí se exponen los comandos para probar el funcionamiento integral:

### 1. Obtener Lista de Usuarios (Por defecto JSON)
*Podrás notar que están ordenados alfabéticamente por 'username'.*
```bash
curl -X GET http://localhost:5000/api/users
```

### 2. Obtener Lista de Usuarios (Forzando XML)
*Demuestra el "Content Negotiation".*
```bash
curl -X GET http://localhost:5000/api/users -H "Accept: application/xml"
```

### 3. Obtener Usuario Específico
```bash
curl -X GET http://localhost:5000/api/users/jperez
```

### 4. Insertar Usuario Exitosamente
```bash
curl -X POST http://localhost:5000/api/users \
-H "Content-Type: application/json" \
-d '{"username":"nuevoUser","password":"123","dni":"99887766X","name":"Pedro","surnames":"Paramo","age":40}'
# Devuelve HTTP 201 Created
```

### 5. Probar Validación (Fallo Intencional por DNI Duplicado)
```bash
curl -X POST http://localhost:5000/api/users \
-H "Content-Type: application/json" \
-d '{"username":"otroUser","password":"123","dni":"99887766X","name":"Fallara","surnames":"Seguro","age":20}'
# Devuelve HTTP 409 Conflict: "El DNI ya existe."
```
