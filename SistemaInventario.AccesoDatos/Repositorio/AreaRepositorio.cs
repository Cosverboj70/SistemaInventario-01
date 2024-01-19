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
    public class AreaRepositorio : Repositorio<Area>, IAreaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public AreaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Area area)
        {
            var areaBD = _db.Areas.FirstOrDefault(b => b.Id == area.Id);
            if (areaBD != null)
            {
                areaBD.Nombre = area.Nombre;
                areaBD.Descripcion = area.Descripcion;
                areaBD.Estado = area.Estado;
                _db.SaveChanges();
            }
        }

    }


}
