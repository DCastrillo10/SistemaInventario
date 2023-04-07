using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository.IRepository
{
    public interface IUnitWork: IDisposable
    {
        IBodegaRepository Bodega { get; }
        ICategoriaRepository Categoria { get; }
        IMarcaRepository Marca { get; }
        IProductoRepository Producto { get; }
        IUsuarioAplicacionRepository UsuarioAplicacion { get; }
        Task Guardar();

    }
}
