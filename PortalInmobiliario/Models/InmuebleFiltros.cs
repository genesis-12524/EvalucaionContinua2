using System.ComponentModel.DataAnnotations;

namespace PortalInmobiliario.Models;

    public class InmuebleFiltros
    {
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Display(Name = "Tipo de Inmueble")]
        public TipoInmueble? Tipo { get; set; }

        [Display(Name = "Precio Mínimo")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo no puede ser negativo")]
        public decimal? PrecioMin { get; set; }

        [Display(Name = "Precio Máximo")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio máximo no puede ser negativo")]
        public decimal? PrecioMax { get; set; }

        [Display(Name = "Mín. Dormitorios")]
        [Range(0, 10, ErrorMessage = "El número de dormitorios debe estar entre 0 y 10")]
        public int? DormitoriosMin { get; set; }

        
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 6;
        public int TotalPaginas { get; set; }
        public int TotalInmuebles { get; set; }

      
        public List<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();

       
        public List<string> Ciudades { get; set; } = new List<string>();
    }
