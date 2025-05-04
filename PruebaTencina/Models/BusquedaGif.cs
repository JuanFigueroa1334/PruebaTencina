using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTencina.Models
{
    [Table("busquedagif")]
    public class BusquedaGif
    {
        public int Id { get; set; }
        public DateTime? FechaBusqueda { get; set; }
        public string CatFact { get; set; } = string.Empty;
        public string? ParamSearch { get; set; }
        public string? UrlGif { get; set; }
    }
}
