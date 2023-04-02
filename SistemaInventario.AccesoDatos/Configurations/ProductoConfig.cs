using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Configurations
{
    public class ProductoConfig: IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.ImgUrl).IsRequired(false);

            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.PadreId).IsRequired(false);

            //Foreing Keys
            builder.HasOne(c=>c.Categoria).WithMany().HasForeignKey(i => i.CategoriaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m=>m.Marca).WithMany().HasForeignKey(i => i.MarcaId).OnDelete(DeleteBehavior.NoAction);

            //Recursividad
            builder.HasOne(p => p.Padre).WithMany().HasForeignKey(i => i.PadreId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
