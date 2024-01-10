﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {

        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        ICoordinacionRepositorio Coordinacion { get; }
        IProductoRepositorio Producto { get; }
        ITareaRepositorio Tarea { get; }
        ICodigoFuenteRepositorio CodigoFuente { get; }
        ITipoRepositorio Tipo { get; }
        IMunicipioRepositorio Municipio { get; }

        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }
		IBodegaProductoRepositorio BodegaProducto { get; }
		IInventarioRepositorio Inventario { get; }
		IInventarioDetalleRepositorio InventarioDetalle { get; }
		IKardexInventarioRepositorio KardexInventario { get; }
        ICompaniaRepositorio Compania { get; }
        ICarroCompraRepositorio CarroCompra { get; }

        IOrdenRepositorio Orden { get; }

        IOrdenDetalleRepositorio OrdenDetalle { get; }

        Task Guardar();
    }
}
