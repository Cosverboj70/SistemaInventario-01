using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaInventario.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        public IEnumerable<SelectListItem> CategoriaLista { get; set; }
        public IEnumerable<SelectListItem> MarcaLista { get; set; }
        public IEnumerable<SelectListItem> PadreLista { get; set; }

    }
}