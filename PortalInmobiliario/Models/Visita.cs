using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalInmobiliario.Models;

    public class Visita
    {
        public int Id { get; set; }
        
        public int InmuebleId { get; set; }
        
        [Required]
        public string UsuarioId { get; set; }
        
        [Required]
        public DateTime FechaInicio { get; set; }
        
        [Required]
        public DateTime FechaFin { get; set; }
        
        [Required]
        public EstadoVisita Estado { get; set; } = EstadoVisita.Solicitada;
        
        [StringLength(500)]
        public string Notas { get; set; }
        
        
        [ForeignKey("InmuebleId")]
        public Inmueble Inmueble { get; set; }
        
        [ForeignKey("UsuarioId")]
        public ApplicationUser Usuario { get; set; }
    }

    public enum EstadoVisita
    {
        Solicitada,
        Confirmada,
        Cancelada
    }
