using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;

    public class AgendarVisita
    {
        public int InmuebleId { get; set; }
        public string TituloInmueble { get; set; }
        public string CodigoInmueble { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [Display(Name = "Fecha y Hora de Inicio")]
        public DateTime FechaInicio { get; set; } = DateTime.Today.AddHours(9); // Por defecto 9:00 AM

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [Display(Name = "Fecha y Hora de Fin")]
        public DateTime FechaFin { get; set; } = DateTime.Today.AddHours(10); // Por defecto 10:00 AM

        [Display(Name = "Notas adicionales")]
        [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres")]
        public string Notas { get; set; }

       
        public List<Visita> VisitasExistentes { get; set; } = new List<Visita>();
        public bool TieneReservaActiva { get; set; }
    }
