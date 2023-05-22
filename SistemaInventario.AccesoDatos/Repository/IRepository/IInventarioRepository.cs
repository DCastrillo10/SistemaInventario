using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository.IRepository
{
    public  interface IInventarioRepository : IRepository<Inventario>
    {
        void Actualizar(Inventario inventario);

        IEnumerable<SelectListItem> ObtenerTodosDropDownLista(string obj);

    }
}
