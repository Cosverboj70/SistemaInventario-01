using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class MunicipioRepositorio : Repositorio<Municipio>, IMunicipioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public MunicipioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Municipio municipio)
        {
            var municipioBD = _db.Municipios.FirstOrDefault(b => b.Id == municipio.Id);
            if (municipioBD != null)
            {
                municipioBD.Nombre = municipio.Nombre;
                municipioBD.Descripcion = municipio.Descripcion;
                municipioBD.Estado = municipio.Estado;
                municipioBD.CoordinacionId = municipio.CoordinacionId;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if (obj == "Coordinacion")
            {
                return _db.Coordinaciones.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
