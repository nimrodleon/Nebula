# API REST para registro de comprobantes electrónicos.

Esta es una API REST desarrollada con Symfony que permite el registro y gestión de comprobantes tributarios electrónicos, tales como boletas, facturas y notas de crédito. La aplicación almacena los archivos XML y CDR devueltos por SUNAT en una estructura de carpetas organizada por un identificador de empresa.

## Características principales

- Registro de Boletas: Permite registrar boletas electrónicas y almacenar los archivos XML y CDR devueltos por SUNAT.
- Registro de Facturas: Permite registrar facturas electrónicas y almacenar los archivos XML y CDR devueltos por SUNAT.
- Registro de Notas de Crédito: Permite registrar notas de crédito electrónicas y almacenar los archivos XML y CDR devueltos por SUNAT.
- Anulación de Operaciones: Los documentos registrados pueden ser anulados mediante la API.
- Autenticación y Autorización: La API implementa un sistema de autenticación y autorización para proteger los datos de los contribuyentes y garantizar un acceso seguro a la API.
- Almacenamiento de Archivos: Los archivos XML y CDR devueltos por SUNAT se almacenan en el sistema de archivos local.
- Gestión Multiempresa: La aplicación puede manejar múltiples contribuyentes, organizando los archivos y certificados en carpetas por un identificador de empresa.

## Requerimientos

- PHP 8.2 o superior
- Composer
- Symfony 6.3 o superior

## Instalación

1. Clonar el repositorio desde GitHub:

```bash
git clone git@github.com:nimrodleon/invoicehub.git
```

2. Instalar las dependencias con Composer:

```bash
cd invoicehub
composer install
```

3. Ejecutar el servidor de desarrollo:

```bash
symfony server:start
```

## Uso

Una vez que el servidor de desarrollo esté en ejecución, la API estará disponible en la URL http://127.0.0.1:8000.

Para registrar un documento tributario electrónico, se pueden enviar las peticiones HTTP correspondientes a los endpoints definidos en la API. Por ejemplo, para registrar una boleta electrónica:

```
POST /api/boletas
```

El cuerpo de la petición debe incluir los datos de la boleta en formato JSON. La API validará los datos y almacenará los archivos XML y CDR devueltos por SUNAT en la carpeta correspondiente.

Para más detalles sobre los endpoints disponibles y el formato de las peticiones, consulta la documentación interactiva de la API en la URL http://127.0.0.1:8000/api/doc.
