namespace PortalInmobiliario.Models;

    public class ReservarVista
    {
        public int InmuebleId { get; set; }
        public string TituloInmueble { get; set; }
        public string CodigoInmueble { get; set; }
        public bool PuedeReservar { get; set; }
        public string Mensaje { get; set; }
        public Reserva ReservaExistente { get; set; }
    }
