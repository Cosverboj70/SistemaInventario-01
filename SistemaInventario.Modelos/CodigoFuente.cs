using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class CodigoFuente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(150, ErrorMessage ="Nombre debe ser Maximo 150 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        //[MaxLength(100, ErrorMessage = "Descripcion debe ser Maximo 100 Caracteres")]
        public string Descripcion { get; set; }
        public string Web { get; set; }

        [Required(ErrorMessage = "Fecha es Requerida")]
        public DateTime FechaCreacion { get; set; }
        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }
    }
}
