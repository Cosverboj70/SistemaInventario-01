using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IMunicipioRepositorio : IRepositorio<Municipio>
    {
        void Actualizar(Municipio municipio);

        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

    }
}
