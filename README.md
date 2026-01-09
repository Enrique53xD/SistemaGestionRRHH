# Sistema de Gestion de Recursos Humanos

## Descripcion del Proyecto
Este proyecto es una solucion integral para la gestion de Recursos Humanos, desarrollada como parte de una evaluacion tecnica de Ingenieria de Software. La aplicacion permite la administracion de empresas, colaboradores y la estructura geografica operativa, implementando una arquitectura moderna de cliente-servidor utilizando .NET 8.

## Caracteristicas y Funcionalidades

El sistema cumple con la totalidad de los requerimientos solicitados, incluyendo validaciones de negocio y funcionalidades extendidas:

### Modulos Principales
* **Gestion Geografica:** Mantenimiento completo (Crear, Leer, Actualizar, Eliminar) de Paises, Departamentos y Municipios. Incluye proteccion de integridad referencial para evitar la perdida de datos vinculados.
* **Gestion de Empresas:** Registro y administracion de empresas con validacion estricta de NIT unico y formatos de contacto.
* **Gestion de Colaboradores:** Administracion de personal con validaciones de edad y correo electronico unico.

### Funcionalidades Avanzadas
* **Relacion Muchos a Muchos:** Implementacion tecnica que permite asignar un colaborador a multiples empresas simultaneamente.
* **Busqueda en Tiempo Real:** Sistema de filtrado instantaneo para localizar colaboradores por nombre, correo o empresa asignada.
* **Dashboard Ejecutivo:** Panel de control inicial con metricas y contadores generales del sistema.

## Tecnologias Utilizadas

* **Backend:** ASP.NET Core Web API (.NET 8)
* **Frontend:** Blazor WebAssembly
* **Base de Datos:** SQL Server (Alojada en Azure SQL)
* **ORM:** Entity Framework Core
* **Estilo:** Bootstrap 5

## Instrucciones de Instalacion y Ejecucion

### Requisitos Previos
* Visual Studio 2022 (con carga de trabajo ASP.NET y desarrollo web).
* SDK de .NET 8.0.

### Pasos para ejecutar
1.  Clonar el repositorio o descargar el codigo fuente.
2.  Abrir el archivo de solucion `PdcEvaluacion.slnx` en Visual Studio.
3.  Configurar la solucion para inicio multiple:
    * Clic derecho en la Solucion -> Propiedades.
    * Seleccionar "Proyectos de inicio multiples".
    * Establecer la accion "Iniciar" para `PdcEvaluacion.API` y `PdcEvaluacion.Web`.
4.  Ejecutar la aplicacion.

## Configuracion de Base de Datos

El proyecto se encuentra configurado por defecto para conectarse a una base de datos de demostracion en Azure SQL, por lo que no requiere configuracion local inmediata.

**Nota para despliegue local:**
Si desea ejecutar la base de datos localmente debido a restricciones de red:
1.  Utilice el archivo `Script_BaseDeDatos.sql` adjunto en la raiz del repositorio.
2.  Ejecute el script en su servidor SQL Server local.
3.  Modifique la cadena de conexion en el archivo `appsettings.json` del proyecto API.

## Autor
Desarrollado por Enrique
