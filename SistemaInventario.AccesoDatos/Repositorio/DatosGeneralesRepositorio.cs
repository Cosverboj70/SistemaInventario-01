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
    public class DatosGeneralesRepositorio : Repositorio<DatosGenerales>, IDatosGeneralesRepositorio
    {

        private readonly ApplicationDbContext _db;

        public DatosGeneralesRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(DatosGenerales DatosGenerales)
        {
           var datosBD = _db.DatosGenerales.FirstOrDefault(b => b.Id == DatosGenerales.Id);
            if(datosBD != null)
            {
                datosBD.Titular= DatosGenerales.Titular;
                datosBD.Puesto = DatosGenerales.Puesto;
                datosBD.Direccion = DatosGenerales.Direccion;
                datosBD.Colonia = DatosGenerales.Colonia;
                datosBD.Telefonos = DatosGenerales.Telefonos;
                datosBD.Email = DatosGenerales.Email;
                datosBD.SiglasCom = DatosGenerales.SiglasCom;
                datosBD.SiglasOp = DatosGenerales.SiglasOp;
                datosBD.SiglasIns = DatosGenerales.SiglasIns;
                datosBD.SiglasEvi = DatosGenerales.SiglasEvi;
                datosBD.Estado = DatosGenerales.Estado;
                _db.SaveChanges();
            }
        }
    }
}
