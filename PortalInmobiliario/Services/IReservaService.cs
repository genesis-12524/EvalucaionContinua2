using PortalInmobiliario.Models;

namespace PortalInmobiliario.Services;

    public interface IReservaService
    {
        Task<bool> TieneReservaActivaAsync(int inmuebleId);
        Task<Reserva> CrearReservaAsync(int inmuebleId, string usuarioId);
        Task<bool> LiberarReservaAsync(int reservaId);
        Task<Reserva> ObtenerReservaActivaAsync(int inmuebleId);
    }

    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TieneReservaActivaAsync(int inmuebleId)
        {
            return await _context.Reservas
                .AnyAsync(r => r.InmuebleId == inmuebleId && 
                              r.FechaExpiracion > DateTime.Now);
        }

        public async Task<Reserva> ObtenerReservaActivaAsync(int inmuebleId)
        {
            return await _context.Reservas
                .Include(r => r.Inmueble)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.InmuebleId == inmuebleId && 
                                         r.FechaExpiracion > DateTime.Now);
        }

        public async Task<Reserva> CrearReservaAsync(int inmuebleId, string usuarioId)
        {
            var reserva = new Reserva
            {
                InmuebleId = inmuebleId,
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddHours(48) // 48 horas de reserva
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return reserva;
        }

        public async Task<bool> LiberarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
