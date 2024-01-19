using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class TareaRepositorio : Repositorio<Tareas>, ITareaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TareaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tareas tarea)
        {
            var tareaBD = _db.Tareas.FirstOrDefault(b => b.Id == tarea.Id);
            if (tareaBD != null)
            {
                tareaBD.Nombre = tarea.Nombre;
                tareaBD.Web = tarea.Web;
                tareaBD.Descripcion = tarea.Descripcion;
                tareaBD.Estado = tarea.Estado;
                _db.SaveChanges();
            }
        }
    }
}
