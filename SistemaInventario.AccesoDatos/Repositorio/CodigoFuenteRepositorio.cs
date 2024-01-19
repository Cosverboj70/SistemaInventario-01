using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class CodigoFuenteRepositorio : Repositorio<CodigoFuente>, ICodigoFuenteRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CodigoFuenteRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(CodigoFuente codigoFuente)
        {
            var codigoFuenteBD = _db.CodigoFuentes.FirstOrDefault(b => b.Id == codigoFuente.Id);
            if (codigoFuenteBD != null)
            {
                codigoFuenteBD.Nombre = codigoFuente.Nombre;
                codigoFuenteBD.Web = codigoFuente.Web;
                codigoFuenteBD.Descripcion = codigoFuente.Descripcion;
                codigoFuenteBD.Estado = codigoFuente.Estado;
                _db.SaveChanges();
            }
        }
    }
}
