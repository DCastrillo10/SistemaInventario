using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository.IRepository
{
    public  interface IMarcaRepository: IRepository<Marca>
    {
        void Actualizar(Marca marca);
    }
}
