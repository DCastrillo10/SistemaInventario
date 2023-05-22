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
        IBodegaProductoRepository BodegaProducto { get; }
        IInventarioRepository Inventario { get; }
        IInventarioDetalleRepository InventarioDetalle { get; }
        IKardexInventarioRepository KardexInventario { get; }
        Task Guardar();

    }
}
