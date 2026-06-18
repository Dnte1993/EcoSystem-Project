using System.Collections.Generic;
using System.Threading.Tasks;
using EcoSystem.Data.Models;

namespace EcoSystem.Business.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetProductosAsync();
        Task<Producto> GetProductoByIdAsync(int id);
        Task<Producto> CreateProductoAsync(Producto producto);
        Task<bool> UpdateProductoAsync(int id, Producto producto);
        Task<bool> DeleteProductoAsync(int id);
    }
}