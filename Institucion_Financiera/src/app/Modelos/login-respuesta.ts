
export interface LoginRespuesta {
  mensaje: string;
  usuario: {
    idUsuario: number;
    usuario: string;
    nombre: string;
    estado: boolean;
  };
}