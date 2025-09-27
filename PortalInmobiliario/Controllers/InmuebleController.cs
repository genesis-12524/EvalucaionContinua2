using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using PortalInmobiliario.Data;
using Microsoft.AspNetCore.Authorization;
using PortalInmobiliario.Services;

namespace PortalInmobiliario.Controllers;




public class InmueblesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<InmueblesController> _logger;

    private readonly IVisitaService _visitaService;
    private readonly IReservaService _reservaService;

    public InmueblesController(ApplicationDbContext context, ILogger<InmueblesController> logger, IVisitaService visitaService, IReservaService reservaService)
    {
        _context = context;
        _logger = logger;
        _visitaService = visitaService;
        _reservaService = reservaService;
    }

    
    public async Task<IActionResult> Index(InmuebleFiltros filtros)
    {
        try
        {
            // Validación del rango de precios
            if (filtros.PrecioMin.HasValue && filtros.PrecioMax.HasValue &&
                filtros.PrecioMin > filtros.PrecioMax)
            {
                ModelState.AddModelError("PrecioMax", "El precio máximo debe ser mayor o igual al precio mínimo");
            }

            if (!ModelState.IsValid)
            {
                
                await CargarListas(filtros);
                filtros.Inmuebles = new List<Inmueble>();
                return View(filtros);
            }

            
            var query = _context.Inmuebles.Where(i => i.Activo).AsQueryable();

            
            if (!string.IsNullOrEmpty(filtros.Ciudad))
            {
                query = query.Where(i => i.Ciudad.Contains(filtros.Ciudad));
            }

            if (filtros.Tipo.HasValue)
            {
                query = query.Where(i => i.Tipo == filtros.Tipo.Value);
            }

            if (filtros.PrecioMin.HasValue)
            {
                query = query.Where(i => i.Precio >= filtros.PrecioMin.Value);
            }

            if (filtros.PrecioMax.HasValue)
            {
                query = query.Where(i => i.Precio <= filtros.PrecioMax.Value);
            }

            if (filtros.DormitoriosMin.HasValue)
            {
                query = query.Where(i => i.Dormitorios >= filtros.DormitoriosMin.Value);
            }

           
            filtros.TotalInmuebles = await query.CountAsync();

           
            filtros.TotalPaginas = (int)Math.Ceiling(filtros.TotalInmuebles / (double)filtros.TamanoPagina);

            
            if (filtros.Pagina < 1) filtros.Pagina = 1;
            if (filtros.Pagina > filtros.TotalPaginas) filtros.Pagina = filtros.TotalPaginas;

           
            var inmuebles = await query
                .OrderBy(i => i.Precio)
                .Skip((filtros.Pagina - 1) * filtros.TamanoPagina)
                .Take(filtros.TamanoPagina)
                .ToListAsync();

            filtros.Inmuebles = inmuebles;

            
            await CargarListas(filtros);

            return View(filtros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar el catálogo de inmuebles");
            ModelState.AddModelError("", "Ocurrió un error al cargar los inmuebles.");
            await CargarListas(filtros);
            return View(filtros);
        }
    }

    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inmueble = await _context.Inmuebles
            .FirstOrDefaultAsync(m => m.Id == id && m.Activo);

        if (inmueble == null)
        {
            return NotFound();
        }

        return View(inmueble);
    }

    private async Task CargarListas(InmuebleFiltros filtros)
    {
        
        filtros.Ciudades = await _context.Inmuebles
            .Where(i => i.Activo)
            .Select(i => i.Ciudad)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }
}
