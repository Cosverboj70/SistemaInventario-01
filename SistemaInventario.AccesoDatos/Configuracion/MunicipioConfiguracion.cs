﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class MunicipioConfiguracion : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.CoordinacionId).IsRequired();

            /* Relaciones*/
            /* HasOne==>Relacion de uno a Muchos*/

            builder.HasOne(x => x.Coordinacion).WithMany()
                   .HasForeignKey(x => x.CoordinacionId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
