using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICoordinacionRepositorio : IRepositorio<Coordinacion>
    {
        void Actualizar(Coordinacion coordinacion);
    }


}
