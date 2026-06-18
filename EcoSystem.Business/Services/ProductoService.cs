using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;
using EcoSystem.Data.Models;
using EcoSystem.Business.Interfaces;

namespace EcoSystem.Business.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        // Aquí la capa de Negocio recibe la base de datos (Data)
        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetProductosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<Producto> CreateProductoAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> UpdateProductoAsync(int id, Producto producto)
        {
            if (id != producto.Id) return false;

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}