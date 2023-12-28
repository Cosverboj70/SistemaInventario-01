using System;
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


        Task Guardar();
    }
}
