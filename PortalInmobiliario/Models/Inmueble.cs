
using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;

    public class Inmueble
    {
        public int Id { get; set; }
        
        [Required, StringLength(20)]
        public string Codigo { get; set; }  
        
        [Required, StringLength(100)]
        public string Titulo { get; set; }
        
        public string Imagen { get; set; }
        
        [Required]
        public TipoInmueble Tipo { get; set; }
        
        [Required, StringLength(50)]
        public string Ciudad { get; set; }
        
        [Required, StringLength(200)]
        public string Direccion { get; set; }
        
        [Range(1, 10)]
        public int Dormitorios { get; set; }
        
        [Range(1, 10)]
        public int Banos { get; set; }
        
        [Range(1, 1000)]
        public int MetrosCuadrados { get; set; }
        
        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }
        
        public bool Activo { get; set; } = true;
        
       
        public ICollection<Visita> Visitas { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }

    public enum TipoInmueble
    {
        Departamento,
        Casa,
        Oficina,
        Local
    }
