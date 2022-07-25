﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Datos.Repositorio.IRespositorio
{
    public interface IUnidadTrabajo: IDisposable
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        IProductoRepositorio Producto { get; }
        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }

        void Guardar();
    }
}