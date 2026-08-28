export interface SolicitudPago {
  idSolicitudPago: number;
  entidad: number;
  unidadEjecutora: number;
  unidadDesconcentrada: number;
  idUsuario: number;
  idCuenta: number;
  descripcion: string;
  monto: number;
  estado?: string;
  fechaCreacion?: Date;
  fechaActualizacion?: Date;
}