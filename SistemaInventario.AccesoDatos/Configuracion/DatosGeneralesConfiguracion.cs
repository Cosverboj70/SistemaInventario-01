using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class DatosGeneralesConfiguracion : IEntityTypeConfiguration<DatosGenerales>
    {
        public void Configure(EntityTypeBuilder<DatosGenerales> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Titular).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Puesto).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Direccion).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Colonia).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Telefonos).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.SiglasCom).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.SiglasOp).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.SiglasIns).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.SiglasEvi).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
