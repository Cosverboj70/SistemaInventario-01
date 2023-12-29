using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.Modelos.ViewModels
{
    public class MunicipioVM
    {
        public Municipio Municipio { get; set; }

        public IEnumerable<SelectListItem> CoordinacionLista { get; set; }

    }
}