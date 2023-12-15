using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class TareaConfiguracion : IEntityTypeConfiguration<Tareas>
    {
        public void Configure(EntityTypeBuilder<Tareas> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Descripcion).IsRequired(false);
            builder.Property(x => x.Web).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.FechaCreacion).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
