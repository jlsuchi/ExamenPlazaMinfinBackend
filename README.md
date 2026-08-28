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

## Ejecución

Para ejecutar el backend:

dotnet restore
dotnet run

## Autor

Juan Luis Suchí Martínez