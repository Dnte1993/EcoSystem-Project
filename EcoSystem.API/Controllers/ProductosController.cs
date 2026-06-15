using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoSystem.Data;
using EcoSystem.Data.Models;

namespace EcoSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            // Consultamos la tabla Productos en Supabase
            var productos = await _context.Productos.ToListAsync();

            // Devolvemos un código HTTP 200 OK junto con los datos
            return Ok(productos);
        }
    }
}