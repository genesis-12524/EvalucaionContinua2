using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;
using PortalInmobiliario.Data;

namespace PortalInmobiliario.Controllers;

    public class InmueblesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InmueblesController> _logger;

        public InmueblesController(ApplicationDbContext context, ILogger<InmueblesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Inmuebles
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
                    // Si hay errores, recargar las ciudades y devolver la vista con errores
                    await CargarListas(filtros);
                    filtros.Inmuebles = new List<Inmueble>();
                    return View(filtros);
                }

                // Consulta base - solo inmuebles activos
                var query = _context.Inmuebles.Where(i => i.Activo).AsQueryable();

                // Aplicar filtros
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

                // Obtener total antes de paginación
                filtros.TotalInmuebles = await query.CountAsync();

                // Calcular paginación
                filtros.TotalPaginas = (int)Math.Ceiling(filtros.TotalInmuebles / (double)filtros.TamanoPagina);
                
                // Asegurar que la página esté en rango válido
                if (filtros.Pagina < 1) filtros.Pagina = 1;
                if (filtros.Pagina > filtros.TotalPaginas) filtros.Pagina = filtros.TotalPaginas;

                // Aplicar paginación
                var inmuebles = await query
                    .OrderBy(i => i.Precio)
                    .Skip((filtros.Pagina - 1) * filtros.TamanoPagina)
                    .Take(filtros.TamanoPagina)
                    .ToListAsync();

                filtros.Inmuebles = inmuebles;

                // Cargar listas para dropdowns
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

        // GET: Inmuebles/Details/5
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
            // Cargar ciudades únicas para el dropdown
            filtros.Ciudades = await _context.Inmuebles
                .Where(i => i.Activo)
                .Select(i => i.Ciudad)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
    }
