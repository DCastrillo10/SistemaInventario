using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository
{
    public class UnitWork : IUnitWork
    {
        private readonly ApplicationDbContext _db;
        public IBodegaRepository Bodega { get; private set; }
        public ICategoriaRepository Categoria { get; private set; }
        public IMarcaRepository Marca {get; private set; }
        public  IProductoRepository Producto {get; private set; }

        public UnitWork(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepository(_db);
            Categoria = new CategoriaRepository(_db);
            Marca = new MarcaRepository(_db);
            Producto = new ProductoRepository(_db);
        }
   
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
