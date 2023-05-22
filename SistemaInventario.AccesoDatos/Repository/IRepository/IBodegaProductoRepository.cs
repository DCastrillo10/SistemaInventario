using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository.IRepository
{
    public  interface IBodegaProductoRepository : IRepository<BodegaProducto>
    {
        void Actualizar(BodegaProducto bodegaProducto);

    }
}
