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
    public class CoordinacionRepositorio : Repositorio<Coordinacion>, ICoordinacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CoordinacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Coordinacion coordinacion)
        {
            var coordinacionBD = _db.Coordinaciones.FirstOrDefault(b => b.Id == coordinacion.Id);
            if (coordinacionBD != null)
            {
                coordinacionBD.Nombre = coordinacion.Nombre;
                coordinacionBD.Descripcion = coordinacion.Descripcion;
                coordinacionBD.Estado = coordinacion.Estado;
                _db.SaveChanges();
            }
        }

    }


}
