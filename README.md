
# 📕 Introducción

El sistema esta basado en un template hecho por mi para una charla de buenas practicas que di.
Este contiene todo lo siguiente:
 - Desacopla los endpoint de minimal api en pequeñas funciones para poder someterlos a prueba mas facilmente
 - Manejo de excepciones globales
 - Manejo de errores de validacion en de inputs a ValidationProblems
 - Desacoplamiento de los archivos de configuracion (appsettings) a traves de objetos fuertemente tipados
 - Uso de global.json dentro de la carpeta de solución para que el proyecto no tenga conflictos con otras versiones del SDK instaladas, etc.
 - Implementacion de Serilog
 - Proyecto de pruebas unitarias.
 - Uso de HealtChecks en caso de aplicaciones contenerizadas

Al ser una BD de ejercicios de lectura, utilice un patron mas de CQRS, evitando asi tener que crear clases, servicios y repositorios mas complejas para querys de lectura solamente.

Hay un proyecto completo de pruebas unitarias el cual se puede ejecutar para correr todas las pruebas desarrolladas hacia todos los endpoints solicitados.

# 📖 Consideraciones

- Por defecto el puerto de inicio de la aplicacion es el 2716, pero si se modifica, tambien hay una variable global en la coleccion de postman para modificar el puerto.
- Hay una variable global en la coleccion para modificar la apikey y agregar la authorization automaticamente para cada consulta.
- En la raiz del proyecto hay 2 archivos "postman_collecion_v20" y "postman_collecion_v21". Importar la coleccion segun corresponda la version de Postman.
- Ya que en la entrevista preguntaron sobre contenedores, agregue tambien un Dockerfile que es posible ejecutar para contenerizar la aplicacion.
- Para efectos de la prueba, se realizo un Middleware sencillo que verifica una ApiKey (tambien en swagger). la ApiKey se encuentra en authentication.json aunque para codigos mas grande es mejor utilizar algun KeyVault o Secretos.
- Se utilizó una Base de Datos en memoria para que sea mas facil de acceder y compilar el codigo, aunque el DbContext aguante cambiar esta BD a una real.
- Se utilizó un HostedService para llenar la BD desde el Json, para desacoplar la aplicacion y la migracion de los datos del Json.

# 💻 Una vez compilado y el sistema corriendo
## 🔗 Swagger: https://localhost:2716/swagger
## 🔑 ApiKey:  3ecdc1da15d04217b10d4508150453a4
