using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepositorio Bodega { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public ICoordinacionRepositorio Coordinacion { get; private set; }

        public ITareaRepositorio Tarea { get; private set; }
        public ICodigoFuenteRepositorio CodigoFuente { get; private set; }
        public ITipoRepositorio Tipo { get; private set; }


        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
            Categoria = new CategoriaRepositorio(_db);
            Coordinacion =new CoordinacionRepositorio(_db);
            Tipo = new TipoRepositorio(_db);
            Tarea = new TareaRepositorio(_db);
            CodigoFuente = new CodigoFuenteRepositorio(_db);
            //Marca = new MarcaRepositorio(_db);
            //Producto = new ProductoRepositorio(_db);
            //UsuarioAplicacion = new UsuarioAplicacionRepositorio(_db);
            //BodegaProducto = new BodegaProductoRepositorio(_db);
            //Inventario = new InventarioRepositorio(_db);
            //InventarioDetalle = new InventarioDetalleRepositorio(_db);
            //KardexInventario = new KardexInventarioRepositorio(_db);
            //Compania = new CompaniaRepositorio(_db);
            //CarroCompra = new CarroCompraRepositorio(_db);
            //Orden = new OrdenRepositorio(_db);
            //OrdenDetalle = new OrdenDetalleRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
