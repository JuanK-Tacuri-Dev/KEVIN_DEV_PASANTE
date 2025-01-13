# Guía para Clonar, Abrir y Publicar un Microservicio desde Azure DevOps usando Visual Studio 2022

## Paso 1: Clonar el Repositorio desde Azure DevOps

1. **Acceder a Azure DevOps**:
   - Ve a [Azure DevOps](https://dev.azure.com/) e inicia sesión con tus credenciales.

2. **Seleccionar el Proyecto**:
   - Entra en tu organización y selecciona el proyecto donde se encuentra el repositorio del microservicio.

3. **Ir a Repositorios**:
   - En el menú de la izquierda, selecciona "Repos".

4. **Clonar el Repositorio**:
   - En la vista del repositorio, haz clic en el botón "Clonar". Verás una URL para clonar el repositorio usando Git.
   - Copia la URL.

5. **Clonar con Git**:
   - Abre una terminal (CMD, PowerShell, Git Bash, etc.).
   - Navega a la carpeta donde quieres clonar el repositorio.
   - Ejecuta el siguiente comando, reemplazando `<URL_DEL_REPOSITORIO>` con la URL que copiaste:
     ```bash
     git clone <URL_DEL_REPOSITORIO>
     ```

## Paso 2: Abrir el Proyecto en Visual Studio 2022
1. **Abrir Visual Studio 2022**:
   - Abre Visual Studio 2022 desde tu menú de inicio o buscándolo en tu sistema.

2. **Abrir Proyecto/Solución**:
   - En Visual Studio, selecciona "Abrir un proyecto o solución".
   - Navega a la carpeta donde clonaste el repositorio y selecciona el archivo de solución (.sln).

3. **Restaurar Paquetes NuGet**:
   - Visual Studio debería detectar automáticamente los paquetes NuGet necesarios y restaurarlos. Si no, haz clic derecho en la solución y selecciona "Restaurar paquetes NuGet".

## Paso 3: Configurar y Ejecutar el Microservicio

1. **Configurar Variables de Entorno (si es necesario)**:
   - Asegúrate de que todas las variables de entorno necesarias estén configuradas. Puedes hacerlo en el archivo `appsettings.json` o directamente en las propiedades del proyecto en Visual Studio.

2. **Ejecutar el Microservicio**:
   - Configura el proyecto principal (si hay más de uno) haciendo clic derecho en el proyecto y seleccionando "Establecer como proyecto de inicio".
   - Haz clic en el botón de "Iniciar" (o presiona F5) para ejecutar el microservicio.

## Paso 4: Publicar el Microservicio

1. **Preparar la Publicación**:
   - Haz clic derecho en el proyecto y selecciona "Publicar".
   - Se abrirá el asistente de publicación.

2. **Seleccionar el Destino de Publicación**:
   - Puedes publicar en Azure, en una carpeta local, en un servidor FTP, etc.
   - Por ejemplo, para publicar en Azure:
     - Selecciona "Azure" y sigue las instrucciones para configurar el servicio de destino (puede ser una App Service, un contenedor, etc.).

3. **Configurar el Perfil de Publicación**:
   - Configura los detalles del perfil de publicación según el destino seleccionado.
   - Asegúrate de revisar las configuraciones de conexión, autenticación y cualquier otro detalle necesario.

4. **Publicar**:
   - Haz clic en "Publicar" y espera a que Visual Studio despliegue el microservicio en el destino seleccionado.

## Paso 5: Verificar la Publicación

1. **Acceder al Microservicio**:
   - Una vez publicado, verifica que el microservicio está funcionando correctamente accediendo a la URL del servicio o utilizando cualquier otra herramienta de prueba que uses habitualmente.

2. **Revisar Logs y Errores**:
   - Revisa los logs y cualquier error que pueda haber ocurrido durante la publicación para asegurarte de que todo funciona como se espera.

---

## Paso 6: Abrir el Proyecto en Visual Studio code
1. **Abrir Visual Studio code**:
   - Abre Visual Studio code desde tu menú de inicio o buscándolo en tu sistema.

2. **Abrir terminal**:
   - Abre una terminal en la raíz del proyecto donde se encuentra el archivo de  solución `Integration.Orchestrator.Backend.sln`.

3. **Restaurar Paquetes NuGet**:
   - Ejecuta el siguiente comando para restaurar los paquetes NuGet necesarios:
   ```bash
     dotnet restore Integration.Orchestrator.Backend.sln
   ``` 

4. **Configurar Variables de Entorno (si es necesario)**:
   - Asegúrate de que todas las variables de entorno necesarias estén configuradas. Puedes hacerlo en el archivo `appsettings.json`.

5. **Compilar proyecto**:
   - Luego, ejecuta el siguiente comando para compilar el proyecto:
   ```bash
     dotnet build Integration.Orchestrator.Backend.sln
   ```

6. **Ejecutar proyecto**:
   - Finalmente, para ejecutar el proyecto, usa el siguiente comando:
   ```bash
     dotnet run --project Integration.Orchestrator.Backend.Api/Integration.Orchestrator.Backend.Api.csproj
   ```

7. **Verificar proyecto**:
   - Para verificar que el proyecto es iniciado o levantado correctamente, debes ingresar a la siguiente URL:
   ```bash
     http://localhost:5052/swagger
   ```
   
   

---

¡Listo! Ahora has clonado, abierto y publicado tu microservicio desde Azure DevOps usando Visual Studio 2022 o Visual studio code.