using PortalInmobiliario.Models;

namespace PortalInmobiliario.Services;

    public interface IVisitaService
    {
        Task<bool> ExisteVisitaSolapadaAsync(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, int? visitaId = null);
        Task<bool> EsHorarioLaboralAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<List<Visita>> ObtenerVisitasDelDiaAsync(int inmuebleId, DateTime fecha);
    }

    public class VisitaService : IVisitaService
    {
        private readonly ApplicationDbContext _context;

        public VisitaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteVisitaSolapadaAsync(int inmuebleId, DateTime fechaInicio, DateTime fechaFin, int? visitaId = null)
        {
            
            var visitasSolapadas = await _context.Visitas
                .Where(v => v.InmuebleId == inmuebleId &&
                           v.Estado != EstadoVisita.Cancelada && 
                           v.Id != visitaId && 
                           ((fechaInicio >= v.FechaInicio && fechaInicio < v.FechaFin) ||
                            (fechaFin > v.FechaInicio && fechaFin <= v.FechaFin) ||
                            (fechaInicio <= v.FechaInicio && fechaFin >= v.FechaFin)))
                .ToListAsync();

            return visitasSolapadas.Any();
        }

        public async Task<bool> EsHorarioLaboralAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            
            if (fechaInicio.DayOfWeek == DayOfWeek.Saturday || fechaInicio.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            
            var horaInicio = fechaInicio.TimeOfDay;
            var horaFin = fechaFin.TimeOfDay;
            var horarioLaboralInicio = new TimeSpan(8, 0, 0);
            var horarioLaboralFin = new TimeSpan(19, 0, 0);

            return horaInicio >= horarioLaboralInicio && 
                   horaFin <= horarioLaboralFin && 
                   horaInicio < horaFin;
        }

        public async Task<List<Visita>> ObtenerVisitasDelDiaAsync(int inmuebleId, DateTime fecha)
        {
            return await _context.Visitas
                .Where(v => v.InmuebleId == inmuebleId &&
                           v.FechaInicio.Date == fecha.Date &&
                           v.Estado != EstadoVisita.Cancelada)
                .OrderBy(v => v.FechaInicio)
                .ToListAsync();
        }
    }
