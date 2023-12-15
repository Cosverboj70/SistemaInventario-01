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
    public class TipoRepositorio : Repositorio<Tipo>, ITipoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TipoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tipo tipo)
        {
            var tipoBD = _db.Tipos.FirstOrDefault(b => b.Id == tipo.Id);
            if (tipoBD != null)
            {
                tipoBD.Nombre = tipo.Nombre;
                tipoBD.Descripcion = tipo.Descripcion;
                tipoBD.Estado = tipo.Estado;
                _db.SaveChanges();
            }
        }
    }
}
