using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class DatosGenerales
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Titular { get; set; }
        public string Puesto { get; set; }
        public string Direccion { get; set; }
        public string Colonia { get; set; }

        public string Telefonos { get; set; }
        public string Email { get; set; }
        public string SiglasCom { get; set; }
        public string SiglasOp { get; set; }
        public string SiglasIns { get; set; }
        public string SiglasEvi { get; set; }
        public int Estado { get; set; }

    }
}
