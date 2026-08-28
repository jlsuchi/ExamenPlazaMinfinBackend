# Examen Plaza MINFIN - Experto en sistemas financieros

Proyecto desarrollado como parte del Examen de Experto de Sistemas Financieros.

## Descripción

Backend desarrollado en .NET para la gestión y procesamiento de solicitudes de pago,
aplicando una arquitectura distribuida y comunicación mediante mensajería.

## Tecnologías utilizadas

- .NET
- PostgreSQL
- MongoDB
- RabbitMQ
- Git
- Angular 21
- .NET CORE  9

## Funcionalidades principales

- Gestión de usuarios y roles
- Gestión de entidades
- Solicitudes de pago
- Procesamiento de mensajes mediante RabbitMQ
- Manejo de eventos
- Auditoría y trazabilidad
- Control de acceso por roles

## Arquitectura

La solución utiliza servicios .NET, bases de datos PostgreSQL y MongoDB,
RabbitMQ para mensajería y una capa de integración para comunicación
con sistemas externos.

Para los procesos distribuidos se utilizan patrones como Outbox y Saga.

## Documentación

La documentación completa del proyecto se encuentra en:

 `Documentacion/EXAMEN_EXPERTO_JUAN_LUIS_SUCHI.pdf`

La documentación incluye:

- Arquitectura de la solución
- Modelo de base de datos
- Diseño de colas RabbitMQ
- Outbox y Saga
- Seguridad
- Observabilidad y continuidad
- Estrategia de entrega y gobierno técnico

## Estructura del Proyecto

El repositorio se encuentra organizado de la siguiente manera:

### Backend_Financiero

Contiene el Backend de la aplicación desarrollado en .NET.

Dentro de este proyecto se encuentran los servicios y APIs encargados de manejar la lógica de negocio, acceso a datos, usuarios, entidades y solicitudes de pago.

### BaseDeDatos

Contiene los scripts utilizados para la creación y configuración de la base de datos


### Documentacion

Contiene la documentación relacionada con el diseño y arquitectura de la solución.

Incluye información sobre:

- Arquitectura de la solución
- Modelo de base de datos
- RabbitMQ y manejo de colas
- Outbox
- Saga y procesos compensatorios
- Seguridad
- Observabilidad
- CI/CD y estrategia de despliegue

### Institucion_Financiera

Contiene el Frontend de la aplicación desarrollado en Angular.

Desde esta aplicación el usuario puede interactuar con las diferentes funcionalidades del sistema y consumir los servicios proporcionados por el Backend.


## Arquitectura

La solución separa el Frontend, Backend, base de datos y procesos de integración.

Para la comunicación asíncrona se utiliza RabbitMQ y para los procesos distribuidos se consideran patrones como Outbox y Saga, permitiendo tener mayor trazabilidad y control de las operaciones.

## Autor

Juan Luis Suchí Martínez