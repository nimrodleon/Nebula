# Nebula

![License](https://img.shields.io/badge/license-GPL--3.0-blue)
![.NET](https://img.shields.io/badge/.NET-5.0-blue)
![Angular](https://img.shields.io/badge/Angular-12-red)
![PHP](https://img.shields.io/badge/PHP-7.4-blue)
![MongoDB](https://img.shields.io/badge/MongoDB-4.4-green)

Nebula es un software de facturación electrónica diseñado para generar comprobantes electrónicos para la SUNAT. Este proyecto es de código abierto y está licenciado bajo GPL-3.0. Nebula soporta la generación de facturas, boletas, notas de venta y tiene los módulos de gestión de comprobantes, punto de venta, catálogo de productos, contactos y cuentas por cobrar. El proyecto está desarrollado con .NET, Angular, PHP y utiliza MongoDB como base de datos.

## Tecnologías Utilizadas

- **Backend**: .NET
- **Frontend**: Angular
- **Servicios Adicionales**: PHP
- **Base de Datos**: MongoDB
- **Generación de XML**: [Greenter](https://github.com/thegreenter/greenter)

## Instalación y Configuración

1. **Clona el repositorio:**

```sh
git clone git@github.com:nimrodleon/Nebula.git
```

2. **Configura las dependencias del proyecto:**

   - .NET: [Instalación de .NET](https://dotnet.microsoft.com/download)
   - Angular: [Instalación de Angular](https://angular.io/guide/setup-local)
   - PHP: [Instalación de PHP](https://www.php.net/manual/es/install.php)
   - MongoDB: [Instalación de MongoDB](https://docs.mongodb.com/manual/installation/)

3. **Configura la base de datos:**

   - Asegúrate de tener MongoDB ejecutándose.
   - Configura las conexiones a la base de datos en los archivos de configuración del proyecto.

4. **Ejecuta la aplicación:**

   - Backend: `dotnet run`
   - Frontend: `ng serve`
   - Servicios adicionales: Ejecuta los scripts PHP según sea necesario.

## Contribuciones

Las contribuciones son bienvenidas. Por favor, sigue las pautas de contribución y asegúrate de que tus cambios estén bien documentados.

## Licencia

Este proyecto está licenciado bajo la licencia GPL-3.0. Para más detalles, consulta el archivo [LICENSE](LICENSE).
