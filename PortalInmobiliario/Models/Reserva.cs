using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalInmobiliario.Models;


    public class Reserva
    {
        public int Id { get; set; }
        
        public int InmuebleId { get; set; }
        
        [Required]
        public string UsuarioId { get; set; }
        
        [Required]
        public DateTime FechaExpiracion { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        
        [ForeignKey("InmuebleId")]
        public Inmueble Inmueble { get; set; }
        
        [ForeignKey("UsuarioId")]
        public ApplicationUser Usuario { get; set; }
    }
